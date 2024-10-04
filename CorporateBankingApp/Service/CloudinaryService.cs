using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace CorporateBankingApp.Service
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration configuration)
        {
            // Fetch the Cloudinary credentials from appsettings.json
            var cloudinarySettings = configuration.GetSection("CloudinarySettings");
            Account account = new Account(
                cloudinarySettings["CloudName"],    // Access CloudName
                cloudinarySettings["ApiKey"],       // Access ApiKey
                cloudinarySettings["ApiSecret"]     // Access ApiSecret
            );
            _cloudinary = new Cloudinary(account);
        }

        // Method to upload image
        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.FileName, stream)
                    };

                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                    return uploadResult.SecureUrl.ToString();  // Return the uploaded image URL
                }
            }
            return null;
        }
    }

}

