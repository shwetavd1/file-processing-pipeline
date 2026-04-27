namespace Producer.Domain
{
    public interface IFileData
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        IList<IFileData> Folders { get; }
    }

    public class FileData : IFileData
    {
        public string FileName { get; private set; }
        public string FilePath { get; private set; }
        public Stream Content { get; private set; }
        string IFileData.FileName { get => FileName; set => FileName = value; }
        string IFileData.FilePath { get => FilePath; set => FilePath = value; }
        public IList<IFileData> Folders { get; } = new List<IFileData>();

        public FileData(string fileName, string filePath, Stream content)
        {
            FileName = fileName;
            FilePath = filePath;
            Content = content;
        }
    }
}
