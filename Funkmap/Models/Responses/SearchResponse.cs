using System.Collections.Generic;

namespace Funkmap.Models.Responses
{
    public class SearchResponse
    {
        public ICollection<SearchModel> Items { get; set; }

        public ICollection<string> AllLogins { get; set; }

        public long AllCount { get; set; }
    }
}
