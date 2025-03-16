namespace PowerTrade.Business.Services.Dtos
{
    public record IntraDaySchedule
    {
        public DateTime ScheduleUtcTime { get; private set; }

        public IntraDaySchedule( DateTime scheduleUtcTime)
        {
            ScheduleUtcTime = scheduleUtcTime;
        }
    }
}
