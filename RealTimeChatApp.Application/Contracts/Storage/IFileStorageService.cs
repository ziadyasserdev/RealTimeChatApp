using Microsoft.AspNetCore.Http;
using RealTimeChatApp.Application.Commons.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Contracts.Storage
{
    public interface IFileStorageService
    {
        Task<FileUploadResultDto> UploadImageAsync(
            IFormFile file,
            string folder,
            CancellationToken cancellationToken = default);

        Task<FileUploadResultDto> UploadVideoAsync(
            IFormFile file,
            string folder,
            CancellationToken cancellationToken = default);

        Task DeleteImageAsync(
            string publicId,
            CancellationToken cancellationToken = default);

        Task DeleteVideoAsync(
            string publicId,
            CancellationToken cancellationToken = default);
    }
}
