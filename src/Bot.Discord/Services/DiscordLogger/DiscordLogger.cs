using Discord.Commands;
using Microsoft.Extensions.Logging;

namespace Bot.Client.Services.DiscordLoggerService
{
    class DiscordLogger : IDiscordLogger
    {
        private readonly ILogger _logger;

        public DiscordLogger(ILogger<DiscordLogger> logger)
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
