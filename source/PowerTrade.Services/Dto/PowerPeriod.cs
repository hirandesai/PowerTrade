namespace PowerTrade.Services.Dto
{
    public record PowerPeriod
    {
        public int Period { get; private set; }

        public double Volume { get; private set; }

        public PowerPeriod(int period, double volume)
        {
            Period = period;
            Volume = volume;
        }

        public static explicit operator PowerPeriod(Axpo.PowerPeriod powerPeriod) => new PowerPeriod(powerPeriod.Period, powerPeriod.Volume);

    }
}
