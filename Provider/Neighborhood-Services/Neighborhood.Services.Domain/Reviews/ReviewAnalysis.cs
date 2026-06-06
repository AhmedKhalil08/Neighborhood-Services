using Neighborhood.Services.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neighborhood.Services.Domain.Reviews
{
    public class ReviewAnalysis:BaseEntity<int>
    {
      
        public int ReviewId { get;set; }

        public ReviewSentiment Sentiment { get;  set; }

        public bool IsFlagged { get;  set; }

        public decimal QualityScore { get; set; }

        public DateTime CreatedAt { get; set; }


        // Navigation Property
        public Review Review { get; set; }
    }
}
