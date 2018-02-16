using System.Collections.Generic;

namespace Funkmap.Models.Responses
{
    public class SearchResponse
    {
        public List<SearchModel> Items { get; set; }

        public long AllCount { get; set; }
    }
}
