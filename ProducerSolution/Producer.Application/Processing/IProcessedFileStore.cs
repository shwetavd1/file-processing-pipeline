namespace Producer.Application.Processing
{
    public interface IProcessedFileStore
    {
        bool IsProcessed(string fileName);
        void MarkAsProcessed(string fileName);
    }
}
