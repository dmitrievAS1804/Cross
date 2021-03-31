using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lr1.Models
{
    public class AllContext: DbContext
    {
        public DbSet<School> School { get; set; }
        public DbSet<Student> Student { get; set; }

        public AllContext (DbContextOptions<AllContext> options)
        : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
    }
}
