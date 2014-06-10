using System;
using System.Linq;
using System.Text;

namespace Api
{
    /// <summary>
    /// S3 string data formats
    /// </summary>
    public static class S3F
    {
        public static string DateFormat = "yyyy-MM-dd";
        public static string TimeFormat = "hh:mm:ss.fff";

        public static string ToS3String(this DateTime datetime)
        {
            return string.Format("{0}T{1}Z", datetime.ToString(DateFormat), datetime.ToString(TimeFormat));
        }

        public static string UriEncode(string input, bool encodeSlash)
        {
            var result = new StringBuilder();
            for (var i = 0; i < input.Length; i++)
            {
                var ch = input[i];
                if ((ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z') || (ch >= '0' && ch <= '9') || ch == '_' ||
                    ch == '-' || ch == '~' || ch == '.')
                {
                    result.Append(ch);
                }
                else if (ch == '/')
                {
                    if (encodeSlash) result.Append("%2F");
                    else
                        result.Append(ch);
                }
                else
                {
                    result.Append(string.Format("%{0}", string.Join("", Encoding.UTF8.GetBytes(new []{ch}).Select(b => b.ToString("X2")))));
                }
            }
            return result.ToString();
        }
    }
}
