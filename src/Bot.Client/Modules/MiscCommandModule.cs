using Bot.Common.StringService;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace Bot.Client.Modules
{
    /// <summary>
    /// Commands that aren't useful, memes or only for testing purposes.
    /// </summary>
    public class MiscCommandModule : ModuleBase<SocketCommandContext>
    {
        private protected IStringService _stringService;

        public MiscCommandModule(IStringService stringProcService)
        {
            _stringService = stringProcService;
        }

        [Command("ping")]
        [Alias("test")]
        [Name("Ping")]
        [Summary("Simply replies to a message with \"Ping!\" and sends a DM to the user who executed it.")]
        public async Task PingAsync()
        {
            await Context.Channel.TriggerTypingAsync();
            await ReplyAsync(_stringService["pinganswer"]);
            await Context.User.SendMessageAsync(_stringService["pingdmanswer"]);
        }
    }
}
