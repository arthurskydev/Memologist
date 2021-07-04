using Bot.DataAccess.Contract;
using Discord.Addons.Hosting;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Bot.Client.EventHandlers
{
    internal class GuildConntectedHandler : DiscordClientService
    {
        private readonly IDataAccessLayer _dataAccess;
        public GuildConntectedHandler(
            DiscordSocketClient client, 
            ILogger<GuildConntectedHandler> logger,
            IDataAccessLayer dataAccess) : base(client, logger)
        {
            _dataAccess = dataAccess;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Client.JoinedGuild += OnGuildConnected;
            Client.GuildAvailable += OnGuildConnected;
            return Task.CompletedTask;
        }

        private async Task OnGuildConnected(SocketGuild arg)
        {
            if (!await _dataAccess.GuildExists(arg.Id))
            {
               await _dataAccess.AddGuildAsync(arg.Id);
            }
        }
    }
}
