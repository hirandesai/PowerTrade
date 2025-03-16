
using PowerTrade.Business.Services.Dtos;
using PowerTrade.Business.Services.Extensions;
using PowerTrade.Services.Dto;

namespace PowerTrade.Business.Services.Unit.Tests.Implementations
{
    [TestFixture]
    public class PowerTradeRecordExtensionsTests
    {
        [Test]
        public void ShouldAggregateTrades()
        {
            // Arrange
            var requestedDate = new DateTime(2025, 03, 15, 23, 55, 15);
            var localTimezone = "Romance Standard Time";

            var date = new DateTime(2025, 03, 16, 00, 00, 00);
            var powerTraderRecord1 = new PowerTradeRecord(Guid.NewGuid().ToString(),
                                                            date,
                                                            new PowerPeriod[]
                                                            {
                                                                new PowerPeriod(1,751.9685187043873),
                                                                new PowerPeriod(2,502.53077327530394),
                                                                new PowerPeriod(3,996.774680459041),
                                                                new PowerPeriod(4,418.78729519441504),
                                                                new PowerPeriod(5,3.1945635089841717),
                                                                new PowerPeriod(6,811.6795090784999),
                                                            });
            var powerTraderRecord2 = new PowerTradeRecord(Guid.NewGuid().ToString(),
                                                            date,
                                                            new PowerPeriod[]
                                                            {
                                                                new PowerPeriod(1,755.9685187043873),
                                                                new PowerPeriod(2,503.53077327530394),
                                                                new PowerPeriod(3,99.774680459041),
                                                                new PowerPeriod(4,41.78729519441504),
                                                                new PowerPeriod(5,30.1945635089841717),
                                                                new PowerPeriod(6,81.6795090784999),
                                                                new PowerPeriod(7,150.6795090784999),
                                                            });
            var expectedDate = new DateTime(2025, 03, 16, 01, 00, 00, DateTimeKind.Utc);
            var expectedData = new AggregatedPowerTradeRecord[]
            {
                new AggregatedPowerTradeRecord(expectedDate.AddHours(0),1507.9370374087746),
                new AggregatedPowerTradeRecord(expectedDate.AddHours(1),1006.0615465506079),
                new AggregatedPowerTradeRecord(expectedDate.AddHours(2),1096.549360918082),
                new AggregatedPowerTradeRecord(expectedDate.AddHours(3),460.5745903888301),
                new AggregatedPowerTradeRecord(expectedDate.AddHours(4),33.38912701796834),
                new AggregatedPowerTradeRecord(expectedDate.AddHours(5),893.3590181569998),
                new AggregatedPowerTradeRecord(expectedDate.AddHours(6),150.6795090784999),
            };


            var powerTraders = new PowerTradeRecord[]
            {
                powerTraderRecord1,
                powerTraderRecord2
            };

            // Act
            var aggregatedData = powerTraders.ToAggregated(requestedDate, localTimezone);
            var x = System.Text.Json.JsonSerializer.Serialize(aggregatedData);
            // Assert
            Assert.That(aggregatedData, Is.EqualTo(expectedData));
        }

        [Test]
        public void ShouldAggregateTradesConsideringTimezone()
        {
            // Arrange
            var requestedDate = new DateTime(2025, 03, 16, 19, 25, 16);
            var localTimezone = "US Eastern Standard Time";

            var date = new DateTime(2025, 03, 17, 00, 00, 00);
            var powerTraderRecord1 = new PowerTradeRecord(Guid.NewGuid().ToString(),
                                                            date,
                                                            new PowerPeriod[]
                                                            {
                                                                new PowerPeriod(1,751.9685187043873),
                                                                new PowerPeriod(2,502.53077327530394),
                                                                new PowerPeriod(3,996.774680459041),
                                                                new PowerPeriod(4,418.78729519441504),
                                                                new PowerPeriod(5,3.1945635089841717),
                                                                new PowerPeriod(6,811.6795090784999),
                                                            });
            var powerTraderRecord2 = new PowerTradeRecord(Guid.NewGuid().ToString(),
                                                            date,
                                                            new PowerPeriod[]
                                                            {
                                                                new PowerPeriod(1,755.9685187043873),
                                                                new PowerPeriod(2,503.53077327530394),
                                                                new PowerPeriod(3,99.774680459041),
                                                                new PowerPeriod(4,41.78729519441504),
                                                                new PowerPeriod(5,30.1945635089841717),
                                                                new PowerPeriod(6,81.6795090784999),
                                                                new PowerPeriod(7,150.6795090784999),
                                                            });
            var expectedDate = new DateTime(2025, 03, 16, 16, 00, 00, DateTimeKind.Utc);
            var expectedData = new AggregatedPowerTradeRecord[]
            {
                new AggregatedPowerTradeRecord(expectedDate.AddHours(0),1507.9370374087746),
                new AggregatedPowerTradeRecord(expectedDate.AddHours(1),1006.0615465506079),
                new AggregatedPowerTradeRecord(expectedDate.AddHours(2),1096.549360918082),
                new AggregatedPowerTradeRecord(expectedDate.AddHours(3),460.5745903888301),
                new AggregatedPowerTradeRecord(expectedDate.AddHours(4),33.38912701796834),
                new AggregatedPowerTradeRecord(expectedDate.AddHours(5),893.3590181569998),
                new AggregatedPowerTradeRecord(expectedDate.AddHours(6),150.6795090784999),
            };


            var powerTraders = new PowerTradeRecord[]
            {
                powerTraderRecord1,
                powerTraderRecord2
            };

            // Act
            var aggregatedData = powerTraders.ToAggregated(requestedDate, localTimezone);

            // Assert
            Assert.That(aggregatedData, Is.EqualTo(expectedData));
        }

        [Test]
        public void ShouldAggregateTradesConsideringDaylightSavingTime()
        {
            // Arrange
            var requestedDate = new DateTime(2025, 03, 29, 23, 55, 15);
            var localTimezone = "Romance Standard Time";

            var date = new DateTime(2025, 03, 30, 00, 00, 00);
            var powerTraderRecord1 = new PowerTradeRecord(Guid.NewGuid().ToString(),
                                                            date,
                                                            new PowerPeriod[]
                                                            {
                                                                new PowerPeriod(1,751.9685187043873),
                                                                new PowerPeriod(2,502.53077327530394),
                                                                new PowerPeriod(3,996.774680459041),
                                                                new PowerPeriod(4,418.78729519441504),
                                                                new PowerPeriod(5,3.1945635089841717),
                                                                new PowerPeriod(6,811.6795090784999),
                                                            });
            var powerTraderRecord2 = new PowerTradeRecord(Guid.NewGuid().ToString(),
                                                            date,
                                                            new PowerPeriod[]
                                                            {
                                                                new PowerPeriod(1,755.9685187043873),
                                                                new PowerPeriod(2,503.53077327530394),
                                                                new PowerPeriod(3,99.774680459041),
                                                                new PowerPeriod(4,41.78729519441504),
                                                                new PowerPeriod(5,30.1945635089841717),
                                                                new PowerPeriod(6,81.6795090784999),
                                                                new PowerPeriod(7,150.6795090784999),
                                                            });
            var expectedDate = new DateTime(2025, 03, 30, 01, 00, 00, DateTimeKind.Utc);
            var expectedData = new AggregatedPowerTradeRecord[]
            {
                new AggregatedPowerTradeRecord(expectedDate.AddHours(0),1507.9370374087746),
                new AggregatedPowerTradeRecord(expectedDate.AddHours(2),1006.0615465506079),
                new AggregatedPowerTradeRecord(expectedDate.AddHours(3),1096.549360918082),
                new AggregatedPowerTradeRecord(expectedDate.AddHours(4),460.5745903888301),
                new AggregatedPowerTradeRecord(expectedDate.AddHours(5),33.38912701796834),
                new AggregatedPowerTradeRecord(expectedDate.AddHours(6),893.3590181569998),
                new AggregatedPowerTradeRecord(expectedDate.AddHours(7),150.6795090784999),
            };


            var powerTraders = new PowerTradeRecord[]
            {
                powerTraderRecord1,
                powerTraderRecord2
            };

            // Act
            var aggregatedData = powerTraders.ToAggregated(requestedDate, localTimezone);
            var x = System.Text.Json.JsonSerializer.Serialize(aggregatedData);
            // Assert
            Assert.That(aggregatedData, Is.EqualTo(expectedData));
        }
    }
}
