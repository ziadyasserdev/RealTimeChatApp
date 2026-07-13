using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Commons.Dtos
{
    public sealed class FileUploadResultDto
    {
        public string Url { get; init; } = string.Empty;

        public string PublicId { get; init; } = string.Empty;

        public long Bytes { get; init; }

        public string Format { get; init; } = string.Empty;

        public string ResourceType { get; init; } = string.Empty;
    }
}
