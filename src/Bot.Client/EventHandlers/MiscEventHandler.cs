using Bot.Client.EmbedBuilders;
using Bot.Client.Utilities;
using Bot.Common.StringService;
using Bot.DataAccess;
using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bot.Client.EventHandlers
{
    internal class MiscEventHandler : DiscordClientService
    {
        private readonly IServiceProvider _provider;
        private readonly CommandService _commandService;
        private readonly IConfiguration _configuration;
        private readonly IStringService _stringService;
        private readonly IDataAccessLayer _dataAccess;

        public MiscEventHandler(
            DiscordSocketClient client,
            ILogger<MiscEventHandler> logger,
            IServiceProvider provider,
            CommandService commandService,
            IConfiguration configuration,
            IStringService stringService,
            IDataAccessLayer dataAccess) : base(client, logger)
        {
            _provider = provider;
            _commandService = commandService;
            _configuration = configuration;
            _stringService = stringService;
            _dataAccess = dataAccess;
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Client.MessageReceived += OnMessageRecieved;
            Client.JoinedGuild += OnJoinAsync;
            Client.UserJoined += OnUserJoinAsync;
            return Task.CompletedTask;
        }

        private async Task OnMessageRecieved(SocketMessage socketMessage)
        {
            Logger.LogDebug($"Message Recieved: {socketMessage.Id} by {socketMessage.Author.Username}.");
            if (socketMessage is not SocketUserMessage message)
            {
                return;
            }

            if (message.Source != MessageSource.User)
            {
                return;
            }

            int argPos = 0;
            string prefix = await _dataAccess.GetPrefixAsync((socketMessage.Channel as SocketGuildChannel).Guild.Id) ?? _configuration["Prefix"];

            if (!message.HasStringPrefix(prefix, ref argPos) &&
                !message.HasMentionPrefix(Client.CurrentUser, ref argPos))
            {
                return;
            }

            Logger.LogDebug($"Message was recognized as a command: {message.Content}");

            if (MiscUtilities.IsAllCaps(message.Content[(_configuration["Prefix"].Length - 1)..]))
            {
                await message.ReplyAsync(_stringService["whyallcaps"]);
            }

            var context = new SocketCommandContext(Client, message);
            await _commandService.ExecuteAsync(context, argPos, _provider);
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

        private Task OnUserJoinAsync(SocketGuildUser socketGuildUser)
        {
            return Task.CompletedTask;
        }
    }
}