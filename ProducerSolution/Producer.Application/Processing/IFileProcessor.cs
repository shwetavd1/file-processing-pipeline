namespace Producer.Application.Messaging
{
    public interface IFileProcessor<TInput, TOutput>
    {
        Task<TOutput> ProcessAsync(TInput input);
    }
}
