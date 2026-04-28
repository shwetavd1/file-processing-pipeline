//using Producer.Infrastructure;
//using System.Xml;
//namespace Producer.Tests
//{
//    [TestFixture]
//    public class XmlToCsvConverterTests
//    {
//        private XmlToCsvConverter _converter;
//        [SetUp]
//        public void Setup()
//        {
//            _converter = new XmlToCsvConverter();
//        }

//        // tests csv is valid or not, does contain all required data, is empty or not
//        [Test]
//        public async Task ConvertXmlToCsvAsync_ValidXml_ReturnsCorrectCsv()
//        {
//            // arrange
//            var xml = "<People>" +
//                "<Person>" +
//                "<Id>1</Id>" +
//                "<Name>Shweta</Name>" +
//                "<Age>27</Age>" +
//                "</Person>" +
//                "</People>";
//            //act
//            var result = await _converter.ConvertXmlToCsvAsync(xml);
//            Console.WriteLine(result);
//            //assert
//            Assert.Multiple(() =>
//            {
//                Assert.That(result, Does.Contain("Shweta"));
//                Assert.That(result, Does.Contain("Id, Name, Age"));
//                Assert.That(result, Is.Not.Null);
//            });
//        }

//        // tests empty file
//        [Test]
//        public async Task ConvertXmlToCsvAsync_EmptyXmlFile_ThrowsException()
//        {
//            var xml = "";

//            var exception = Assert.ThrowsAsync<XmlException>(async () =>
//            await _converter.ConvertXmlToCsvAsync(xml));

//            Console.WriteLine(exception);
//            Assert.That(exception, Is.Not.Null);
//        }

//        // tests the invalid xml content
//        [Test]
//        public async Task ConvertXmlToCsvAsync_InvalidXml_ThrowsException()
//        {
//            var xml = "<People><Person><Id>1</Id>";

//            var exception = Assert.ThrowsAsync<XmlException>(async () =>
//            {
//                await _converter.ConvertXmlToCsvAsync(xml);
//            });

//            Assert.That(exception, Is.Not.Null);
//        }
//    }
//}
