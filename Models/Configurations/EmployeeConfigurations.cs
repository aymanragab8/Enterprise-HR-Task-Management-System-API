using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication2.Models.Entities;

namespace WebApplication2.Models.Configurations
{
    public class EmployeeConfigurations : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => e.Email).IsUnique();
            builder.HasQueryFilter(e => !e.IsDeleted);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(100); 
            builder.Property(e => e.Email).IsRequired().HasMaxLength(100);
            builder.Property(e => e.PhoneNumber).HasMaxLength(20); 
            builder.Property(e => e.Address).HasMaxLength(200); 
            builder.Property(e => e.JobTitle).HasMaxLength(50); 
            builder.Property(e => e.Salary).HasColumnType("decimal(10,2)"); 
            builder.Property(e => e.Status).IsRequired(); 
            builder.Property(e => e.IsDeleted).IsRequired(); 
            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.UpdatedAt).IsRequired(false);

            builder.HasOne(e => e.Department) 
                .WithMany(d => d.Employees) 
                .HasForeignKey(e => e.DepartmentId) 
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
