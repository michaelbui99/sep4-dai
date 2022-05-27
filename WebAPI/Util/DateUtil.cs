namespace WebAPI.Util
{
    public class DateUtil
    {
        public static DateTime GetDateTimeFromUnixTimeSeconds(long unixTimeSeconds)
        {
            var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTimeSeconds);
            return dateTimeOffset.DateTime;
        }
    }
}