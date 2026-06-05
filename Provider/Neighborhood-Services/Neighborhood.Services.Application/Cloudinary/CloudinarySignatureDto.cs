using System;
using System.Collections.Generic;
using System.Text;

namespace Neighborhood.Services.Application.Cloudinary
{
    public class CloudinarySignatureDto
    {
        public string Signature { get; set; } = string.Empty;

        public long Timestamp { get; set; }

        public string ApiKey { get; set; } = string.Empty;

        public string CloudName { get; set; } = string.Empty;
    }
}
