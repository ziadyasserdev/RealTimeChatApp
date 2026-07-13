using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Infrastructure.Constants
{
    public static class FileExtensions
    {
        public static readonly string[] ImageExtensions =
        [
            ".jpg",
        ".jpeg",
        ".png",
        ".webp"
        ];

        public static readonly string[] VideoExtensions =
        [
            ".mp4",
        ".mov",
        ".avi",
        ".mkv"
        ];
    }
}
