using Data.Models;

namespace Data.Interfaces
{
    public interface IConfigurationRepository
    {
        Configuration GetConfig();

        void UpdateLastRunDate();
    }
}
