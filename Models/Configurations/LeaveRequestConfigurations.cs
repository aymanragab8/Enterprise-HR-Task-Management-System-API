using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication2.Models.Entities;

namespace WebApplication2.Models.Configurations
{
    public class LeaveRequestConfigurations : IEntityTypeConfiguration<LeaveRequest>
    {
        public void Configure(EntityTypeBuilder<LeaveRequest> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.EmployeeId).IsRequired();
            builder.Property(e => e.FromDate).IsRequired();
            builder.Property(e => e.ToDate).IsRequired();
            builder.Property(e => e.Reason).IsRequired().HasMaxLength(500);
            builder.Property(e => e.Status).IsRequired();
            builder.Ignore(e => e.TotalDays);

            builder.HasOne(e => e.Employee)
                   .WithMany(e=>e.leaveRequests)
                   .HasForeignKey(e => e.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.ApprovedByEmployee)
                .WithMany(e => e.ApprovedleaveRequests)
                .HasForeignKey(e => e.ApprovedByEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
