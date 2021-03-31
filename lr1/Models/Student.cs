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
        public string Class { set; get; }
        public int Age { set; get; }

        public virtual School School { set; get; }

    }
}
