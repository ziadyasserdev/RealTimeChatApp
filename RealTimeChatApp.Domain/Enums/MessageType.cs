using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Domain.Enums
{
    public enum MessageType
    {
        Text = 1,
        Image,
        File,
        Voice,
        Video
    }
}
