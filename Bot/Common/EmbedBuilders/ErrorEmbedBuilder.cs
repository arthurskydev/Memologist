namespace Bot.Common.EmbedBuilders
{
    using Bot.Services.StringProcService;
    using Discord;

    /// <summary>
    /// Builds a red embed with the error title.
    /// </summary>
    internal class ErrorEmbedBuilder : EmbedBuilder
    {
        private readonly IStringProcService _stringProcessor;

        public ErrorEmbedBuilder(IStringProcService stringProcService)
        {
            _stringProcessor = stringProcService;
            this.WithColor(Discord.Color.Red)
                .WithTitle(_stringProcessor["commanderror"]);
        }
    }
}
