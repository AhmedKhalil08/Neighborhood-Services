using Neighborhood.Services.Application.Shared;
using Neighborhood.Services.Domain.Reviews;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neighborhood.Services.Application.Reviews.Interfaces
{
    public interface IReviewRepository:IGenericRepository<Review,int>
    {
        // ── Queries ────────────────────────────────────────────────────────────
        Task<Review?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Review?> GetByBookingIdAsync(int bookingId, CancellationToken cancellationToken = default);
       
        Task<IReadOnlyList<Review>> GetByReviewerIdAsync(string reviewerId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Review>> GetByRevieweeIdAsync(string revieweeId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Review>> GetByStatusAsync(ReviewStatus status, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Review>> GetFlaggedAsync(CancellationToken cancellationToken = default);
        //Task<bool> ExistsByBookingIdAsync(int bookingId, CancellationToken cancellationToken = default);

      
        Task<bool> ExistsByDirectionAsync(int bookingId, ReviewType direction, CancellationToken cancellationToken = default);
        // ── Commands ───────────────────────────────────────────────────────────

    }
}
