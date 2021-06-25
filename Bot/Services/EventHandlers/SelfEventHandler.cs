using Bot.Common.EmbedBuilders;
using Discord.Addons.Hosting;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bot.Services.EventHandlers
{
    class SelfEventHandler : DiscordClientService
    {
        public SelfEventHandler(DiscordSocketClient client, ILogger<SelfEventHandler> logger) : base(client, logger) { }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Client.JoinedGuild += OnJoinAsync;
            return Task.CompletedTask;
        }

        private async Task OnJoinAsync(SocketGuild socketGuild)
        {
            var embed = new DefaultEmbedBuilder()
                .WithTitle($"I am here.")
                .WithDescription($"I am Memologist. To see a list of commands do \"give help\". To learn more about me do \"give m\".")
                .WithFooter(footer => { footer.WithText($"Thank you for your invitation! ❤️"); })
                .Build();

            await socketGuild.DefaultChannel.SendMessageAsync(embed: embed);
        }
    }
}
