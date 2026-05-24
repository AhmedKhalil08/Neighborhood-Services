using System;
using System.Collections.Generic;
using System.Text;

namespace Neighborhood.Services.Application.Reviews.Interfaces
{
    public interface IReviewAnalysisService
    {
        Task AnalyzeAsync(int reviewId, string comment);
    }
}
