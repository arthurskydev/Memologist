using Common.StringService;
using Discord;

namespace Bot.Common.EmbedBuilders
{
    /// <summary>
    /// Builds a red embed with the error title.
    /// </summary>
    public class ErrorEmbedBuilder : EmbedBuilder
    {
        private readonly IStringService _stringService;

        public ErrorEmbedBuilder(IStringService stringService)
        {
            _stringService = stringService;
            WithColor(Discord.Color.Red)
               .WithTitle(_stringService["commanderror"]);
        }
    }
}
