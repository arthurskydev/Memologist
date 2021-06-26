using Common.StringService;
using Common.Helpers;
using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bot.EventHandlers
{
    internal class MiscEventHandler : DiscordClientService
    {
        private readonly IServiceProvider _provider;
        private readonly CommandService _commandService;
        private readonly IConfiguration _configuration;
        private readonly Utilities _utilities;
        private readonly IStringService _stringService;

        public MiscEventHandler(
            DiscordSocketClient client,
            ILogger<MiscEventHandler> logger,
            IServiceProvider provider,
            CommandService commandService,
            IConfiguration configuration,
            Utilities utilities,
            IStringService stringService) : base(client, logger)
        {
            _provider = provider;
            _commandService = commandService;
            _configuration = configuration;
            _utilities = utilities;
            _stringService = stringService;
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Client.MessageReceived += OnMessageRecieved;
            return Task.CompletedTask;
        }

        private async Task OnMessageRecieved(SocketMessage socketMessage)
        {
            Logger.LogDebug($"Message Recieved: {socketMessage.Id} by {socketMessage.Author.Username}.");
            if (!(socketMessage is SocketUserMessage message))
            {
                return;
            }

            if (message.Source != MessageSource.User)
            {
                return;
            }

            int argPos = 0;
            if (!message.HasStringPrefix(_configuration["Prefix"], ref argPos) &&
                !message.HasMentionPrefix(Client.CurrentUser, ref argPos))
            {
                return;
            }

            Logger.LogDebug($"Message was recognized as a command: {message.Content}");

            if (_utilities.IsAllCaps(message.Content.Substring(_configuration["Prefix"].Length - 1)))
            {
                await message.ReplyAsync(_stringService["whyallcaps"]);
            }

            var context = new SocketCommandContext(Client, message);
            await _commandService.ExecuteAsync(context, argPos, _provider);
        }
    }
}