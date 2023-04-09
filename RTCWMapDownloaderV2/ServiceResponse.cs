using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace RTCWMapDownloader
{
    public class ServiceResponse
    {
        [JsonProperty("file")]
        public string File { get; set; }

        [JsonProperty("checksum")]
        public string Checksum { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("downloadMirrors")]
        public List<DownloadMirror> DownloadMirrors { get; set; }

        [JsonProperty("levelshotUrl")]
        public string LevelshotUrl { get; set; }

        [JsonProperty("levelshotThumb")]
        public string LevelshotThumb { get; set; }
    }

    public class DownloadMirror
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("fileType")]
        public string FileType { get; set; }

        [JsonProperty("byteSize")]
        public int ByteSize { get; set; }

        public bool Used { get; set; }
    }
}
