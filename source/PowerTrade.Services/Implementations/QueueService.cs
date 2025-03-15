using PowerTrade.Services.Abstracts;

namespace PowerTrade.Services.Implementations
{
    public class QueueService<T> : IQueueService<T>
    {
        public async Task AddAsync(T item)
        {
            await Task.CompletedTask;
        }

        public async Task<T> Read()
        {
            await Task.CompletedTask;
            return default(T);
        }
    }
}
