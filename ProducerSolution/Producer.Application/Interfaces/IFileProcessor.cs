using Producer.Domain;

namespace Producer.Application.Interfaces
{
    public interface IFileProcessor<TInput, TOutput>
    {
        Task<TOutput> ProcessAsync(TInput input);
    }
}
