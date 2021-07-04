using Bot.Common.Contract;
using Bot.DataAccess.Contract;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Bot.Client.Modules
{
    internal class AdminCommandModule : ModuleBase<SocketCommandContext>
    {
        private readonly IDataAccessLayer _dataAccess;
        private readonly IStringService _stringService;
        private readonly IConfiguration _configuration;

        public AdminCommandModule(
            IDataAccessLayer dataAccess,
            IStringService stringService,
            IConfiguration configuration)
        {
            _dataAccess = dataAccess;
            _stringService = stringService;
            _configuration = configuration;
        }

        [Command("prefix")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [Summary("Change the bot's prefix. Only valid for your server. Type \\reset as the argument to reset the prefix to the default.")]
        public async Task PrefixAsync(string prefix)
        {
            if (prefix == "\\reset")
            {
                prefix = _configuration["Prefix"];
            }
            await _dataAccess.SetPrefixAsync(Context.Guild.Id, prefix);
            await ReplyAsync(_stringService[$"**{_stringService["adminprefixchanged"]} {prefix}**"]);
        }
    }
}
