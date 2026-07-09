using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Domain.Helpers
{
    public static class InviteCodeGenerator
    {
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static string Generate(int length = 8)
        {
            var result = new char[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = Chars[RandomNumberGenerator.GetInt32(Chars.Length)];
            }

            return new string(result);
        }
    }
}
