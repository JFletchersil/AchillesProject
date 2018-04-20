using AchillesAPI.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace AchillesAPI.Contexts
{
    public partial class AngularDbContext : DbContext
    {
        public AngularDbContext(DbContextOptions<AngularDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExerciseType>().HasMany(et => et.Exercises).WithOne(x => x.ExerciseType);
        }

        public DbSet<Stage> Stages { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<ExerciseType> ExerciseTypes { get; set; }
        public DbSet<AdditionalExercise> AdditionalExercises { get; set; }
        public DbSet<UserExercise> UserExercises { get; set; }
        public DbSet<ExerciseStage> ExerciseStages { get; set; }
    }
}
