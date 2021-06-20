using Discord.Commands;
using Microsoft.Extensions.Logging;

namespace Bot.Services.DiscordLoggerService
{
    class DiscordLoggerService : IDiscordLoggerService
    {
        private readonly ILogger _logger;

        public DiscordLoggerService(ILogger<DiscordLoggerService> logger)
        {
            _logger = logger;
        }

        public void DiscordCommandLog(SocketCommandContext context, LoggingEvent loggingEvent, string descriptioin)
        {
            _logger.LogInformation($"{loggingEvent} in {context.Channel.Name}." +
                "\n" +
                $"{descriptioin}");
        }
    }
}
