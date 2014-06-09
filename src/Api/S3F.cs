using System;

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
    }
}
