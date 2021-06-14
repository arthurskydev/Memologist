namespace Bot.EventHandlers
{
    using Bot.Services.StringProcService;
    using Discord;
    using Discord.Addons.Hosting;
    using Discord.Commands;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    internal class CommandExecutedEventHandler : InitializedService
    {
        private readonly ILogger _logger;
        private readonly IStringProcService _stringProcessor;
        private readonly CommandService _commandService;
        private readonly IServiceProvider _serviceProvider;

        public CommandExecutedEventHandler(
            ILogger<CommandExecutedEventHandler> logger, 
            IStringProcService stringProcService,
            CommandService commandService,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _stringProcessor = stringProcService;
            _commandService = commandService;
            _serviceProvider = serviceProvider;
        }

        public override async Task InitializeAsync(CancellationToken cancellationToken)
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

            await commandContext.Channel.SendMessageAsync($"{_stringProcessor["commanderror"]} \n{result.ErrorReason}.");

            if (result.Error == CommandError.UnknownCommand)
            {
                return;
            }
            _logger.LogWarning($"Exeption while executing command: {result.ErrorReason}");
        }
    }
}
