using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Settings
{
    public class FileStorageSettings
    {
        public int ImageSizeInMB { get; set; }
        public int VideoSizeInMB { get; set; }
        public int FileSizeInMB { get; set; }
        public string UploadsFolder { get; set; } = null!;
        public string ImagesFolder { get; set; } = null!;
        public string VideosFolder { get; set; } = null!;
        public string FilesFolder { get; set; } = null!;
        public string[] ImageExtentionAllowed { get; set; } = null!;
        public string[] FileExtentionAllowed { get; set; } = null!;
        public string[] VideoExtentionAllowed { get; set; } = null!;
    }
}
