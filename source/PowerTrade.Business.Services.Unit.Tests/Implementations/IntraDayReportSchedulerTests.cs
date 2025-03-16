using Microsoft.Extensions.Logging;
using Moq;
using PowerTrade.Business.Services.Abstracts;
using PowerTrade.Business.Services.Dtos;
using PowerTrade.Business.Services.Implementations;
using PowerTrade.Services.Abstracts;

namespace PowerTrade.Business.Services.Unit.Tests.Implementations
{
    public class IntraDayReportSchedulerTests
    {
        private IntraDayReportSchedulerConfig config;
        private Mock<IQueueService<IntraDaySchedule>> queueService;
        private Mock<IDateTimeProvieder> dateTimeProvieder;

        private IIntraDayReportScheduler intraDayReportScheduler;

        DateTime CurrentTime = new DateTime(2025, 03, 15, 10, 59, 00, DateTimeKind.Unspecified);
        DateTime CurrentUtcTime = new DateTime(2025, 03, 15, 11, 59, 00, DateTimeKind.Utc);

        [SetUp]
        public void Setup()
        {
            var logger = new Mock<ILogger<IntraDayReportScheduler>>();
            queueService = new Mock<IQueueService<IntraDaySchedule>>();
            dateTimeProvieder = new Mock<IDateTimeProvieder>();
            dateTimeProvieder.Setup(q => q.CurrentTime).Returns(CurrentTime);
            dateTimeProvieder.Setup(q => q.CurrentUtcTime).Returns(CurrentUtcTime);
            config = new IntraDayReportSchedulerConfig(1);

            intraDayReportScheduler = new IntraDayReportScheduler(logger.Object, queueService.Object, dateTimeProvieder.Object, config);
        }

        [Test]
        public async Task ShouldNotQueueScheduleIfTokenIsAlreadyCancelled()
        {
            // Arrange
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // Act
            await intraDayReportScheduler.Start(cancellationTokenSource.Token);

            // Assert
            queueService.Verify(q => q.AddAsync(It.IsAny<IntraDaySchedule>()), Times.Never);
        }

        [Test]
        public async Task ShouldQueueScheduleIfTokenIsCancelledAfterSomeTime()
        {
            // Arrange
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(5));

            // Act
            await intraDayReportScheduler.Start(cancellationTokenSource.Token);

            // Assert
            queueService.Verify(q => q.AddAsync(It.Is<IntraDaySchedule>(schedule => AssertIntraDaySchedule(schedule, CurrentTime, CurrentUtcTime))), Times.AtLeastOnce());
        }

        private bool AssertIntraDaySchedule(IntraDaySchedule schedule, DateTime expectedScheduleLocalTime, DateTime expectedScheduleUtcTime)
        {
            Assert.That(schedule.ScheduleLocalTime, Is.EqualTo(expectedScheduleLocalTime));
            Assert.That(schedule.ScheduleUtcTime, Is.EqualTo(expectedScheduleUtcTime));
            return true;
        }
    }
}