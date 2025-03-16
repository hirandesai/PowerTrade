namespace PowerTrade.Business.Services.Dtos
{
    public record IntraDaySchedule
    {
        public string ScheduleId => Guid.NewGuid().ToString();

        public DateTime ScheduleUtcTime { get; private set; }

        public int RetryCount { get; private set; }

        public IntraDaySchedule(DateTime scheduleUtcTime)
        {
            ScheduleUtcTime = scheduleUtcTime;
            RetryCount = 1;
        }

        public void IncrementRetry() => RetryCount++;
    }
}
