using Data.Interfaces;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Data.Repositories
{
    public class ConfigFileRepository : IConfigurationRepository
    {
        private Configuration _config;

        public ConfigFileRepository()
        {
            this._config = new Configuration();
            this._config.Provider = CultureInfo.InvariantCulture;
        }

        public Configuration GetConfig()
        {
            //read last updated date from date file
            DateTime lastDate = DateTime.ParseExact(File.ReadAllText("Config\\date.txt"), "MM/dd/yy", this._config.Provider);
            this._config.Date = lastDate;

            //read shows into list
            this._config.Shows = File.ReadAllLines("Config\\shows.txt").ToList();

            this._config.Size = "720p";

            return this._config;
        }

        public void UpdateLastRunDate()
        {
            File.WriteAllText("Config\\date.txt", DateTime.Now.ToString("MM/dd/yy").Replace('-', '/'));
        }
    }
}
