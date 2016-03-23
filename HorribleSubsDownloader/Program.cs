using System.Diagnostics;
using Data.Interfaces;

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

            foreach (string link in torrentData.TorrentFileLinks)
            {
                Process.Start(link);
            }
            
            configRepository.UpdateLastRunDate();
        }
    }
}
