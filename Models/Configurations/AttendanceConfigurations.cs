using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication2.Models.Entities;

namespace WebApplication2.Models.Configurations
{
    public class AttendanceConfigurations : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Date).HasColumnType("date"); 
            builder.Property(a => a.CheckIn).IsRequired();
            builder.Property(a => a.CheckOut).IsRequired(false);
            builder.HasIndex(a => new { a.EmployeeId, a.Date })
                   .IsUnique();

            builder.HasOne(a => a.Employee)
                   .WithMany()
                   .HasForeignKey(a => a.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
