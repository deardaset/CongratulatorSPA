using Amazon.S3;
using Amazon.S3.Model;
using CongratulatorSPA.Server.Exceptions;
using CongratulatorSPA.Server.Interfaces.Services;

namespace CongratulatorSPA.Server.Services
{
    public class StorageService : IStorageService
    {
        private readonly IAmazonS3 _s3;
        private readonly string _bucket;

        public StorageService(IConfiguration config)
        {
            var accessKey = config["YandexStorage:AccessKey"];
            var secretKey = config["YandexStorage:SecretKey"];
            _bucket = config["YandexStorage:Bucket"];

            var s3Config = new AmazonS3Config
            {
                ServiceURL = "https://storage.yandexcloud.net",
                ForcePathStyle = true
            };

            _s3 = new AmazonS3Client(accessKey, secretKey, s3Config);
        }
        
        public async Task<string> UploadPhotoAsync(IFormFile file)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            using var stream = file.OpenReadStream();

            var request = new PutObjectRequest
            {
                BucketName = _bucket,
                Key = fileName,
                InputStream = stream,
                ContentType = file.ContentType,
                CannedACL = S3CannedACL.PublicRead
            };

            await _s3.PutObjectAsync(request);

            return $"https://storage.yandexcloud.net/{_bucket}/{fileName}";
        }
        public async Task DeletePhotoAsync(string file)
        {
            if (string.IsNullOrEmpty(file))
                throw new BadRequestException("File name cannot be null or empty");

            var fileName = GetKeyFromUrl(file);

            var request = new DeleteObjectRequest
            {
                BucketName = _bucket,
                Key = fileName
            };

            await _s3.DeleteObjectAsync(request);
        }

        //Additional
        private string GetKeyFromUrl(string url)
        {
            var uri = new Uri(url);
            var segments = uri.AbsolutePath.Split('/');
            return segments.Length >= 2 ? string.Join('/', segments.Skip(2)) : null;
        }

    }
}
