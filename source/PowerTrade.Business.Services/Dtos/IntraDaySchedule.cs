namespace PowerTrade.Business.Services.Dtos
{
    public record IntraDaySchedule
    {
        public DateTime ScheduleTime { get; private set; }

        public IntraDaySchedule(DateTime scheduleTime)
        {
            this.ScheduleTime = scheduleTime;
        }
    }
}
