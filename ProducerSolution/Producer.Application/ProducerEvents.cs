// notify when a file is processed
namespace Producer.Application
{
    public class ProducerEvents
    {
        //Action - built in delegate 
        public event Action<string>? OnFileProcessed;
        // does not return a value after processing the files just notifies
        public void RaiseFileProcessed(string fileName)
        {
            OnFileProcessed.Invoke(fileName);
        }
    }
}
