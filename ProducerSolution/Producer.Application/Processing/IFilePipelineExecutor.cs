using Producer.Domain.Entities;

namespace Producer.Application.Messaging
{
    public interface IFilePipelineExecutor
    {
        Task ExecuteAsync (IEnumerable<IFileData> files, CancellationToken cancellationToken);
    }
}
