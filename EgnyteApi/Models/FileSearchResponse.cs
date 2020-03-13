using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EgnyteApi.Models
{
    public class FileSearchResponse
    {
        public int Count { get; set; }
        public int Offset { get; set; }
        public bool HasMore { get; set; }
        
        [JsonProperty("total_count")]
        public int TotalCount { get; set; }

        public List<FileSearchResponseResult> Results { get; set; }
    }

    public class FileSearchResponseResult
    {
        public string Name { get; set; }
		public string Path { get; set; }
		public string Type { get; set; }
		public int Size { get; set; }
		public string Snippet { get; set; }

        [JsonProperty("entry_id")]
		public Guid EntryId { get; set; }

        [JsonProperty("group_id")]
		public Guid GroupId { get; set; }

        [JsonProperty("last_modified")]
		public DateTime LastModified { get; set; }

        [JsonProperty("uploaded_by")]
		public string UploadedBy { get; set; }

        [JsonProperty("uploaded_by_username")]
		public string UploadedByUsername { get; set; }

        [JsonProperty("num_versions")]
		public int NumberOfVersions { get; set; }

        [JsonProperty("snippet_html")]
        public string SnippetHtml { get; set; }

        [JsonProperty("is_folder")]
		public bool IsFolder { get; set; }
    }
}
