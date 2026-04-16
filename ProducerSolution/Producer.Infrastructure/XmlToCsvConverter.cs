/* read xml
 * convert to csv format
 */
using System.Xml.Linq;
using Producer.Application;
namespace Producer.Infrastructure
{
    public class XmlToCsvConverter : IConverterStrategy
    {
        // used aysnc because file processing takes time
        // method returns csv content as string
        public async Task<string> ConvertAsync(string xmlContent)
        {
            /* converts to string
             * XDocument - built in class represents entire XML used for XML parsing
             * it stores - root element, child element, element names, values
             * parses the string into objects - XElement
            */
            var doc = XDocument.Parse(xmlContent);

            //loops records
            /* descendants - traverses xml tree and returns all child elements 
             * it returns collection XElement
            */
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
