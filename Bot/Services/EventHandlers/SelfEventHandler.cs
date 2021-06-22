using Bot.Common.EmbedBuilders;
using Discord.Addons.Hosting;
using Discord.WebSocket;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bot.Services.EventHandlers
{
    class SelfEventHandler : InitializedService
    {
        private readonly DiscordSocketClient _client;

        public SelfEventHandler(DiscordSocketClient client)
        {
            _client = client;
        }

        public override Task InitializeAsync(CancellationToken cancellationToken)
        {
            _client.JoinedGuild += OnJoinAsync;
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
