namespace PowerTrade.Services.Abstracts
{
    public interface IDateTimeProvieder
    {
        DateTime CurrentUtcTime => DateTime.UtcNow;

        DateTime GetLocalTime(DateTime utcTime, string timeZone);
    }
}
