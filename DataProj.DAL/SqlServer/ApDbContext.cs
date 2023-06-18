using DataProj.DAL.SqlServer.Configuration;
using DataProj.Domain.Models.Abstractions.BaseUsers;
using DataProj.Domain.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataProj.DAL.SqlServer
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<ProjectEmployee> ProjectEmployees { get; set; }
        public DbSet<TaskItem> ProjectTasks { get; set; }

        public AppDbContext() : base()
        {
        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(ProjectConfiguration).Assembly);


            builder.Entity<ProjectEmployee>()
                .HasKey(pe => new { pe.ProjectId, pe.EmployeeId });

            builder.Entity<ProjectEmployee>()
                .HasOne(pe => pe.Project)
                .WithMany(p => p.Employees)
                .HasForeignKey(pe => pe.ProjectId);

            builder.Entity<ProjectEmployee>()
                .HasOne(pe => pe.Employee)
                .WithMany(e => e.Projects)
                .HasForeignKey(pe => pe.EmployeeId);

            builder.Entity<Employee>()
                .HasMany(employee => employee.AuthoredTasks)
                .WithOne(task => task.Author)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(task => task.AuthorId);

            builder.Entity<Employee>()
               .HasMany(employee => employee.ExecutorTasks)
               .WithOne(task => task.Executor)
               .OnDelete(DeleteBehavior.NoAction)
               .HasForeignKey(task => task.ExecutorId);


            builder.Entity<Project>()
                .HasOne(p => p.ProjectManager)
                .WithMany()
                .HasForeignKey(p => p.ProjectManagerId)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
