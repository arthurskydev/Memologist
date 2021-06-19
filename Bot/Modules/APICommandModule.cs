using Bot.Models;
using Bot.Services.RedditAPIService;
using Bot.Services.StringProcService;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Bot.Modules
{
    public class APICommandModule : ModuleBase<SocketCommandContext>
    {
        private protected IStringProcService _stringProcessor;
        private protected IConfiguration _configuration;
        private protected ILogger _logger;
        private protected IRedditAPIService _redditAPI;

        public APICommandModule(
            IStringProcService stringProcessor,
            IConfiguration configuration,
            ILogger<APICommandModule> logger,
            IRedditAPIService redditAPI)
        {
            _stringProcessor = stringProcessor;
            _configuration = configuration;
            _logger = logger;
            _redditAPI = redditAPI;
        }

        [Command("meme")]
        [Alias("m", "memes")]
        [Name("Meme")]
        [Summary("Sends back an image of a random meme or meme with specified tag.")]
        public async Task MemeAsync()
        {
            RedditPostModel post;

            string subreddit = _configuration.GetSection("CommandSettings").GetValue<string>("MemeSubreddit");
            if (String.IsNullOrEmpty(subreddit))
            {
                subreddit = "memes";
            }

            try
            {
                post = await _redditAPI.GetRedditPostAsync(subreddit, ResultMethod.Random);
                if (post == null || post?.kind != "t3")
                {
                    throw new Exception(message: "Post abnormal.");
                }
            }
            catch (Exception ex)
            {
                await ReplyAsync(_stringProcessor["nothingwasfoundreddit"]);
                _logger.LogTrace(ex.Message);
                return;
            }

            var builder = new EmbedBuilder()
                .WithImageUrl(post.data.url);
            var embed = builder.Build();

            await ReplyAsync(embed: embed);
        }
    }
}
