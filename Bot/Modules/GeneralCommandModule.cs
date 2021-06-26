using Bot.Common.EmbedBuilders;
using Common.StringService;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Bot.Modules
{
    /// <summary>
    /// General useful commands that everyone can use.
    /// </summary>
    public class GeneralCommandModule : ModuleBase<SocketCommandContext>
    {
        private readonly IStringService _stringService;
        private readonly IConfiguration _config;

        public GeneralCommandModule(
            IStringService stringService,
            IConfiguration config)
        {
            _stringService = stringService;
            _config = config;
        }

        [Command("info")]
        [Alias("i", "user", "u")]
        [Summary("Replies with the User ID, Tag and Descriminator aswell as the Date the User was created.")]
        public async Task InfoAsnyc(SocketGuildUser user = null)
        {
            await Context.Channel.TriggerTypingAsync();
            user = user ?? Context.User as SocketGuildUser;

            var embed = new DefaultEmbedBuilder()
                .WithTitle($"{_stringService["whois"]} {user.Username}#{user.Discriminator}?")
                .WithThumbnailUrl(user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
                .AddField($"{_stringService["userid"]}", user.Id)
                .AddField($"{_stringService["username"]}", user.Username, true)
                .AddField($"{_stringService["descriminator"]}", user.Discriminator, true)
                .AddField($"{_stringService["createdat"]}", user.CreatedAt.ToString("dd/MM/yyyy"))
                .AddField($"{_stringService["joinedat"]}", user.JoinedAt.Value.ToString("dd/MM/yyyy"))
                .Build();

            await this.ReplyAsync(null, false, embed);
        }
    }
}
