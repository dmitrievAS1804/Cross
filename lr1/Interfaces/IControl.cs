using lr1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lr1.Interfaces
{
   public interface IControl
    {
        List<Student> AllSutdCour(List<Student> Student,  int id);
        List<Teacher> FindTeacher(List<Teacher> Teacher, int id);

    }
}
