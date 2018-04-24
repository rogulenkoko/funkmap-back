using System.Collections.Generic;
using Funkmap.Domain.Models;

namespace Funkmap.Models.Responses
{
    public class SearchResponse
    {
        public List<SearchItem> Items { get; set; }

        public long AllCount { get; set; }
    }
}
