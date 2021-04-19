using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lr1.Models
{
    public class CourseStudent
    {
        public int ID{ set; get; }
        public int CourseID { set; get; }
        public int Stage { set; get; }
        public int StudentID { set; get; }
        public Course Course { set; get; }
        public Student Student { set; get; }
    }
}
