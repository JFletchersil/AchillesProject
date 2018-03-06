using AchillesAPI.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AchillesAPI.Contexts
{
    public class AngularDbContext : DbContext
    {
        public AngularDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<ExerciseType> ExerciseTypes { get; set; }
    }
}
