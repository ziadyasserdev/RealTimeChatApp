using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using RealTimeChatApp.Application.Commons.Dtos;
using RealTimeChatApp.Application.Contracts.Storage;
using RealTimeChatApp.Application.Settings;
using RealTimeChatApp.Infrastructure.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Infrastructure.ExternalServices
{
    public sealed class CloudinaryStorageService : IFileStorageService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryStorageService(
            IOptions<CloudinarySettings> options)
        {
            var settings = options.Value;

            var account = new Account(
                settings.CloudName,
                settings.ApiKey,
                settings.ApiSecret);

            _cloudinary = new Cloudinary(account);

            _cloudinary.Api.Secure = true;
        }

      

        private static void ValidateFile(
    IFormFile file,
    string[] allowedExtensions,
    long maxSize)
        {
            if (file is null || file.Length == 0)
                throw new ArgumentException("File is required.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                throw new ArgumentException("Unsupported file format.");

            if (file.Length > maxSize)
                throw new ArgumentException("File size exceeded the allowed limit.");
        }
        private async Task<FileUploadResultDto> UploadAsync(
    IFormFile file,
    string folder,
    bool isVideo,
    CancellationToken cancellationToken)
        {
            await using var stream = file.OpenReadStream();

            if (!isVideo)
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = folder,
                    UseFilename = false,
                    UniqueFilename = true,
                    Overwrite = false
                };

                var result = await _cloudinary.UploadAsync(
                    uploadParams,
                    cancellationToken);

                if (result.Error is not null)
                    throw new Exception(result.Error.Message);

                return new FileUploadResultDto
                {
                    Url = result.SecureUrl.AbsoluteUri,
                    PublicId = result.PublicId,
                    Bytes = result.Bytes,
                    Format = result.Format,
                    ResourceType = "image"
                };
            }

            var videoParams = new VideoUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folder,
                UseFilename = false,
                UniqueFilename = true,
                Overwrite = false
            };

            var videoResult = await _cloudinary.UploadAsync(
                videoParams,
                cancellationToken);

            if (videoResult.Error is not null)
                throw new Exception(videoResult.Error.Message);

            return new FileUploadResultDto
            {
                Url = videoResult.SecureUrl.AbsoluteUri,
                PublicId = videoResult.PublicId,
                Bytes = videoResult.Bytes,
                Format = videoResult.Format,
                ResourceType = "video"
            };
        }
        public async Task DeleteImageAsync(
     string publicId,
     CancellationToken cancellationToken = default)
        {
            var result = await _cloudinary.DestroyAsync(
                new DeletionParams(publicId)
                {
                    ResourceType = ResourceType.Image
                });

            if (result.Error is not null)
                throw new Exception(result.Error.Message);
        }

        public async Task DeleteVideoAsync(
    string publicId,
    CancellationToken cancellationToken = default)
        {
            var result = await _cloudinary.DestroyAsync(
                new DeletionParams(publicId)
                {
                    ResourceType = ResourceType.Video
                });

            if (result.Error is not null)
                throw new Exception(result.Error.Message);
        }
        public async Task<FileUploadResultDto> UploadImageAsync(
     IFormFile file,
     string folder,
     CancellationToken cancellationToken = default)
        {
            ValidateFile(
                file,
                FileExtensions.ImageExtensions,
                FileLimits.MaxImageSize);

            return await UploadAsync(
                file,
                folder,
                false,
                cancellationToken);
        }

        public async Task<FileUploadResultDto> UploadVideoAsync(
      IFormFile file,
      string folder,
      CancellationToken cancellationToken = default)
        {
            ValidateFile(
                file,
                FileExtensions.VideoExtensions,
                FileLimits.MaxVideoSize);

            return await UploadAsync(
                file,
                folder,
                true,
                cancellationToken);
        }
    }
}
