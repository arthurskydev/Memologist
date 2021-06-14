namespace Bot.Modules
{
    using System.Threading.Tasks;
    using Bot.Services.StringProcService;
    using Discord;
    using Discord.Commands;

    /// <summary>
    /// Commands that aren't useful, memes or only for testing purposes.
    /// </summary>
    public class MiscCommandModule : ModuleBase<SocketCommandContext>
    {
        private protected IStringProcService _stringProcService;

        public MiscCommandModule(IStringProcService stringProcService)
        {
            _stringProcService = stringProcService;
        }

        [Command("ping")]
        [Alias("test")]
        [Name("Ping")]
        [Summary("Simply replies to a message with \"Ping!\" and sends a DM to the user who executed it.")]
        public async Task PingAsync()
        {
            await Context.Channel.TriggerTypingAsync();
            await ReplyAsync(_stringProcService["pinganswer"]);
            await Context.User.SendMessageAsync(_stringProcService["pingdmanswer"]);
        }
    }
}
