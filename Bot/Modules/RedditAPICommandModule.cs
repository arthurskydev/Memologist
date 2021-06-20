using Bot.Common.EmbedBuilders;
using Bot.Models;
using Bot.Services.RedditAPIService;
using Bot.Services.StringProcService;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Bot.Modules
{
    public class RedditAPICommandModule : ModuleBase<SocketCommandContext>
    {
        private protected IStringProcService _stringProcessor;
        private protected IConfiguration _configuration;
        private protected ILogger _logger;
        private protected IRedditAPIService _redditAPI;
        private protected Random _random;

        private string subreddit;

        public RedditAPICommandModule(
            IStringProcService stringProcessor,
            IConfiguration configuration,
            ILogger<RedditAPICommandModule> logger,
            IRedditAPIService redditAPI)
        {
            _stringProcessor = stringProcessor;
            _configuration = configuration;
            _logger = logger;
            _redditAPI = redditAPI;
            _random = new Random();

            subreddit = _configuration.GetSection("CommandSettings").GetValue<string>("MemeSubreddit");
            if (String.IsNullOrEmpty(subreddit))
            {
                subreddit = "memes";
            }

        }

        [Command("reddit")]
        [Alias("rget")]
        [Name("Reddit")]
        [Summary("Gets a meme from a subreddit with specified arguments.")]
        public async Task RedditAsync(string subreddit, ResultMethod method = ResultMethod.Random, int number = 1, TopOf topOf = TopOf.Day)
        {
            await Context.Channel.TriggerTypingAsync();

            RedditPostModel post;
            try
            {
                post = await _redditAPI.GetRedditPostAsync(subreddit, method, number, topOf);
            }
            catch (Exception ex)
            {
                await ReplyAsync(_stringProcessor["nothingwasfoundreddit"] +
                    "\n" +
                    ex.Message);

                _logger.LogTrace(ex.Message);
                return;
            }

            var builder = new RedditPostEmbedBuilder(post, _stringProcessor, true, true);
            var embed = builder.Build();

            await ReplyAsync(embed: embed);
        }

        [Command("meme")]
        [Alias("m", "memes")]
        [Name("Meme")]
        [Summary("Gives a randomg meme from the meme subreddit.")]
        public async Task MemeAsync()
        {
            await Context.Channel.TriggerTypingAsync();

            RedditPostModel post;
            try
            {
                post = await _redditAPI.GetRedditPostAsync(subreddit, ResultMethod.Random);
            }
            catch (Exception ex)
            {
                await ReplyAsync(_stringProcessor["nothingwasfoundreddit"] +
                    "\n" +
                    ex.Message);

                _logger.LogTrace(ex.Message);
                return;
            }

            var builder = new RedditPostEmbedBuilder(post, _stringProcessor);
            var embed = builder.Build();

            await ReplyAsync(embed: embed);
        }

        [Command("freshmeme")]
        [Alias("fm")]
        [Name("Fresh Meme")]
        [Summary("Gives a random meme from \"the top of today\" memes of the meme subreddit.")]
        public async Task FreshMemeAsync()
        {
            await Context.Channel.TriggerTypingAsync();

            RedditPostModel post;
            try
            {
                post = await _redditAPI.GetRedditPostAsync(subreddit, ResultMethod.Top, _random.Next(1, 100), TopOf.Day);
            }
            catch (Exception ex)
            {
                await ReplyAsync(_stringProcessor["nothingwasfoundreddit"] + 
                    "\n" + 
                    ex.Message);

                _logger.LogTrace(ex.Message);
                return;
            }

            var builder = new RedditPostEmbedBuilder(post, _stringProcessor);
            var embed = builder.Build();

            await ReplyAsync(embed: embed);
        }
    }
}
