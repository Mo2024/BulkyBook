using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Net;
using System.Net.Mail;

namespace BulkyBookWeb.Services
{
    public interface IUploadImageService
    {
        Task<ImageUploadResult> UploadImageToCloudinary(IFormFile image);

    }
    public class UploadImageService : IUploadImageService
    {
        private readonly Cloudinary _cloudinary;

        public UploadImageService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<ImageUploadResult> UploadImageToCloudinary(IFormFile image)
        {
            using var stream = image.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(image.FileName, stream),
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            return uploadResult;
        }
    }
}
