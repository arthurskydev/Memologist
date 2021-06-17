namespace Bot.Common.EmbedBuilders
{
    using Discord;

    class DefaultEmbedBuilder : EmbedBuilder
    {
        public DefaultEmbedBuilder()
        {
            this.WithColor(Discord.Color.Blue)
                .WithCurrentTimestamp();
        }
    }
}
