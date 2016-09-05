namespace WebFileExplorer.ServiceDomain.Api.Responses
{
    public class CountFilesResponse
    {
        public int FilesLessTen { get; set; }
        public int FilesLessFiftyAndGreaterTen { get; set; }
        public int FilesGreaterHundred { get; set; }
        public bool Timeouted { get; set; }
    }
}
