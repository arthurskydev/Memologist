using BotModels.Reddit;
using BotCommon.StringService;
using Discord;
using System;

namespace BotDiscord.EmbedBuilders.EmbedBuilders
{
    /// <summary>
    /// Builds embed from a RedditPostModel with configurable information.
    /// </summary>
    public class RedditPostEmbedBuilder : EmbedBuilder
    {
        private protected IStringService _stringService;

        public RedditPostEmbedBuilder(
            RedditPostModel post,
            IStringService stringService,
            bool hasContext = false,
            bool hasRating = false)
        {
            _stringService = stringService;


            if (post.data.post_hint == "image")
            {
                WithImageUrl(post.data.url);
            }
            else
            {
                WithDescription(_stringService["redditnotanimage"]);
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
                    WithFooter($"{_stringService["speechbubbleemoji"]} {post.data.num_comments}  {_stringService["scorehidden"]}");
                }
                else
                {
                    WithFooter($"{_stringService["speechbubbleemoji"]} {post.data.num_comments}  {_stringService["arrowupemoji"]} {post.data.score}");
                }
            }
        }
    }
}
