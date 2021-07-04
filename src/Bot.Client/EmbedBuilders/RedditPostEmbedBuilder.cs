using Bot.Common.Contract;
using Bot.Client.Models.RedditModels;
using Discord;

namespace Bot.Client.EmbedBuilders
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

            var data = post.Data;

            if (data.PostHint == "image")
            {
                WithImageUrl(data.URL);
            }
            else
            {
                WithDescription(_stringService["redditnotanimage"]);
            }

            if (data.PostHint != "image" || hasContext)
            {
                string title = data.Title;
                if (title.Length > 250)
                {
                    title = $"{title.Substring(0, 249)}...";
                }
                WithTitle(title);
                WithUrl($"https://reddit.com{data.Permalink}");
            }

            if (hasRating)
            {
                if (data.IsScoreHidden)
                {
                    WithFooter($"{_stringService["speechbubbleemoji"]} {data.CommentCount}  {_stringService["scorehidden"]}");
                }
                else
                {
                    WithFooter($"{_stringService["speechbubbleemoji"]} {data.CommentCount}  {_stringService["arrowupemoji"]} {data.Score}");
                }
            }
        }
    }
}
