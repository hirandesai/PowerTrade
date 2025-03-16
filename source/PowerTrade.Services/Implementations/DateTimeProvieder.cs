using PowerTrade.Services.Abstracts;

namespace PowerTrade.Services.Implementations
{
    public class DateTimeProvieder : IDateTimeProvieder
    {
        public DateTime CurrentUtcTime => DateTime.UtcNow;

        public DateTime GetLocalTime(DateTime utcTime, string timeZone)
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
            return TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZoneInfo);
        }
    }
}
