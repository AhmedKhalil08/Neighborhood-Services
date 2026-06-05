using System;
using System.Collections.Generic;
using System.Text;

namespace Neighborhood.Services.Application.Cloudinary
{
    public interface ICloudinaryService
    {
        CloudinarySignatureDto GenerateSignature();
    }
}
