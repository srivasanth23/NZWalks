using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IImageRepo
    {
        Task<Image> Upload(Image image);
    }
}
