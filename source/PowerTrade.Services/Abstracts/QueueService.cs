using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTrade.Services.Abstracts
{
    public interface IQueueService<T>
    {
        Task AddAsync(T item);

        Task<T> Read();
    }
}
