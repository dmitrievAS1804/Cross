using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lr1.Models
{
    public class Student
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public string Surname { set; get; }
        public string Patronymic { set; get; }

        public List<CourseStudent> CourseStudents { set; get; } = new List<CourseStudent>();

        public void AddStudentCourse (Course Course,int IDStud)
        {
            CourseStudent CS = new CourseStudent();
            CS.Course = Course;
            CS.CourseID = Course.ID;
            CS.StudentID = IDStud;
            CS.Stage = 1;
            CourseStudents.Add(CS);

        }



    }
}
