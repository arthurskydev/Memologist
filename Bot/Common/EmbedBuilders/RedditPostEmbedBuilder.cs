using Bot.Models;
using Bot.Services.StringProcService;
using Discord;
using System;

namespace Bot.Common.EmbedBuilders
{
    /// <summary>
    /// Builds embed from a RedditPostModel with configurable information.
    /// </summary>
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
                string title = post.data.title;
                if (title.Length > 250)
                {
                    title = $"{title.Substring(0,249)}...";
                }
                WithTitle(title);
                WithUrl($"https://reddit.com{post.data.permalink}");
            }

            if (hasRating)
            {
                if (post.data.hide_score)
                {
                    WithFooter($"{_stringProcessor["speechbubbleemoji"]} {post.data.num_comments}  {_stringProcessor["scorehidden"]}");
                }
                else
                {
                    WithFooter($"{_stringProcessor["speechbubbleemoji"]} {post.data.num_comments}  {_stringProcessor["arrowupemoji"]} {post.data.score}");
                }
            }
        }
    }
}
