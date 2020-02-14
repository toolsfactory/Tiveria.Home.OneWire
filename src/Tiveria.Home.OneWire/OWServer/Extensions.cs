using System;
using System.Text;

namespace System
{
    public static class Extensions
    {
        public static string FromNullTerminatedBytes(this byte[] data)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            var result = new String(Encoding.ASCII.GetChars(data));
            return result.TrimEnd('\0');
        }

        public static byte[] ToNullTerminatedBytes(this string value)
        {
            return Encoding.ASCII.GetBytes(value + "\x00");
        }
    }

}
