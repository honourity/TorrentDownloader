using Data.Models;

namespace Data.Interfaces
{
    public interface ITorrentRepository
    {
        TorrentSource GetTorrents(Configuration config);
    }
}
