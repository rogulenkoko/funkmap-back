using System.Collections.Generic;

namespace Funkmap.Models.Responses
{
    public class SearchResponse
    {
        public List<SearchItem> Items { get; set; }

        public long AllCount { get; set; }
    }
}
