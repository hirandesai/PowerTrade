using PowerTrade.Services.Abstracts;
using PowerTrade.Services.Implementations;

namespace PowerTrade.Services.Integration.Tests
{
    [TestFixture]
    public class QueueServiceTests
    {
        IQueueService<int> queue;

        [SetUp]
        public void Setup()
        {
            queue = new QueueService<int>();
        }

        [TestCase]
        public async Task CanReadAndWriteToQueue()
        {
            // Arrange
            int itemToAdd = 100;
            int expectedItem = 100;

            // Act
            await queue.AddAsync(itemToAdd);
            var actualItem = await queue.ReadAsync();

            // Assert
            Assert.That(actualItem, Is.EqualTo(expectedItem));
        }

        [TestCase]
        public async Task ReturnsDefaultWhenNoItemExists()
        {
            // Act
            var actualItem = await queue.ReadAsync();

            // Assert
            Assert.That(actualItem, Is.EqualTo(0));
        }

        [TestCase]
        public async Task CanReadAndWriteToComplexType()
        {
            // Arrange
            IQueueService<ComplexRecord> complexQueue = new QueueService<ComplexRecord>();

            DateTime time = new DateTime(2025, 03, 15, 10, 32, 00);
            ComplexRecord itemToAdd = new ComplexRecord(100, time);
            ComplexRecord expectedItem = new ComplexRecord(100, time);

            // Act
            await complexQueue.AddAsync(itemToAdd);
            var actualItem = await complexQueue.ReadAsync();

            // Assert
            Assert.That(actualItem, Is.EqualTo(expectedItem));


        }

        private record ComplexRecord
        {
            public int Id { get; private set; }
            public DateTime Time { get; private set; }

            public ComplexRecord(int id, DateTime time)
            {
                Id = id;
                Time = time;
            }
        }
    }
}