using AT.Server.Models;
using AT.Share.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System;
using System.Diagnostics;
using System.Reflection.Emit;

namespace AT.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<GroupUser> GroupUsers { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<TaskModel> Tasks { get; set; }
        public DbSet<TaskProgressHistory> TaskProgressHistorys { get; set; }
        public DbSet<ProgressStep> ProgressSteps { get; set; }
        public DbSet<ProgressUpdate> ProgressUpdates { get; set; }
        public DbSet<StepModel> StepModels { get; set; }
        // DbSet cho ProjectTypeModel
        public DbSet<ProjectTypeModel> ProjectTypes { get; set; }

        // DbSet cho ProgressModel
        public DbSet<ProgressModel> Progresses { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }
}