using PowerTrade.Services.Abstracts;

namespace PowerTrade.Services.Implementations
{
        public class DateTimeProvieder : IDateTimeProvieder
        {
            public DateTime CurrentTime => DateTime.UtcNow;
        }
}
