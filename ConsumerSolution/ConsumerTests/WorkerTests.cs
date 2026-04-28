using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Consumer.Application;
using Consumer.Infrastructure;
using ConsumerService;
using Consumer.Domain;

namespace ConsumerTests
{
    public class WorkerTests
    {
        [Fact]
        public async Task HandleMessage_Should_Process_Message_Successfully()
        {
            //Arrange
            var consumerMock = new Mock<IMessageConsumer<string>>();
            var converterMock = new Mock<ICsvToJsonConverter<Dictionary<string,object>>>();
            var statusMock = new Mock<IStatusTracker>();

            converterMock.Setup(c => c.ConvertAsync(It.IsAny<string>())).ReturnsAsync(new List<Dictionary<string, object>>
            {
                new Dictionary<string,object>
                {
                    {"Id", 1 },
                    {"Name", "Test" },
                    {"Age",25 }
                }
            });

            var worker = new Worker(consumerMock.Object, converterMock.Object, statusMock.Object);
            
            var message = new MessageData<string> 
            {
                Id = 1,
                Content = "Id,Name,Age\n1,Prajakta,25"
            };
            //Act
            await worker.HandleMessageReceived(message);
            //Assert
            statusMock.Verify(x=>x.MarkAsPending(1), Times.Once); 
            statusMock.Verify(x=>x.MarkAsCompleted(1), Times.Once); 
            converterMock.Verify(x=> x.ConvertAsync(It.IsAny<string>()), Times.Once); 


        }
    }
}
