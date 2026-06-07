using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neighborhood.Services.Application.Cloudinary;

namespace Neighborhood.Services.API.Controllers.FilesCloudinary
{
    [ApiController]
    [Route("api/files")]
    public class FilesController : ControllerBase
    {
        private readonly ICloudinaryService _cloudinaryService;

        public FilesController(
            ICloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }

        [HttpPost("signature")]
        public IActionResult GetSignature()
        {
            var result =
                _cloudinaryService.GenerateSignature();

            return Ok(result);
        }
    }
}
