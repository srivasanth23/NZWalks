using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class LocalImageRepo : IImageRepo
    {

        private readonly IWebHostEnvironment _host;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly NZWalksDbContext _dbContext;

        public LocalImageRepo(IWebHostEnvironment host, 
            IHttpContextAccessor contextAccessor,
            NZWalksDbContext dbContext
            )
        {
            _host = host;
            _contextAccessor = contextAccessor;
            _dbContext = dbContext;
        }




        public async Task<Image> Upload(Image image)
        {
            var localFilePath = Path.Combine(_host.ContentRootPath, "Images",
                $"{image.FileName}{image.FileExtension}");

            // Upload Image to Local Path
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            // https:localhost:7080/images/image.jpeg
            var urlFilePath = $"{_contextAccessor.HttpContext.Request.Scheme}://{_contextAccessor.HttpContext.Request.Host}{_contextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";

            image.FilePath = urlFilePath;

            // Add Image to the Images table
            await _dbContext.Images.AddAsync(image);
            await _dbContext.SaveChangesAsync();

            return image;
        }
    }
}
