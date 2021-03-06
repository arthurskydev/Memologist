using Discord.Commands;

namespace Bot.Client.Services.DiscordLoggerService
{
    public enum LoggingEvent
    {
        UserKick,
        UserBan,
        UserUnban,
        MessagePurge
    }

    public interface IDiscordLogger
    {
        /// <summary>
        /// If guild has discord logging set up: loggs to the logging channel.
        /// </summary>
        void DiscordCommandLog(SocketCommandContext context, LoggingEvent loggingEvent, string loggingDescription);
    }
}
