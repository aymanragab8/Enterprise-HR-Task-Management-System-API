using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication2.Models.Entities;

namespace WebApplication2.Models.Configurations
{
    public class DepartmentConfigurations : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(d => d.Id);
            builder.HasQueryFilter(d => !d.IsDeleted);
            builder.HasIndex(d=>d.Code).IsUnique();
            builder.Property(d => d.Code).IsRequired().HasMaxLength(10);
            builder.Property(d => d.Name).IsRequired().HasMaxLength(100);
            builder.Property(d => d.Description).HasMaxLength(200);
            builder.Property(d => d.IsDeleted).IsRequired();
            builder.Property(d => d.CreatedAt).IsRequired();
            builder.Property(d => d.UpdatedAt).IsRequired(false);

            builder.HasOne(d => d.Manager)
                   .WithMany()
                   .HasForeignKey(d => d.ManagerId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
