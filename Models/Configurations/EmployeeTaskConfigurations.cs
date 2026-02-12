using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication2.Models.Entities;

namespace WebApplication2.Models.Configurations
{
    public class EmployeeTaskConfigurations : IEntityTypeConfiguration<EmployeeTask>
    {
        public void Configure(EntityTypeBuilder<EmployeeTask> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(t => t.Title).IsRequired().HasMaxLength(150);
            builder.Property(t => t.Description).HasMaxLength(500);
            builder.Property(t => t.Status).IsRequired();
            builder.Property(t => t.Priority).IsRequired();
            builder.Property(t => t.DeadLine).IsRequired();

            builder.HasOne(et=>et.CreatedByEmployee)
                .WithMany(e=>e.CreatedTasks)
                .HasForeignKey(et=>et.CreatedByEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(et=>et.AssignedToEmployee)
                .WithMany(e=>e.AssignedTasks)
                .HasForeignKey(et=>et.AssignedToEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
