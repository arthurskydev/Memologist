using Bot.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace Bot.DataAccess
{
    public class GuildContext : DbContext
    {
        public GuildContext(DbContextOptions options) : base(options) { }
        public DbSet<GuildModel> GuildModels { get; set; }
    }
}
