//reads files from folder, reads each file content and returns list of objects
using Producer.Application;
using Producer.Domain;

namespace Producer.Infrastructure
{
    public class FileFetcher : IFileFetcher
    {
        public async Task<List<FileData>> GetFilesAsync(string folderPath)
        {
            // file to list of object
            var files = new List<FileData>();
 
            /* get all xml files from the specified folder name
             * directory - built in class provides methods for directories
             * here used because we are working with the directory 
             * 
             */
            var paths = Directory.GetFiles(folderPath, "*.xml");
 
            foreach (var path in paths)
            {
                /* reads file content
                 * File - in built class - reads file content as string
                 * await - waits till file reading is complete
                 */
                var content = await File.ReadAllTextAsync(path);

                //adding and collecting the files to list
                files.Add(new FileData(
                    //seperates the filepath and filename
                    //Path - built in class
                    Path.GetFileName(path), // stores filename only
                    path,
                    content
                ));
                //FileData object now contains - filename, filepath and content
            }
 
            //returns collected FileData
            return files;
        }
    }
}
