using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepMap.DAL
{
    public class StepMapDbContext : DbContext
    {
        static StepMapDbContext()
        {
            Database.SetInitializer<StepMapDbContext>(null);
        }

        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Step> Steps { get; set; }

        public StepMapDbContext()
            : base("name = StepMapDbContext")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("stepmap");


            modelBuilder.Entity<Project>().ToTable("Project");
            modelBuilder.Entity<Step>().ToTable("Step");

            modelBuilder.Entity<Project>()
                    .HasMany(e => e.FinishedSteps)
                    .WithRequired(e => e.Project)
                    .HasForeignKey(e => e.ProjectId);

            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<User>()
                    .HasMany(e => e.Projects)
                    .WithRequired(e => e.User)
                    .HasForeignKey(e => e.UserId);
        }
    }
}
