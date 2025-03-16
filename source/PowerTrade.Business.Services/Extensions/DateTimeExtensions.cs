namespace PowerTrade.Business.Services.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ToNextDayDate(this DateTime dateTime)
        {
            return dateTime.Date.AddDays(1).AddMinutes(-1).Date;
        }
    }
}
