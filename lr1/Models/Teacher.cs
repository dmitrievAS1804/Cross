using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lr1.Models
{
    public class Teacher
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public string Surname { set; get; }
        public string Patronymic { set; get; }
        public virtual Course Course { set; get; }

        public void AddTeacherCourse(Course Course, Teacher Teacher)
        {
            Teacher.Course = Course;
        }

    }
}
