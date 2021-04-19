using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lr1.Models
{
    public class AllContext: DbContext
    {
        public DbSet<Course> Corurse { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<Teacher> Teacher { get; set; }
        public DbSet<CourseStudent> CourseStudent { get; set; }



        public AllContext (DbContextOptions<AllContext> options)
        : base(options)
        {
          //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
