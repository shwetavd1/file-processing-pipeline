//using AutoFixture;
//using Moq;
//using Producer.Application;
//using Producer.Domain;
//using ProducerService;
//using System.Text;

//namespace Producer.Tests
//{
//    [TestFixture]
//    public class WorkerTests
//    {
//        private Mock<IFileFetcher> _fileFetcherMock;
//        private Mock<IFileProcessor<FileData, string>> _fileProcessorMock;
//        private Mock<IMessagePublisher> _messagePublisherMock;
//        private Mock<IFileConversion> _converterMock;
//        private Mock<ProducerEvents> _producerEventsMock;
//        private Worker _worker;

//        [SetUp]
//        public void SetUp()
//        {
//            _fileFetcherMock = new Mock<IFileFetcher>();
//            _fileProcessorMock = new Mock<IFileProcessor<FileData, string>>();
//            _messagePublisherMock = new Mock<IMessagePublisher>();
//            _converterMock = new Mock<IFileConversion>();
//            _producerEventsMock = new Mock<ProducerEvents>();

//            _worker = new Worker(
//                _fileFetcherMock.Object,
//                _fileProcessorMock.Object,
//               _messagePublisherMock.Object);
//        }

//        [Test]
//        public async Task ExecuteAsync_WhenFileFound_ProcessesAndPublishes()
//        {
//            var stream = new MemoryStream(Encoding.UTF8.GetBytes("<People></People>"));

//            var file = new FileData("test.xml", "path", stream);

//            _fileFetcherMock.Setup(x =>
//            x.GetFilesAsync(It.IsAny<string>())).ReturnsAsync(new List<IFileData> { file });

//            _fileProcessorMock.Setup(x =>
//            x.ProcessAsync(file)).ReturnsAsync("csv-data");

//            using var cts = new CancellationTokenSource();
//            cts.CancelAfter(100);

//            await _worker.StartAsync(cts.token);


//        }

//        [Test]
//        public async Task StartAsync_NoFiles_DoesNotPublish()
//        {
//            _fileFetcherMock.Setup(x =>
//            x.GetFilesAsync(It.IsAny<string>())).ReturnsAsync(new List<FileData>());

//            await _worker.StartAsync(CancellationToken.None);

//            _messagePublisherMock.Verify(x =>
//            x.Publish(It.IsAny<string>()), Times.Never);
//        }

//        [TearDown]
//        public void Cleanup()
//        {
//            _worker.Dispose();
//        }
//    }
//}
