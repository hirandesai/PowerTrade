namespace PowerTrade.Services.Abstracts
{
    public interface IFileService
    {
        StreamWriter CreateFileStream(string filePath);
    }
}
