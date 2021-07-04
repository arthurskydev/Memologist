using System.Threading.Tasks;

namespace Bot.DataAccess.Contract
{
    public interface IDataAccessLayer
    {
        Task AddGuildAsync(ulong guildId, string prefix = null);
        Task<string> GetPrefixAsync(ulong guildId);
        Task<bool> GuildExists(ulong guildId);
        Task SetPrefixAsync(ulong guildId, string prefix);
    }
}