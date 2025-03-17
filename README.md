# PowerTrade
## Design Decisions
There are two critical areas in the system:
1. Executing the schduler on time
2. Ensuring report is generated irrespective of what.

This kind of suggest that if a report generation takes long, it should not affect the scheduler. Thus these two component must be executed independently.
In the code, you will see two hosted service
* **SchedulerBackgroundService** - responsible for triggering the schedule
* **ReportBackgroundService** - responsible for generating the CSV file/report
These two component communicates via `IQueueService<T>` service which uses `Channel<T>` internally for produce consumer pattern. Choosing `Channel<T>` as primarily to keep the development simple and was not based on a design decision. Depending on what alternatives are available (SQS, Azure Service Bus, RabbitMQ etc), the choice will defer.... but there are certain criteria what will influence the selection:
    1. Should support asynchronous communication
    2. Should support built in retry mechanism
    3. Should support at-least one delivery
    4. Should support delayed delivery (not mandatory but good to have!)

### Database
One compoent which is not in this decision and we definately should have is database/persistant storage that stores schedule.
* SchedulerBackgroundService - should store the schedule in database & send a message for processing
* ReportBackgroundService - should read message from database & update status when processed or failed
* RetryService - Can periodically read the failed schedule from db and schedule them again. - This should be a fallback mechanism and not replacement of retry mechanism that's already part of ReportBackgroundService.

### Tests
Sample & most important unit & integration tests are added in code. Some others which should have been added are:
     1. Test for service configuartion especially the config initialization
     2. Unit tests for hosted services
     3. Unit tests for `IntraDayReportScheduleProcessor`
  
### Logging
Serilog was used given its popularity, support for recent updates and structured logging. The solution uses console & file logging which in most cases should be enough but depending on what monitoring tool is being used this may differ. Missing logging that can be implemented
* Performace Monitoring
* Distributed tracing

### Configs
* **SchedulerFrequencyInSec**: Schedure Frequency in seconds for better control (instead of minute). (alternative - CRON)
* **LocalTimeZoneId**: Timezone where application is running
* **ReportPath**: Path where file should be stored...  (IFileService currently implements FileService using local file system but can be replaced with S3FileService or BlobService)
* **CsvDelimiter**: Delimiter for CSV file


