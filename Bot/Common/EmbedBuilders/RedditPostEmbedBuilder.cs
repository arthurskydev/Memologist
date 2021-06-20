using Bot.Models;
using Bot.Services.StringProcService;
using Discord;

namespace Bot.Common.EmbedBuilders
{
    public class RedditPostEmbedBuilder : EmbedBuilder
    {
        private protected IStringProcService _stringProcessor;

        public RedditPostEmbedBuilder(
            RedditPostModel post,
            IStringProcService stringProcessor,
            bool hasContext = false,
            bool hasRating = false)
        {
            _stringProcessor = stringProcessor;

            if (post.data.post_hint == "image")
            {
                WithImageUrl(post.data.url);
            }
            else
            {
                WithDescription(_stringProcessor["redditnotanimage"]);
            }

            if (post.data.post_hint != "image" || hasContext)
            {
                WithTitle(post.data.title);
                WithUrl($"https://reddit.com{post.data.permalink}");
            }

            if (hasRating)
            {
                if (post.data.hide_score)
                {
                    WithFooter($"{_stringProcessor["scorehidden"]}  {_stringProcessor["speechbubbleemoji"]} {post.data.num_comments}");
                }
                else
                {
                    WithFooter($"{_stringProcessor["speechbubbleemoji"]} {post.data.num_comments}  {_stringProcessor["arrowupemoji"]} {post.data.score}");
                }
            }
        }
    }
}
