namespace Bot.Services.EventHandlers
{
    using Bot.Common;
    using Bot.Services.StringProcService;
    using Discord;
    using Discord.Addons.Hosting;
    using Discord.Commands;
    using Discord.WebSocket;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    internal class MessageRecievedEventHandler : InitializedService
    {
        private readonly IServiceProvider _provider;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commandService;
        private readonly IConfiguration _configuration;
        private readonly Utilities _utilities;
        private readonly ILogger _logger;
        private readonly IStringProcService _stringProcessor;

        public MessageRecievedEventHandler(
            IServiceProvider provider,
            DiscordSocketClient client,
            CommandService commandService,
            IConfiguration configuration,
            Utilities utilities,
            ILogger<MessageRecievedEventHandler> logger,
            IStringProcService stringProcessor)
        {
            _provider = provider;
            _client = client;
            _commandService = commandService;
            _configuration = configuration;
            _utilities = utilities;
            _logger = logger;
            _stringProcessor = stringProcessor;
        }

        public override Task InitializeAsync(CancellationToken cancellationToken)
        {
            _client.MessageReceived += OnMessageRecieved;
            return Task.CompletedTask;
        }

        private async Task OnMessageRecieved(SocketMessage socketMessage)
        {
            _logger.LogDebug($"Message Recieved: {socketMessage.Id} by {socketMessage.Author.Username}.");
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
                !message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                return;
            }

            _logger.LogDebug($"Message was recognized as a command: {message.Content}");

            if (_utilities.IsAllCaps(message.Content))
            {
                await message.ReplyAsync(_stringProcessor["whyallcaps"]);
            }

            var context = new SocketCommandContext(_client, message);
            await _commandService.ExecuteAsync(context, argPos, _provider);
        }
    }
}
