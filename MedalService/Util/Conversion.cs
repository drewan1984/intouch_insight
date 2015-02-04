using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedalService.Util
{
    public static class Conversion
    {
        public static String AsciiConverter(string utf8String)
        {
            System.Text.Encoding utf8 = System.Text.Encoding.UTF8;
            Byte[] encodedBytes = utf8.GetBytes(utf8String);
            Byte[] convertedBytes =
                    Encoding.Convert(Encoding.UTF8, Encoding.ASCII, encodedBytes);
            System.Text.Encoding ascii = System.Text.Encoding.ASCII;

            var result = ascii.GetString(convertedBytes);
            return result;
        }
    }
}
