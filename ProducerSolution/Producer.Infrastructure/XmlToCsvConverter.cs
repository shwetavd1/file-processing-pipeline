/* read xml
 * convert to csv format
 */
using System.Xml.Linq;
using Producer.Application;
namespace Producer.Infrastructure
{
    public class XmlToCsvConverter : IConverterStrategy
    {
        // used aysnc because file processing takes time, and we do not want to wait for it
        public async Task<string> ConvertAsync(string xmlContent)
        {
            //converts string 
            var doc = XDocument.Parse(xmlContent);

            //loops records
            var records = doc.Descendants("Person").Select(x => new
            {
                Id = x.Element("Id")?.Value,
                Name = x.Element("Name")?.Value,
                Age = x.Element("Age")?.Value
            });

            var csv = "Id,Name,Age\n" + string.Join("\n", records.Select(r =>
            $"{r.Id},{r.Name},{r.Age}\n"));

            return await Task.FromResult(csv);
        }
    }
}
