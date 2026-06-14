using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neighborhood.Services.Domain.Staffs;

namespace Neighborhood.Services.Infrastructure.Persistence.Staffs.Configurations
{
    public class StaffConfiguration : IEntityTypeConfiguration<Staff>
    {
        public void Configure(EntityTypeBuilder<Staff> builder)
        {
            builder.ToTable("Staffs");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .UseIdentityColumn();

           

            builder.Property(s => s.Role)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(s => s.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(s => s.CreatedByStaffId)
                .IsRequired(false);

            builder.Property(s => s.CreatedAt)
                .IsRequired();

            // Self-referencing FK: the staff member who created this staff
            //builder.HasOne<Staff>()
            //    .WithMany()
            //    .HasForeignKey(s => s.CreatedByStaffId)
            //    .OnDelete(DeleteBehavior.NoAction)
            //    .IsRequired(false);

            builder.HasMany(s => s.Permissions)
                .WithOne(p => p.Staff)
                .HasForeignKey(p => p.StaffId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(s => s.User)
             .WithOne(u => u.Staff)
             .HasForeignKey<Staff>(s => s.UserId)
             .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(s => s.CreatedByStaff)
            .WithMany(s => s.CreatedStaffs)
            .HasForeignKey(s => s.CreatedByStaffId)
            .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(s => s.ResolvedDisputes)
            .WithOne(d => d.ResolvedByStaff)
            .HasForeignKey(d => d.ResolvedByStaffId)
            .OnDelete(DeleteBehavior.NoAction);

            builder.HasQueryFilter(r => !r.IsDeleted);

            builder.HasIndex(s => s.Role)
                .HasDatabaseName("IX_Staffs_Role");

            builder.HasIndex(s => s.IsActive)
                .HasDatabaseName("IX_Staffs_IsActive");

            builder.HasIndex(s => s.UserId)
           .IsUnique();

        }
    }
}