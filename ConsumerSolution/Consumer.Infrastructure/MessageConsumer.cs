using Consumer.Application;
using Consumer.Domain;

/*
 This class gives us a message data(CSV) like it came from queue
Need:
In real system. Data comes from queue(RabbitMQ) and consumer reads message but right now RabbitMQ is not connected so we simulate it
so this class pretend-> i received message from queue
 */
namespace Consumer.Infrastructure //Infrasturucture layer contains actual implementation
{
    public class MessageConsumer : IMessageConsumer
    {
        //public delegate void MessageReceivedHandler(MessageData message); //this is delegate. It defines a method signature. It means any menthod matching this structure can be used
        //public event MessageReceivedHandler? OnMessageReceived; // this is event. This is a trigger that notifies other. This event is fire when message is received
        public event Action<MessageData>? OnMessageReceived;// Action<T> is built in delegate we dont need custom delegate.
        public async Task<MessageData> ConsumeAsync() //returns one message
        {
            //simulating message from queue
            var message1 = new MessageData// creating dummy CSV message
            {
                Id = 1,
                CsvContent = "Id,Name,Age\n 1, Prajakta, 25\n 2, Shweta",
                ReceivedTime = DateTime.UtcNow
            };
            Console.WriteLine("message received from queue");
            OnMessageReceived?.Invoke(message1); // It triggers event. If someone is listening, notifys them with message
            return await Task.FromResult(message1); // returns message in async format
        }
    }
}
//here i am mocking message consumption later i will integrate with RabitMQ.
// adding event because, instead of directly calling worker->converter->tracker we now say, message received-> notify system
//befor adding event, worker controls everything but now events control flow