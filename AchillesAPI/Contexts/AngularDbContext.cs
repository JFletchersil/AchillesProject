using AchillesAPI.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace AchillesAPI.Contexts
{
    /// <summary>
    /// The entity context used to access the database for the exercises.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    public partial class AngularDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AngularDbContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public AngularDbContext(DbContextOptions<AngularDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Override this method to further configure the model that was discovered by convention from the entity types
        /// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting model may be cached
        /// and re-used for subsequent instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context. Databases (and other extensions) typically
        /// define extension methods on this object that allow you to configure aspects of the model that are specific
        /// to a given database.</param>
        /// <remarks>
        /// If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
        /// then this method will not be run.
        /// </remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExerciseType>().HasMany(et => et.Exercises).WithOne(x => x.ExerciseType);
        }

        /// <summary>
        /// Gets or sets the stages.
        /// </summary>
        /// <value>
        /// The stages.
        /// </value>
        public DbSet<Stage> Stages { get; set; }
        /// <summary>
        /// Gets or sets the exercises.
        /// </summary>
        /// <value>
        /// The exercises.
        /// </value>
        public DbSet<Exercise> Exercises { get; set; }
        /// <summary>
        /// Gets or sets the exercise types.
        /// </summary>
        /// <value>
        /// The exercise types.
        /// </value>
        public DbSet<ExerciseType> ExerciseTypes { get; set; }
        /// <summary>
        /// Gets or sets the additional exercises.
        /// </summary>
        /// <value>
        /// The additional exercises.
        /// </value>
        public DbSet<AdditionalExercise> AdditionalExercises { get; set; }
        /// <summary>
        /// Gets or sets the user exercises.
        /// </summary>
        /// <value>
        /// The user exercises.
        /// </value>
        public DbSet<UserExercise> UserExercises { get; set; }
        /// <summary>
        /// Gets or sets the exercise stages.
        /// </summary>
        /// <value>
        /// The exercise stages.
        /// </value>
        public DbSet<ExerciseStage> ExerciseStages { get; set; }
    }
}
