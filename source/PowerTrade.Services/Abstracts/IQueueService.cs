namespace PowerTrade.Services.Abstracts
{
    public interface IQueueService<T>
    {
        Task AddAsync(T item);

        Task<T?> ReadAsync();
    }
}
