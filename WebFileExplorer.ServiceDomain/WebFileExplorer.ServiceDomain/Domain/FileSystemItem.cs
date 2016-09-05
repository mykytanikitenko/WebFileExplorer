namespace WebFileExplorer.ServiceDomain.Domain
{
    public class FileSystemItem
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public FileSystemItemType Type { get; set; }
    }
}
