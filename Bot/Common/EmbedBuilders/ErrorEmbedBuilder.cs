using Bot.Services.StringProcService;
using Discord;

namespace Bot.Common.EmbedBuilders
{
    /// <summary>
    /// Builds a red embed with the error title.
    /// </summary>
    public class ErrorEmbedBuilder : EmbedBuilder
    {
        private readonly IStringProcessor _stringProcessor;

        public ErrorEmbedBuilder(IStringProcessor stringProcService)
        {
            _stringProcessor = stringProcService;
            WithColor(Discord.Color.Red)
               .WithTitle(_stringProcessor["commanderror"]);
        }
    }
}
