using Microsoft.AspNetCore.Http;
using RealTimeChatApp.Application.Commons.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Contracts.Services
{
    public interface IFileService
    {
        Task<Result<string>> UploadImageAsync(IFormFile file);
        Task<Result<string>> UploadVideoAsync(IFormFile file);

        Result<string> Remove(string url);
        Task<Result<byte[]>> DownloadFileAsync(string url);
    }
}
