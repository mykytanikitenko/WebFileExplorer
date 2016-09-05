using System;
using Newtonsoft.Json;

namespace WebFileExplorer.ServiceDomain.Api.Requests
{
    public class FileSystemRequest : IEquatable<FileSystemRequest>
    {
        [JsonProperty("cached")]
        public bool Cached { get; set; }

        [JsonProperty("sourceToken")]
        public Guid SourceToken { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        public bool Equals(FileSystemRequest other)
        {
            return other.Path == Path;
        }

        public override int GetHashCode()
        {
            return Path.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as FileSystemRequest);
        }
    }
}
