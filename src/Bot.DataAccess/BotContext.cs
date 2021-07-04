using Bot.DataAccess.DbModels;
using Microsoft.EntityFrameworkCore;

namespace Bot.DataAccess
{
    public class BotContext : DbContext
    {
        public BotContext(DbContextOptions options) : base(options) { }
        public DbSet<GuildModel> Guilds { get; set; }
    }
}
