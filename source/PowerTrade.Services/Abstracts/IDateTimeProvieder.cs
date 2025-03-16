namespace PowerTrade.Services.Abstracts
{
    public interface IDateTimeProvieder
    {
        DateTime CurrentTime { get; }
        public DateTime CurrentUtcTime => DateTime.UtcNow;
    }
}
