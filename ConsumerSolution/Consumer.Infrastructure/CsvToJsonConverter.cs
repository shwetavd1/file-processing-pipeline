using Consumer.Application;
using Consumer.Domain;

namespace Consumer.Infrastructure //Infrasturucture layer contains actual implementation
{
    public class CsvToJsonConverter : ICsvToJsonConverter // Interface defines what to do, class defines how to do [Abstraction+Dependency Inversion]
    {
        public async Task<List<ProcessedData>> ConvertAsync (string csvData) 
        {
            var result = new List<ProcessedData>(); // creating empty list to store output.

            if (string.IsNullOrWhiteSpace (csvData))// it prevents empty input and avoid crash later
            {
                throw new ArgumentException("CSV data is empty");
            }
            var lines = csvData.Split ('\n', StringSplitOptions.RemoveEmptyEntries); //breaks csv into rows //RemoveEmptyEntries used to avoid empty rows

            if (lines.Length <= 1)
            {
                throw new Exception("No data rows found in CSV");
            }
            foreach (var line in lines.Skip(1)) //skip header // Loop through all rows except first because first row is header
            {
                try //used try catch for if one row fails, others still process, to make system robust
                {
                    var values = line.Split(','); //split rows into columns

                    //validation to check column count
                    if (values.Length<3)
                    {
                        throw new Exception($"Invalid row format:{line}");
                    }
                    //validation for safe parsing
                    if (!int.TryParse(values[0], out int id))
                    {
                        throw new Exception($"Invalid Id:{values[0]}");
                    }
                    if (!int.TryParse(values[0], out int age))
                    {
                        throw new Exception($"Invalid Age:{values[2]}");
                    }
                    var data = new ProcessedData
                    {
                        Id = int.Parse(values[0]),
                        Name = values[1],
                        Age = int.Parse(values[2]) // used int.Parse bcoz csv gives string but here id and age are numbers
                    };// convert string values into structured object
                    result.Add(data); // it stores converted object
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Error processing row: {line}");
                    Console.WriteLine($"Reason: {ex.Message}");
                }   
            }
            return await Task.FromResult(result);
        }
    }
}

/*
for this we can we csv library(CsvHelper) it handles comples csv, handles quotes,commas inside value.
but i am not using it here because for learning and simplicity purpose, i have implemented manual parsing.
 */
