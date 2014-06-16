using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
        public static string TimeFormat = "HH:mm:ss.fff";

        public static string ToS3String(this DateTime datetime)
        {
            return string.Format("{0}T{1}Z", datetime.ToString(DateFormat), datetime.ToString(TimeFormat));
        }

        public static string ToISO8601(this DateTime datetime)
        {
            return string.Format("{0}T{1}Z", datetime.ToString("yyyyMMdd"), datetime.ToString("HHmmss"));
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

        public static string CreateCanonicalQueryString(string queryString)
        {
            return string.Join("&",
                queryString.Replace("?", "")
                    .Split('&')
                    .Select(p => p.Split(new[] { '=' }, 2))
                    .Where(p => p[0].IsNotEmpty())
                    .Select(p =>
                            string.Format("{0}={1}", UriEncode(p[0], true),
                                UriEncode(p.Length > 1 ? p[1] : string.Empty, true))));
        }

        public static string CreateCanonicalRequest(string httpMethod, string absolutePath, string queryString,
            Dictionary<string, string> headers, List<string> signedHeaders, string payload)
        {
            var headerString = string.Join("\n",
                headers.Keys
                    .Where(k => k != "Authorization")
                    .OrderBy(k => k)
                    .Select(h => string.Format("{0}:{1}", h.ToLowerInvariant(), headers[h].Trim())));
            var signedHeadersString = string.Join(";", signedHeaders.OrderBy(h => h).Select(s => s.ToLowerInvariant()));
            var payloadHash = SHA256.Create().HashString(payload);
            var canonicalQueryString = CreateCanonicalQueryString(queryString);
            return string.Join("\n", httpMethod, UriEncode(absolutePath, false), canonicalQueryString, headerString, string.Empty, signedHeadersString,
                payloadHash);
        }

        public static string CreateStringToSign(DateTime date, string region, string canonicalRequest)
        {
            return string.Format("AWS4-HMAC-SHA256\n{0}\n{1}\n{2}", 
                date.ToISO8601(),
                string.Format("{0}/{1}/s3/aws4_request", date.ToString("yyyyMMdd"), region),
                SHA256.Create().HashString(canonicalRequest));
        }

        public static string ComputeSignature(DateTime date, string secretAccessKey, string region, string stringToSign)
        {
            Func<string, byte[]> strToBytes = s => Encoding.UTF8.GetBytes(s);
            Func<byte[], byte[], byte[]> hash = (key, str) => new HMACSHA256(key).ComputeHash(str);
            Func<byte[], string> bytesToString = bts => string.Join(string.Empty, bts.Select(b => b.ToString("x2")));

            var dateKey = hash(strToBytes(string.Format("AWS4{0}", secretAccessKey)), strToBytes(date.ToString("yyyyMMdd")));
            var dateRegionKey = hash(dateKey, strToBytes(region));
            var dateRegionServiceKey = hash(dateRegionKey, strToBytes("s3"));
            var signingKey = hash(dateRegionServiceKey, strToBytes("aws4_request"));

            return bytesToString(hash(signingKey, strToBytes(stringToSign)));
        }

        public static string AssembleAuthorizationHeader(string accessKeyId, DateTime date, string region,
            List<string> signedHeaders, string signature)
        {
            signedHeaders.Sort();
            return string.Format("AWS4-HMAC-SHA256 Credential={0},SignedHeaders={1},Signature={2}",
                string.Format("{0}/{1}/{2}/s3/aws4_request", accessKeyId, date.ToString("yyyyMMdd"), region),
                string.Join(";", signedHeaders.Select(s => s.ToLowerInvariant())),
                signature);
        }
    }
}
