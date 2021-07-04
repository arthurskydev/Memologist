using Bot.Client.EmbedBuilders;
using Bot.Client.Services.DiscordLoggerService;
using Bot.Common.Contract;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Bot.Client.Modules
{
    /// <summary>
    /// Commands for Moderators.
    /// </summary>
    public class ModerationCommandModule : ModuleBase<SocketCommandContext>
    {
        private protected IStringService _stringService;
        private protected ILogger _logger;
        private protected IDiscordLogger _discordLogger;

        public ModerationCommandModule(
            IStringService stringService,
            ILogger<ModerationCommandModule> logger,
            IDiscordLogger discordLogger)
        {
            _stringService = stringService;
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
            _discordLogger.DiscordCommandLog(Context, LoggingEvent.MessagePurge,
                $"{Context.User.Id} ({Context.User.Username}#{Context.User.Discriminator}) purged {number} messages");

            await Context.Channel.TriggerTypingAsync();

            var embed = new DefaultEmbedBuilder()
                .WithDescription($"{number} {_stringService["moderationpurgereply"]} {Context.User.Mention}." +
                "\n" +
                $"*{_stringService["willdeleteshortly"]}*")
                .Build();

            var reply = await ReplyAsync(embed: embed);

            await Task.Delay(5000);

            await Context.Channel.DeleteMessageAsync(reply);
        }

        [Command("kick")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        [RequireBotPermission(GuildPermission.KickMembers)]
        [Summary("Kicks specified user from the server.")]
        //Reason can not be null, but it is optional since otherwise peple would not understand why the command is not working (to show exeption).
        public async Task KickAsync(SocketGuildUser user, string reason = null)
        {
            if (string.IsNullOrEmpty(reason))
            {
                throw new Exception(message: _stringService["moderationnoreason"]);
            }

            await user.KickAsync(reason);
            _discordLogger.DiscordCommandLog(Context, LoggingEvent.UserKick,
                $"{Context.User.Id} ({Context.User.Username}#{Context.User.Discriminator}) " +
                $"kicked {user.Id} ({user.Username}#{user.Discriminator}) for {reason}");

            await Context.Channel.TriggerTypingAsync();
            var embed = new DefaultEmbedBuilder()
                .WithTitle($"{Context.User.Username}#{Context.User.Discriminator} {_stringService["moderationkicked"]}")
                .AddField($"{user.Username}#{user.Discriminator} {_stringService["moderationuserwaskicked"]}", $"***{reason}***")
                .Build();

            await ReplyAsync(embed: embed);
        }

        [Command("ban")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.Administrator)]
        [Summary("Adds specified user to banned list and deletes past messages (optionally).")]
        public async Task BanAsync(IGuildUser user = null, string reason = null, int pruneDays = 0)
        {
            if (string.IsNullOrEmpty(reason))
            {
                reason = "No reason specified.";
            }

            await Context.Guild.AddBanAsync(user, pruneDays, reason);
            _discordLogger.DiscordCommandLog(Context, LoggingEvent.UserBan,
                $"{Context.User.Id} ({Context.User.Username}#{Context.User.Discriminator}) " +
                $"banned {user.Id} ({user.Username}#{user.Discriminator}) for {reason}");

            await Context.Channel.TriggerTypingAsync();

            var embed = new DefaultEmbedBuilder()
                .WithTitle($"{Context.User.Username}#{Context.User.Discriminator} {_stringService["moderationbanned"]}")
                .AddField($"{user.Username}#{user.Discriminator} {_stringService["moderationuserwasbanned"]}", $"***{reason}***")
                .Build();

            await ReplyAsync(embed: embed);
        }

        [Command("unban")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.Administrator)]
        [Summary("Removes specified user to banned list.")]
        public async Task UnbanAsync(string user)
        {

            bool success = ulong.TryParse(user, out ulong userId);

            if (!success)
            {
                throw new Exception(message: _stringService["moderationuseridwrong"]);
            }

            if (await Context.Guild.GetBanAsync(userId) == null)
            {
                throw new Exception(message: _stringService["moderationusernotbanned"]);
            }

            await Context.Guild.RemoveBanAsync(userId);
            _discordLogger.DiscordCommandLog(Context, LoggingEvent.UserUnban,
                $"{Context.User.Id} ({Context.User.Username}#{Context.User.Discriminator}) " +
                $"unbanned {userId})");

            var embed = new DefaultEmbedBuilder()
                .WithDescription($"{Context.User.Username}#{Context.User.Discriminator} {_stringService["moderationunbanned"]} {userId}.")
                .Build();

            await ReplyAsync(embed: embed);
        }
    }
}
