using PowerTrade.Services.Abstracts;

namespace PowerTrade.Services.Implementations
{
    public class FileService : IFileService
    {
        public StreamWriter CreateFileStream(string filePath)
        {
            return new StreamWriter(filePath, new FileStreamOptions()
            {
                Mode = FileMode.Create,
                Access = FileAccess.Write
            });
        }
    }
}
