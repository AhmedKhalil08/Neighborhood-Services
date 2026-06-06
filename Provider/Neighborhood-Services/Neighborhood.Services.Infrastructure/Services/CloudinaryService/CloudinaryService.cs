using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using Neighborhood.Services.Application.Cloudinary;
namespace Neighborhood.Services.Infrastructure.Services.CloudinaryService
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly CloudinarySettings _settings;

        public CloudinaryService(
            IOptions<CloudinarySettings> options)
        {
            _settings = options.Value;
        }

        public CloudinarySignatureDto GenerateSignature()
        {
            var timestamp =
                DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var parameters =
                new SortedDictionary<string, object>
                {
                    { "timestamp", timestamp }
                };

            var account = new Account(
      _settings.CloudName,
      _settings.ApiKey,
      _settings.ApiSecret);

            var cloudinary = new Cloudinary(account);

            var signature = cloudinary.Api.SignParameters(parameters);

            return new CloudinarySignatureDto
            {
                Signature = signature,
                Timestamp = timestamp,
                ApiKey = _settings.ApiKey,
                CloudName = _settings.CloudName
            };
        }
    }
}
