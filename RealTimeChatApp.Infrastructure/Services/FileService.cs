//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Options;
//using RealTimeChatApp.Application.Commons.Results;
//using RealTimeChatApp.Application.Contracts.Services;
//using RealTimeChatApp.Application.Settings;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Hosting;
//namespace RealTimeChatApp.Infrastructure.Services
//{
//    public class FileService : IFileService
//    {
//        private readonly FileStorageSettings _settings;
//        private readonly IWebHostEnvironment webHostEnvironment;

//        public FileService(IOptions<FileStorageSettings> settings, IWebHostEnvironment webHostEnvironment)
//        {
//            _settings = settings.Value;
//            this.webHostEnvironment = webHostEnvironment;
//        }

//        public async Task<Result<byte[]>> DownloadFileAsync(string url)
//        {
//            var fullPath = Path.Combine(webHostEnvironment.WebRootPath, url);

//            if (!File.Exists(fullPath))
//                return Result<byte[]>.Failure(ResultStatus.Failure, "File not found");

//            var fileBytes = await File.ReadAllBytesAsync(fullPath);

//            return Result<byte[]>.Success(fileBytes);
//        }

//        public Result<string> Remove(string url)
//        {


//            var fullPath = Path.Combine(webHostEnvironment.WebRootPath, url);

//            if (!File.Exists(fullPath))
//                return Result<string>.Failure(ResultStatus.Failure, $"File not found at path: {fullPath}");

//            File.Delete(fullPath);
//            return Result<string>.Success("File removed successfully");
//        }

//        public Task<Result<string>> UploadImageAsync(IFormFile file)
//         => UploadAsync(file, new[] { ".jpg", ".png", ".jpeg" }, _settings.ImageSizeInMB, _settings.ImagesFolder);

//        public Task<Result<string>> UploadVideoAsync(IFormFile file)
//  => UploadAsync(file, _settings.VideoExtentionAllowed, _settings.VideoSizeInMB, _settings.VideosFolder);

//        private string EnsureFolder(string folderName)
//        {
//            string path = Path.Combine(webHostEnvironment.WebRootPath, _settings.UploadsFolder, folderName);
//            Directory.CreateDirectory(path);
//            return path;
//        }
//        private async Task<Result<string>> UploadAsync(
//    IFormFile file,
//    string[] extensionAllowed,
//    int maxSizeInMB,
//    string folderName)
//        {
//            var validationResult = ValidateFile(file, extensionAllowed, maxSizeInMB);

//            if (!validationResult.IsSuccess)
//                return Result<string>.Failure(
//                    validationResult.Status,
//                    validationResult.Error
//                );

//            var folderPath = EnsureFolder(folderName);

//            string extension = Path.GetExtension(file.FileName).ToLowerInvariant();
//            string newName = $"{Guid.NewGuid()}{extension}";
//            string fullPath = Path.Combine(folderPath, newName);

//            using (var fileStream = new FileStream(fullPath, FileMode.Create))
//            {
//                await file.CopyToAsync(fileStream);
//            }


//            var Url = $"{_settings.UploadsFolder}/{folderName}/{newName}";
//            return Result<string>.Success(Url);
//        }







//        private Result<object> ValidateFile(
// IFormFile file,
// string[] allowedExtensions,
// int maxSizeInMB)
//        {
//            if (file == null || file.Length == 0)
//                return Result<object>.Failure(
//                    ResultStatus.ValidationError,
//                    "File cannot be null or empty."
//                );

//            string extension = Path.GetExtension(file.FileName);
//            if (allowedExtensions == null || !allowedExtensions.Any())
//                return Result<object>.Failure(
//                    ResultStatus.Failure,
//                    "File extensions are not configured."
//                );
//            if (!allowedExtensions.Any(e =>
//                    e.Equals(extension, StringComparison.OrdinalIgnoreCase)))
//                return Result<object>.Failure(
//                    ResultStatus.ValidationError,
//                    $"Extension '{extension}' is not allowed."
//                );

//            if (file.Length > maxSizeInMB * 1024 * 1024)
//                return Result<object>.Failure(
//                    ResultStatus.ValidationError,
//                    $"File size exceeds {maxSizeInMB}MB."
//                );

//            return Result<object>.Success(null);
//        }





//    }
//}
