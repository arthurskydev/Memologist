namespace Bot.Modules
{
    using System.Threading.Tasks;
    using Bot.Common.EmbedBuilders;
    using Bot.Services.StringProcService;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// General useful commands that everyone can use.
    /// </summary>
    public class GeneralCommandModule : ModuleBase<SocketCommandContext>
    {
        private readonly IStringProcService _stringProcessor;
        private readonly IConfiguration _config;

        public GeneralCommandModule(
            IStringProcService stringProcService,
            IConfiguration config)
        {
            _stringProcessor = stringProcService;
            _config = config;
        }

        [Command("info")]
        [Alias("i", "user", "u")]
        [Summary("Replies with the User ID, Tag and Descriminator aswell as the Date the User was created.")]
        public async Task InfoAsnyc(SocketGuildUser user = null)
        {
            await Context.Channel.TriggerTypingAsync();
            user = user ?? this.Context.User as SocketGuildUser;

            var builder = new DefaultEmbedBuilder()
                .WithTitle($"{_stringProcessor["whois"]} {user.Username}#{user.Discriminator}?")
                .WithThumbnailUrl(user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
                .AddField($"{_stringProcessor["userid"]}", user.Id)
                .AddField($"{_stringProcessor["username"]}", user.Username, true)
                .AddField($"{_stringProcessor["descriminator"]}", user.Discriminator, true)
                .AddField($"{_stringProcessor["createdat"]}", user.CreatedAt.ToString("dd/MM/yyyy"))
                .AddField($"{_stringProcessor["joinedat"]}", user.JoinedAt.Value.ToString("dd/MM/yyyy"));
            var embed = builder.Build();
            await this.ReplyAsync(null, false, embed);
        }
    }
}
