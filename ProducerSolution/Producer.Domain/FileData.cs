// represents the file object

namespace Producer.Domain
{
    public class FileData
    {
        // can only read but cannot modify from outside - used encapsulation
        public string FileName { get; private set; }
        public string FilePath { get; private set; }
        public string Content { get; private set; }

        //object to be created with these values
        public FileData(string fileName, string filePath, string content)
        {
            FileName = fileName;
            FilePath = filePath;
            Content = content;
        }
    }
}
