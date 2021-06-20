using Bot.Common.EmbedBuilders;
using Bot.Services.DiscordLoggerService;
using Bot.Services.StringProcService;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Bot.Modules
{
    public class ModerationCommandModule : ModuleBase<SocketCommandContext>
    {
        private protected IStringProcService _stringProcessor;
        private protected ILogger _logger;
        private protected IDiscordLoggerService _discordLogger;

        public ModerationCommandModule(
            IStringProcService stringProc,
            ILogger<ModerationCommandModule> logger,
            IDiscordLoggerService discordLogger)
        {
            _stringProcessor = stringProc;
            _logger = logger;
            _discordLogger = discordLogger;
        }

        [Command("purge")]
        [Alias("p")]
        [RequireUserPermission(ChannelPermission.ManageMessages)]
        [Summary("Delete a number of messages in a chat. Requires ")]
        public async Task PurgeAsync(int number = 1)
        {
            var messages = await Context.Channel.GetMessagesAsync(number + 1).FlattenAsync();

            await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages);

            await Context.Channel.TriggerTypingAsync();
            var builder = new DefaultEmbedBuilder()
                .WithDescription($"{number} {_stringProcessor["moderationpurgereply"]} {Context.User.Mention}." +
                "\n" +
                $"*{_stringProcessor["willdeleteshortly"]}*");
            var reply = await ReplyAsync(embed: builder.Build());

            await Task.Delay(5000);

            await Context.Channel.DeleteMessageAsync(reply);
        }

        [Command("kick")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        [RequireBotPermission(GuildPermission.KickMembers)]
        [Summary("Kicks specified user from the guild.")]
        public async Task KickAsync(SocketGuildUser user, string reason)
        {
            user = user ?? Context.User as SocketGuildUser;

            await user.KickAsync(reason);
            _discordLogger.DiscordCommandLog(Context, LoggingEvent.UserKick, 
                $"{Context.User.Id} ({Context.User.Username}#{Context.User.Discriminator})) " +
                $"kicked {user.Id} ({user.Username}#{user.Discriminator}) for {reason}");

            await Context.Channel.TriggerTypingAsync();
            var builder = new DefaultEmbedBuilder()
                .WithTitle($"{Context.User.Mention} ({Context.User.Username}#{Context.User.Discriminator}) {_stringProcessor["moderationkicked"]}")
                .AddField($"{user.Username}#{user.Discriminator} {_stringProcessor["moderationuserwaskicked"]}", $"***{reason}***");

            var embed = builder.Build();
            await ReplyAsync(embed: embed);
        }
    }
}
