using Bot.DataAccess.Contract;
using Bot.DataAccess.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;

namespace Bot.DataAccess
{
    public class DataAccessLayer : IDataAccessLayer
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public DataAccessLayer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task<string> GetPrefixAsync(ulong guildId)
        {
            using var scope = _scopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<BotContext>();
            var prefix = await context.Guilds
                .Where(x => x.GuildId == guildId)
                .Select(x => x.Prefix)
                .FirstOrDefaultAsync();

            return prefix;
        }

        public async Task SetPrefixAsync(ulong guildId, string prefix)
        {
            using var scope = _scopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<BotContext>();
            var guild = await context.Guilds.FirstOrDefaultAsync(x => x.GuildId == guildId);
            if (guild == null)
            {
                await AddGuildAsync(guildId, prefix);
            }
            else
            {
                guild.Prefix = prefix;
            }

            await context.SaveChangesAsync();
        }

        public async Task AddGuildAsync(ulong guildId, string prefix = null)
        {
            using var scope = _scopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<BotContext>();
            var guild = new GuildModel()
            {
                GuildId = guildId,
                Prefix = prefix
            };
            context.Add(guild);
            await context.SaveChangesAsync();
        }

        public async Task<bool> GuildExists(ulong guildId)
        {
            using var scope = _scopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<BotContext>();
            var guild = await context.Guilds.FirstOrDefaultAsync(x => x.GuildId == guildId);
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
