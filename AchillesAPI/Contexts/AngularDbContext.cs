using AchillesAPI.DbModels;
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
        public DbSet<StageDbModel> Stages { get; set; }
    }
}
