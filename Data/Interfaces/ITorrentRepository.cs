using Data.Models;

namespace Data.Interfaces
{
    public interface ITorrentRepository
    {
        Torrents GetTorrents(Configuration config);
    }
}
