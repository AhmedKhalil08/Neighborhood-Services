using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neighborhood.Services.Domain.Reviews;

namespace Neighborhood.Services.Infrastructure.Persistence.Reviews.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Reviews");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .UseIdentityColumn();

            builder.Property(r => r.BookingId)
                .IsRequired();

            builder.Property(r => r.ReviewerId)
                 .IsRequired()
                 .HasMaxLength(450);

            builder.Property(r => r.RevieweeId)
                .IsRequired()
                .HasMaxLength(450);

          

            builder.Property(r => r.Comment)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(r => r.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(r => r.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(r => r.CreatedAt)
                .IsRequired();

            // One Review → One ReviewAnalysis
            builder.HasOne(r => r.Analysis)
                .WithOne(a => a.Review)
                .HasForeignKey<ReviewAnalysis>(a => a.ReviewId)
                .OnDelete(DeleteBehavior.NoAction);

            //        builder.HasIndex(r => new { r.BookingId, r.ReviewerId })
            //.IsUnique();
            builder.HasIndex(r => new { r.BookingId, r.ReviewerId, r.RevieweeId })
            .IsUnique();
            builder.HasOne(r => r.Reviewer)
                .WithMany()
                .HasForeignKey(r => r.ReviewerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Reviewee)
                .WithMany()
                .HasForeignKey(r => r.RevieweeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(r => !r.IsDeleted);

          
          

            builder.HasIndex(r => r.ReviewerId)
                .HasDatabaseName("IX_Reviews_ReviewerId");

            builder.HasIndex(r => r.RevieweeId)
                .HasDatabaseName("IX_Reviews_RevieweeId");

            builder.HasIndex(r => r.Status)
                .HasDatabaseName("IX_Reviews_Status");
        }
    }
}