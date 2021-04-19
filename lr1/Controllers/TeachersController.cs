using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lr1.Models;
using lr1.Interfaces;

namespace lr1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly AllContext _context;
        private readonly IControl _mng;

        public TeachersController(AllContext context, IControl mng)
        {
            _context = context;
            _mng = mng;
        }

        // GET: api/Teachers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Teacher>>> GetTeacher()
        {
            var tech = _context.Teacher.Include(p => p.Course);
            return await tech.ToArrayAsync();
        }

        // GET: api/Teachers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Teacher>> GetTeacher(int id)
        {
            var teacher = _context.Teacher.Include(p => p.Course).FirstOrDefault(i=>i.ID==id);

            if (teacher == null)
            {
                return NotFound();
            }

            return teacher;
        }

        // PUT: api/Teachers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeacher(int id, Teacher teacher)
        {
            if (id != teacher.ID)
            {
                return BadRequest();
            }

            _context.Entry(teacher).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherExists(id))
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

        [HttpPut("ChangeCourse")]
        public async Task<IActionResult> ChangeCourse(int id, int CourID)
        {
            var Course = _context.Corurse.FirstOrDefault(c => c.ID == CourID);
            if (Course != null)
            {
                var teacher = _context.Teacher.FirstOrDefault(c => c.ID == id);
                if (teacher != null)
                {
                    teacher.Course = Course;
                    teacher.Course.ID = Course.ID;
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetTeacher", new { id = teacher.ID }, teacher);
                }
                else
                {
                    return CreatedAtAction("DeleteCoursefromStudent", new
                    {
                        result = "Не найден преподаватель с таким ID"
                    });
                }
            }
            else
            {
                return CreatedAtAction("DeleteCoursefromStudent", new
                {
                    result = "Не найден курс с таким ID"
                });
            }
        }


        // POST: api/Teachers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Teacher>> PostTeacher(Teacher teacher)
        {
            _context.Teacher.Add(teacher);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetTeacher", new { id = teacher.ID }, teacher);
        }

        [HttpPost("AddCourse")]
        public async Task<ActionResult<Teacher>> AddCourse([FromForm] int CourseID, [FromForm] int TeacherID)
        {
            var Course = _context.Corurse.FirstOrDefault(c=>c.ID==CourseID);
            if (Course != null)
            {
                var teacher = _context.Teacher.FirstOrDefault(c => c.ID == TeacherID);
                if (teacher != null)
                {
                    teacher.AddTeacherCourse(Course, teacher);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetTeacher", new { id = teacher.ID }, teacher);
                }
                else
                {
                    return CreatedAtAction("DeleteCoursefromStudent", new
                    {
                        result = "Не найден преподаватель с таким ID"
                    });
                }
            }
            else
            {
                return CreatedAtAction("DeleteCoursefromStudent", new
                {
                    result = "Не найден курс с таким ID"
                });
            }
            
        }

        // DELETE: api/Teachers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Teacher>> DeleteTeacher(int id)
        {
            var teacher = await _context.Teacher.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }

            _context.Teacher.Remove(teacher);
            await _context.SaveChangesAsync();

            return teacher;
        }

        [HttpDelete("DeleteCoursefromTeacher")]
        public async Task<ActionResult<Student>> DeleteTeacherCorse([FromForm] int Curid, [FromForm] int Teacherid)
        {

            var Teacher = _context.Teacher.Include(i=>i.Course).FirstOrDefault(b=>b.ID==Teacherid);
            if (Teacher == null)
            {
                return CreatedAtAction("DeleteTeacherCorse", new
                {
                    result = "Не найден преподаватель с таким ID"
                });
            }
            else
            {
                if (Teacher.Course != null)
                {
                    Course NewCourse = new Course();
                    Teacher.Course = NewCourse;
                    Teacher.Course.ID = 5;
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("DeleteTeacherCorse", new
                    {
                        result = "удален"
                    });
                }
                else
                {
                    return CreatedAtAction("DeleteTeacherCorse", new
                    {
                        result = "Не найден курс с таким ID у преподавателя"
                    });
                }
            }

        }


        private bool TeacherExists(int id)
        {
            return _context.Teacher.Any(e => e.ID == id);
        }
    }
}
