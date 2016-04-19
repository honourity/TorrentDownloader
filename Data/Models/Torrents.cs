using System;
using System.Collections.Generic;

namespace Data.Models
{
    public class TorrentSource
    {
        public string Name { get; set; }

        public DateTime DateRetrieved { get; set; }

        public IList<TorrentItem> TorrentItems { get; set; }
    }

    public class TorrentItem
    {
        public DateTime DateCreated { get; set; }

        public string Name { get; set; }

        public Dictionary<String, String> Links { get; set; }
    }
}
