using Da7ee7_Academy.Entities;

namespace Da7ee7_Academy.Interfaces
{
    public interface IPhotoRepository
    {
        bool CheckPhotoSended(IFormFile image);
        Task<AppFile> SavePhotoAsync(IFormFile photo, string path);
        AppFile SavePhoto(IFormFile photo, string path);
        void DeletePhoto(AppFile file);
        Task SaveChangesAsync();
    }
}
