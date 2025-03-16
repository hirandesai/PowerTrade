namespace PowerTrade.Business.Services.Dtos
{
    public record IntraDaySchedule
    {
        public DateTime ScheduleLocalTime { get; private set; }
        public DateTime ScheduleUtcTime { get; private set; }

        public IntraDaySchedule(DateTime scheduleLocalTime, DateTime scheduleUtcTime)
        {
            ScheduleLocalTime = scheduleLocalTime;
            ScheduleUtcTime = scheduleUtcTime;
        }
    }
}
