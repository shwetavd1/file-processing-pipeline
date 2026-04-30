using System.Text;
using System.Xml.Linq;
using Producer.Application.Conversion;

namespace Producer.Infrastructure.Conversion
{
    public class XmlToCsvConverter : IFileConverter
    {
        public bool canHandle(string fileExtension)
        => fileExtension.Equals(".xml", StringComparison.OrdinalIgnoreCase);
        public async Task<string> ConvertAsync(Stream content)
        {
            var reader = new StreamReader(content);
            var xmlContent = await reader.ReadToEndAsync();
            return ConvertXmlToCsv(xmlContent);
        }
        public string ConvertXmlToCsv(string xmlContent)
        {
            var document = XDocument.Parse(xmlContent);

            var root = document.Root;
            if (root == null)
                throw new InvalidOperationException("Invalid XML: Missing root element");

            var records = root.Elements();
            var headers = records.SelectMany(r => r.Elements()).Select(e => e.Name.LocalName).Distinct().ToList();

            var csv = new StringBuilder();
            csv.AppendLine(string.Join(",", headers));

            foreach (var record in records)
            {
                var values = headers.Select(h => record.Element(h)?.Value ?? string.Empty);
                csv.AppendLine(string.Join(",", values));
            }
            return csv.ToString();
        }
    }
}