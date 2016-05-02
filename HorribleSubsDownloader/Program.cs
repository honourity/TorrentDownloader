using System.Diagnostics;
using Data.Interfaces;
using Data.Models;

namespace HorribleSubsDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfigurationRepository configRepository = new Data.Repositories.ConfigFileRepository();
            var config = configRepository.GetConfig();

            ITorrentRepository torrentRepository = new Data.Repositories.HorribleSubsRepository();
            var torrentData = torrentRepository.GetTorrents(config);

            foreach (TorrentItem item in torrentData.TorrentItems)
            {
                Process.Start(item.Links[config.Size]);
            }
            
            configRepository.UpdateLastRunDate();
        }
    }
}
