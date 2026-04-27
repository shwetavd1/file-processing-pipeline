using Consumer.Infrastructure;

namespace Consumer.Tests //grouping of test cases
{
    public class CsvToJsonConverterTests
    {
        [Fact] // this tells xunit that this is a test method
        public async Task ConvertAsync_ValidCsv_ReturnsCorrectData()
        {
            //arrange
            var converter = new CsvToJsonConverter(); // creating an object of the class we want to test
            var csv = "Id,Name,Value\n1,Prajakta,25\n2,Shweta,24"; //input for test

            //act //used to execute the method we want to test
            var result = await converter.ConvertAsync(csv);

            //assert //used to verify result
            Assert.NotNull(result); // check if result is not null, methos should return something
            Assert.Equal(2, result.Count); // expect two records
            Assert.Equal(1, result[0].Id); // check first record's Id, id should be 1
            Assert.Equal("Prajakta", result[0].Name); // check first record's Name, name should be Prajakta
            Assert.Equal(25, result[0].Age); // check first record's Age, age should be 25
        }
    }
}
