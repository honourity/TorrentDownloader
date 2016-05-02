using System;
using System.Collections.Generic;

namespace Data.Models
{
    public class TorrentSource
    {
        public string Name { get; set; }

        public string InputParams { get; set; }

        public DateTime DateRetrieved { get; set; }

        public IList<Query> Queries { get; set; }
    }

    public class Query
    {
        public string SearchQuery { get; set; }

        public IList<TorrentItem> ResultItems { get; set; }
    }

    public class TorrentItem
    {
        public DateTime DateCreated { get; set; }

        public string Name { get; set; }

        public Dictionary<String, String> ItemQualityChoices { get; set; }
    }
}
