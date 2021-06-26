using System.Security.AccessControl;
using Bot.Common.EmbedBuilders;
using Common.StringService;
using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Bot.EventHandlers
{
    internal class CommandHandler : DiscordClientService
    {
        private readonly IStringService _stringService;
        private readonly CommandService _commandService;
        private readonly IServiceProvider _serviceProvider;

        public CommandHandler(
            DiscordSocketClient client,
            ILogger<CommandHandler> logger, 
            IStringService stringService,
            CommandService commandService,
            IServiceProvider serviceProvider) : base(client, logger)
        {
            _stringService = stringService;
            _commandService = commandService;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);
            _commandService.CommandExecuted += CommandExecuted;
        }

        private async Task CommandExecuted(Optional<CommandInfo> commandInfo, ICommandContext commandContext, IResult result)
        {
            if (result.IsSuccess)
            {
                return;
            }

            var embed = new ErrorEmbedBuilder(_stringService)
                .WithDescription(result.ErrorReason)
                .Build();

            await commandContext.Channel.SendMessageAsync(embed: embed);

            if (result.Error == CommandError.UnknownCommand)
            {
                return;
            }
            Logger.LogWarning($"Exeption while executing command: {result.ErrorReason}");
        }
    }
}
