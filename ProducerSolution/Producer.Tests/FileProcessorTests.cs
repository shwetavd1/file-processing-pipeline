//using Moq;
//using Producer.Application;
//using Producer.Infrastructure;
//using Producer.Domain;

//namespace Producer.Tests
//{
//    [TestFixture]
//    public class FileProcessorTests
//    {
//        private Mock <IConverterStrategy> _converterMock;
//        private FileProcessor _fileProcessor;

//        [SetUp]
//        public void SetUp()
//        {
//            _converterMock = new Mock<IConverterStrategy>();
//            _fileProcessor = new FileProcessor();
//        }

//        [Test]
//        public async Task ProcessFileAsync_ValidFile_ReturnsCsv()
//        {
//            var file = new FileData(
//                "text.xml",
//                "D:\\Files\\Person.xml",
//                "<People>" +
//                "<Person>" +
//                "<Id>1</Id>" +
//                "<Name>Shweta</Name>" +
//                "<Age>27</Age>" +
//                "</Person>" +
//                "</People>");

//            var expectedCsv = "Id,NAme,Age\n1,Shweta,27";

//            _converterMock.Setup(x =>
//            x.ConvertXmlToCsvAsync(file.Content)).ReturnsAsync(expectedCsv);

//            var result = await _fileProcessor.ProcessAsync(file);

//            Assert.That(result, Is.SameAs(file));
//        }

//        [Test]
//        public void ProcessAsync_EmptyContent_ThrowsException()
//        {
//            var file = new FileData("test.xml", "path", "");

//            Assert.ThrowsAsync<Exception>(async () =>
//                await _fileProcessor.ProcessAsync(file));
//        }

//    }
//}
