namespace Bot.Common.EmbedBuilders
{
    using Discord;

    /// <summary>
    /// Builds a basic blue embed with timestamp.
    /// </summary>
    class DefaultEmbedBuilder : EmbedBuilder
    {
        public DefaultEmbedBuilder()
        {
            this.WithColor(Discord.Color.Blue)
                .WithCurrentTimestamp();
        }
    }
}
