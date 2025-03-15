using PowerTrade.Services.Abstracts;
using System.Threading.Channels;

namespace PowerTrade.Services.Implementations
{
    public class QueueService<T> : IQueueService<T>
    {
        private const int ReadTimeOutSec = 3;
        private readonly Channel<T> queue;

        public QueueService()
        {
            queue = Channel.CreateUnbounded<T>(new UnboundedChannelOptions()
            {
                SingleWriter = true,
                SingleReader = true
            });
        }

        public async Task AddAsync(T item)
        {
            await queue.Writer.WriteAsync(item);
        }

        public async Task<T?> ReadAsync()
        {
            try
            {
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(ReadTimeOutSec));

                return await queue.Reader.ReadAsync(cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                return default(T);
            }
        }
    }
}
