namespace WebFileExplorer.ServiceDomain.Domain
{
    public class CountFilesResult
    {
        public int FilesLessTen { get; set; }
        public int FilesLessFiftyAndGreaterTen { get; set; }
        public int FilesGreaterHundred { get; set; }
    }
}
