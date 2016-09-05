using System.Collections.Generic;
using WebFileExplorer.ServiceDomain.Domain;

namespace WebFileExplorer.ApiDomain.FileExplorer.Responses
{
    public class FileSystemEntiresResponse
    {
        public IEnumerable<FileSystemItem> Entries { get; set; } 
    }
}
