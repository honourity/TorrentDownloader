using System;
using System.Collections.Generic;
using System.Globalization;

namespace Data.Models
{
    public class Configuration
    {
        public DateTime Date { get; set; }

        public CultureInfo Provider { get; internal set; }

        public List<string> Shows { get; set; }
    }
}
