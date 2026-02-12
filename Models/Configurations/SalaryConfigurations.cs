using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication2.Models.Entities;

namespace WebApplication2.Models.Configurations
{
    public class SalaryConfigurations : IEntityTypeConfiguration<Salary>
    {
        public void Configure(EntityTypeBuilder<Salary> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.BasicSalary).IsRequired().HasColumnType("decimal(12,2)");
            builder.Property(s => s.Bonuses).HasColumnType("decimal(8,2)");
            builder.Property(s => s.Deductions).HasColumnType("decimal(8,2)");
            builder.Ignore(s => s.NetSalary);

            builder.HasOne(s => s.Employee)
                   .WithOne(e => e.Salary)
                   .HasForeignKey<Salary>(s => s.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
