using Discord;

namespace BotDiscord.EmbedBuilders.EmbedBuilders
{
    /// <summary>
    /// Builds a basic blue embed with timestamp.
    /// </summary>
    public class DefaultEmbedBuilder : EmbedBuilder
    {
        public DefaultEmbedBuilder()
        {
            WithColor(Discord.Color.Blue)
                .WithCurrentTimestamp();
        }
    }
}
