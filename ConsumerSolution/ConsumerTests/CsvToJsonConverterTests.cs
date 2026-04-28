using Consumer.Infrastructure;
using Xunit;

namespace Consumer.Tests
{     
    public class CsvToJsonConverterTests
    {
        [Fact] 
        public async Task ConvertAsync_ValidCsv_ReturnsCorrectData()
        {
            //arrange 
            var converter = new CsvToJsonConverter(); 
            var csv = "Id,Name,Value\n1,Prajakta,25\n2,Shweta,24"; 
            //act 
            var result = await converter.ConvertAsync(csv);
            //assert  //used to verify result
            Assert.NotNull(result); 
            Assert.Equal(2, result.Count); 
            Assert.Equal(1, result[0]["Id"]); 
            Assert.Equal("Prajakta", result[0]["Name"]); 
            Assert.Equal("25", result[0]["Age"].ToString()); 
        }
    }
}
