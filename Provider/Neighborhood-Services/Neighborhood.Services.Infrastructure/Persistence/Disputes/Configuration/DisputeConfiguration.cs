using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neighborhood.Services.Domain.Disputes;

namespace Neighborhood.Services.Infrastructure.Persistence.Disputes.Configuration
{
    public class DisputeConfiguration : IEntityTypeConfiguration<Dispute>
    {
        public void Configure(EntityTypeBuilder<Dispute> builder)
        {
            builder.ToTable("Disputes");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Id)
                .UseIdentityColumn();

            builder.Property(d => d.BookingId)
                .IsRequired();

            builder.Property(d => d.RaisedByUserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(d => d.ResolvedByStaffId)
                .IsRequired(false);

            builder.Property(d => d.DisputeType)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(d => d.Reason)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(d => d.Resolution)
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.Property(d => d.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(d => d.CreatedAt)
                .IsRequired();

            builder.Property(d => d.ResolvedAt)
                .IsRequired(false);

            builder.HasQueryFilter(d => !d.IsDeleted);
            // Booking <-> Dispute (1:1)
            builder.HasOne(d => d.Booking)
                .WithOne(b => b.Dispute)
                .HasForeignKey<Dispute>(d => d.BookingId)
                .OnDelete(DeleteBehavior.NoAction);

            // User who raised dispute
            builder.HasOne(d => d.RaisedByUser)
                .WithMany()
                .HasForeignKey(d => d.RaisedByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Staff who resolved dispute
            builder.HasOne(d => d.ResolvedByStaff)
                .WithMany(s => s.ResolvedDisputes)
                .HasForeignKey(d => d.ResolvedByStaffId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasQueryFilter(r => !r.IsDeleted);

            builder.HasIndex(d => d.BookingId)
                .IsUnique()
                .HasDatabaseName("IX_Disputes_BookingId");

            builder.HasIndex(d => d.RaisedByUserId)
                .HasDatabaseName("IX_Disputes_RaisedByUserId");

            builder.HasIndex(d => d.Status)
                .HasDatabaseName("IX_Disputes_Status");

            builder.HasIndex(d => d.ResolvedByStaffId)
                .HasDatabaseName("IX_Disputes_ResolvedByStaffId");
        }
    }
}