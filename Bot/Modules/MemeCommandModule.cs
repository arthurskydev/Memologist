namespace Bot.Modules
{
    using Bot.Services.StringProcService;
    using Discord.Commands;
    using System.Threading.Tasks;

    class MemeCommandModule : ModuleBase<SocketCommandContext>
    {
        private protected IStringProcService _stringProcService;

        public MemeCommandModule(IStringProcService stringProcService)
        {
            _stringProcService = stringProcService;
        }

        [Command("meme")]
        [Alias("m", "memes")]
        [Name("Meme")]
        [Summary("Sends back an image of a random meme or meme with specified tag.")]
        public async Task MemeAsync()
        {
            
        }
    }
}
