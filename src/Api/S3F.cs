using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using LeviySoft.Extensions;

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

        public static string ToISO8601(this DateTime datetime)
        {
            return string.Format("{0}T{1}Z", datetime.ToString("yyyyMMdd"), datetime.ToString("hhmmss"));
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

        public static string CreateCanonicalRequest(string httpMethod, string absolutePath, string queryString,
            SortedDictionary<string, string> headers, List<string> signedHeaders, string payload)
        {
            var headerString = string.Join("\n",
                headers.Select(h => string.Format("{0}:{1}", h.Key.ToLowerInvariant(), h.Value.Trim())));
            signedHeaders.Sort();
            var signedHeadersString = string.Join(";", signedHeaders);
            var payloadHash = SHA256.Create().HashString(payload);
            return string.Join("\n", httpMethod, absolutePath, queryString, headerString, signedHeadersString,
                payloadHash);
        }

        public static string CreateStringToSign(DateTime date, string region, string canonicalRequest)
        {
            return string.Format("AWS4-HMAC-SHA256\n{0}\n{1}\n{2}", 
                DateTime.Now.ToISO8601(),
                string.Format("{0}/{1}/s3/aws4_request", date.ToString("yyyyMMdd"), region),
                SHA256.Create().HashString(canonicalRequest));
        }

        public static string ComputeSignature(DateTime date, string secretAccessKey, string region, string stringToSign)
        {
            Func<string, HMACSHA256> makeHasher = salt => new HMACSHA256(Encoding.ASCII.GetBytes(salt));
            Func<string, string, string> hash = (key, str) => makeHasher(key).HashString(str);

            var dateKey = hash(string.Format("AWS4{0}", secretAccessKey), date.ToString("yyyyMMdd"));
            var dateRegionKey = hash(dateKey, region);
            var dateRegionServiceKey = hash(dateRegionKey, "s3");
            var signingKey = hash(dateRegionServiceKey, "aws4_request");

            return hash(signingKey, stringToSign);
        }
    }
}
