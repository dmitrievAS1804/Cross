using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lr1.Models;
using lr1.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace lr1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly AllContext _context;
        private readonly IControl _mng;

        public StudentsController(AllContext context, IControl mng)
        {
            _context = context;
            _mng = mng;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudent()
        {
            var stud = _context.Student.Include(p => p.CourseStudents).ThenInclude(i => i.Course);
            return await stud.ToArrayAsync();
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var stud = _context.Student.Include(p => p.CourseStudents).ThenInclude(i => i.Course).FirstOrDefault(i => i.ID == id);

            if (stud == null)
            {
                return NotFound();
            }

            return stud;
        }
        [HttpGet("GetStudentsofCourse/{id}")]
        public CreatedAtActionResult GetStudentsofCourse(int id)
        {
            var Students = _mng.AllSutdCour(_context.Student.Include(c => c.CourseStudents).ThenInclude(b => b.Course).ToList(), id);
            if (Students.Count != 0)
            {
                return CreatedAtAction("GetStudentsofCourse", new
                {
                    result = Students
                });
            }
            else
            {
                return CreatedAtAction("GetStudentsofCourse", new
                {
                    result = "В данном курсе нет учеников"
                });
            }
        }
        [HttpGet("BestStudentsofCourse/{Courid}/{Stg}")]
        public ActionResult BestStudentsofCourse(int CourID,int Stg)
        {
            var stud = _context.CourseStudent.Where(i => i.CourseID == CourID && i.Stage >= Stg).Select(i => i.Student).Select(s => $"{s.Name} {s.Surname} {s.Surname}").ToList();
            return Ok(stud);

                
        }

        [HttpGet("QuantityofStudents/{CourID}")]
        public CreatedAtActionResult QuantityofStudents(int CourID)
        {
            var course = _context.Corurse.FirstOrDefault(i => i.ID == CourID);
            int quantity = _context.CourseStudent.Where(i => i.CourseID == CourID).Count();

            return CreatedAtAction("QuantityofStudents", new
            {
                result = "Количество учеников в курсе:",
                course,quantity
            });

        }

        [HttpGet("AllinfoCourse/{id}")]
        public CreatedAtActionResult AllinfoCourse(int id)
        {
            int a = 1;
            var Students = _mng.AllSutdCour(_context.Student.Include(c => c.CourseStudents).ThenInclude(b => b.Course).ToList(), id);
            var Teacher = _mng.FindTeacher(_context.Teacher.Include(c => c.Course).ToList(), id);
            if ((Students.Count != 0) || (Teacher.Count != 0))
            {
                return CreatedAtAction("AllinfoCourse", new
                {

                    result = Teacher,Students
                });
            }
            else
            {
                return CreatedAtAction("AllinfoCourse", new
                {
                    result = "В данном курсе нет учеников или учителя"
                });
            }
        }
        // PUT: api/Students/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (id != student.ID)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPut("PutStage/{Studid}/{CurID}/{Stg}")]
        public async Task<IActionResult> PutStage(int Studid,int CurID, int Stg)
        {
            var student = _context.CourseStudent.FirstOrDefault(i => i.CourseID == CurID && i.StudentID == Studid);
            if (student != null)
            {
                student.Stage = Stg;
                _context.Entry(student).State = EntityState.Modified;
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(Studid))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // POST: api/Students
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            _context.Student.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.ID }, student);
        }



        [HttpPost("PostStudentCours")]
        public async Task<ActionResult<Student>> PostStudentCours([FromForm] int StudID, [FromForm] int id)
        {

            var Course = _context.Corurse.Find(id);
            var Student = _context.Student.Find(StudID);
            if (Course != null || Student !=null)
            {
                Student.AddStudentCourse(Course, StudID);
                await _context.SaveChangesAsync();
                return CreatedAtAction("PostStudent", new
                {
                    result = "Курс добавлен"
                });
            }
            else
            {
                return CreatedAtAction("PostStudent", new
                {
                    result = "Не найден курс или студент с таким ID"
                });
            }
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Student>> DeleteStudent(int id)
        {
            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Student.Remove(student);
            await _context.SaveChangesAsync();

            return student;
        }

        [HttpDelete("DeleteCoursefromStudent")]
        public async Task<ActionResult<Student>> DeleteCoursefromStudent([FromForm] int Curid, [FromForm] int Studid)
        {

            var Student = _context.Student.Find(Studid);
            if (Student == null)
            {
                return CreatedAtAction("DeleteCoursefromStudent", new
                {
                    result = "Не найден ученик с таким ID"
                });
            }
            else
            {
                var CourseStudents = _context.CourseStudent.FirstOrDefault(r => r.CourseID == Curid && r.StudentID == Studid);
                if (CourseStudents != null)
                {
                    _context.CourseStudent.Remove(CourseStudents);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("DeleteCoursefromStudent", new
                    {
                        result = "удален"
                    });
                }
                else
                {
                    return CreatedAtAction("DeleteCoursefromStudent", new
                    {
                        result = "Не найден курс с таким ID"
                    });
                }
            }

        }


        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.ID == id);
        }
    }
}
