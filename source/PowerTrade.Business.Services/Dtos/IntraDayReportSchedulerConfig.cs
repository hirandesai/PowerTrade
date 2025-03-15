using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTrade.Business.Services.Dtos
{
    public record IntraDayReportSchedulerConfig
    {
        public int FrequencyInSec { get; private set; }
        
        public IntraDayReportSchedulerConfig(int frequencyMin)
        {
            FrequencyInSec = frequencyMin;
        }
    }
}
