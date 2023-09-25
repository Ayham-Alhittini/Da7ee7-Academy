using Da7ee7_Academy.Entities;
using Da7ee7_Academy.Extensions;
using Da7ee7_Academy.Interfaces;

namespace Da7ee7_Academy.Data.Repository
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _env;
        public PhotoRepository(DataContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;

        }
        public bool CheckPhotoSended(IFormFile image)
        {
            return image != null && image.Length > 0 && image.ContentType.Contains("image");
        }

        public void DeletePhoto(AppFile file)
        {
            _context.Files.Remove(file);///Remove from database

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), file.Path, file.FileName);
            System.IO.File.Delete(filePath);//Remove from folder
        }

        public Task SaveChangesAsync()
        {
            _context.SaveChangesAsync();
            return Task.CompletedTask;
        }

        public AppFile SavePhoto(IFormFile photo, string path)
        {
            var file = new AppFile
            {
                ContentType = photo.ContentType,
                Path = path
            };

            var extension = photo.FileName.Split('.').LastOrDefault();

            file.FileName = file.Id + "." + extension;

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), file.Path, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                photo.CopyTo(stream);
            }

            _context.Files.Add(file);

            return file;
        }

        public async Task<AppFile> SavePhotoAsync(IFormFile photo, string path)
        {
            var file = new AppFile
            {
                ContentType = photo.ContentType,
                Path = path
            };

            var extension = photo.FileName.Split('.').LastOrDefault();

            file.FileName = file.Id + "." + extension;

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), file.Path, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }

            _context.Files.Add(file);

            return file;
        }
    }
}
