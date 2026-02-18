using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models.Entities;

namespace WebApplication2.Models.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
         public DbSet<Department> Departments { get; set; }
         public DbSet<Employee> Employees { get; set; }
         public DbSet<Attendance>Attendances { get; set; }
         public DbSet<EmployeeTask> Tasks { get; set; }
         public DbSet<LeaveRequest>LeaveRequests { get; set; }
         public DbSet<Salary>Salaries { get; set; }




        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

    }
}
