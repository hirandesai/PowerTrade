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

        DateTime CurrentTime = new DateTime(2025, 03, 15, 10, 59, 00, DateTimeKind.Utc);

        [SetUp]
        public void Setup()
        {
            config = new IntraDayReportSchedulerConfig(1);
            queueService = new Mock<IQueueService<IntraDaySchedule>>();
            dateTimeProvieder = new Mock<IDateTimeProvieder>();
            dateTimeProvieder.Setup(q => q.CurrentTime).Returns(CurrentTime);

            intraDayReportScheduler = new IntraDayReportScheduler(config, queueService.Object, dateTimeProvieder.Object);
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
            cancellationTokenSource.CancelAfter(TimeSpan.FromMinutes(1));

            // Act
            await intraDayReportScheduler.Start(cancellationTokenSource.Token);

            // Assert
            queueService.Verify(q => q.AddAsync(It.Is<IntraDaySchedule>(schedule => AssertIntraDaySchedule(schedule, CurrentTime))), Times.AtLeastOnce());
        }

        private bool AssertIntraDaySchedule(IntraDaySchedule schedule, DateTime expectedScheduleTime)
        {
            Assert.That(schedule.ScheduleTime, Is.EqualTo(expectedScheduleTime));
            return true;
        }
    }
}