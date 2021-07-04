using Bot.DataAccess.Contract;
using Bot.DataAccess.DbModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Bot.DataAccess
{
    public class DataAccessLayer : IDataAccessLayer
    {
        private readonly BotContext _db;

        public DataAccessLayer(BotContext db)
        {
            _db = db;
        }

        public async Task<string> GetPrefixAsync(ulong guildId)
        {
            var prefix = await _db.Guilds
                .Where(x => x.GuildId == guildId)
                .Select(x => x.Prefix)
                .FirstOrDefaultAsync();

            return prefix;
        }

        public async Task SetPrefixAsync(ulong guildId, string prefix)
        {
            var guild = await _db.Guilds.FirstOrDefaultAsync(x => x.GuildId == guildId);
            if (guild == null)
            {
                await AddGuildAsync(guildId, prefix);
            }
            else
            {
                guild.Prefix = prefix;
            }

            await _db.SaveChangesAsync();
        }

        public async Task AddGuildAsync(ulong guildId, string prefix = null)
        {
            var guild = new GuildModel()
            {
                GuildId = guildId,
                Prefix = prefix
            };
            _db.Add(guild);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> GuildExists(ulong guildId)
        {
            var guild = await _db.Guilds.FirstOrDefaultAsync(x => x.GuildId == guildId);
            if (guild == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
