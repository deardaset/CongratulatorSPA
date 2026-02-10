namespace CongratulatorSPA.Server.Interfaces.Services
{
    public interface IStorageService
    {
        public Task<string> UploadPhotoAsync(IFormFile file);
        public Task DeletePhotoAsync(string file);
    }
}
