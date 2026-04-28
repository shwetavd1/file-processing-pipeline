using Consumer.Application;
using Consumer.Domain;

namespace Consumer.Infrastructure 
{
    public class CsvToJsonConverter : ICsvToJsonConverter<Dictionary<string, object>>
    {
        public async Task<List<Dictionary<string,object>>> ConvertAsync(string csvContent)
        {
            var result = new List<Dictionary<string,object>>();
            if (string.IsNullOrWhiteSpace(csvContent))
            {
                throw new ArgumentException("CSV data is empty");
            }
            var lines = csvContent.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length <= 1)
            {
                throw new Exception("No data rows found in CSV");
            }
            var headers = lines[0].Split(','); //read header row
            foreach (var line in lines.Skip(1)) // process each row except header
            {
                try
                {
                    var values = line.Split(','); //split rows into columns

                    if (values.Length != headers.Length)
                    {
                        throw new Exception($"Column count mismatched in row:{line}");
                    }
                    if (!int.TryParse(values[0], out int id))
                    {
                        throw new Exception($"Invalid Id:{values[0]}");
                    }
                    if (!int.TryParse(values[2], out int age))
                    {
                        throw new Exception($"Invalid Age:{values[2]}");
                    }
                    var rowData = new Dictionary<string, object>();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        rowData[headers[i].Trim()] = values[i].Trim();
                    }
                    result.Add(rowData);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing row: {line}");
                    Console.WriteLine($"Reason: {ex.Message}");
                }
            }
            return result;
        }
    }
}

