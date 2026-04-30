using Producer.Domain.Entities;

namespace Producer.Application.Messaging
{
    public interface IFileValidator
    {
        IEnumerable<IFileData> GetValidFiles(IEnumerable<IFileData> files);
    }
}
