using lr1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lr1.Interfaces
{
    public class Control : IControl
    {
        public List<Student> AllSutdCour(List<Student> Stud, int id)
        {
            var Students = Stud.Where(Stud => Stud.CourseStudents.Any(i => i.Course.ID == id)).ToList();
            return Students;

        }

        public List<Teacher> FindTeacher(List<Teacher> Teach, int id)
        {
            var Teacher = Teach.Where(Teach => Teach.Course.ID == id).ToList();
            return Teacher;
        }
    }
}
