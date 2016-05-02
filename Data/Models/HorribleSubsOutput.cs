using System;
using System.Collections.Generic;

namespace Data.Models
{
    public class HorribleSubsOutput
    {
        public string InputParams { get; set; }

        public DateTime DateRetrived { get; set; }

        public IList<HorribleSubsItem> Items { get; set; }
    }

    public class HorribleSubsItem
    {
        public DateTime DateCreated { get; set; }

        public string Name { get; set; }

        public Dictionary<String, String> Links { get; set; }
    }
}
