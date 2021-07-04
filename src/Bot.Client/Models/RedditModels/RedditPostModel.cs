using Newtonsoft.Json;

namespace Bot.Client.Models.RedditModels
{
    public class RedditPostModel
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }
        [JsonProperty("data")]
        public RedditPostData Data { get; set; } = new RedditPostData();
    }

    public class RedditPostData
    {
        [JsonProperty("subreddit")]
        public string Subreddit { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("hide_score")]
        public bool IsScoreHidden { get; set; }
        [JsonProperty("upvote_ratio")]
        public float UpvoteRatio { get; set; }
        [JsonProperty("total_awards_received")]
        public int TotalAwardsReceived { get; set; }
        [JsonProperty("is_original_content")]
        public bool IsOriginalContent { get; set; }
        [JsonProperty("score")]
        public int Score { get; set; }
        [JsonProperty("thumbnail")]
        public string ThumbnailURL { get; set; }
        [JsonProperty("edited")]
        public bool IsEdited { get; set; }
        [JsonProperty("post_hint")]
        public string PostHint { get; set; }
        [JsonProperty("over_18")]
        public bool IsOver18 { get; set; }
        [JsonProperty("spoiler")]
        public bool IsSpoiler { get; set; }
        [JsonProperty("subreddit_id")]
        public string SubredditId { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("author")]
        public string Author { get; set; }
        [JsonProperty("num_comments")]
        public int CommentCount { get; set; }
        [JsonProperty("permalink")]
        public string Permalink { get; set; }
        [JsonProperty("url")]
        public string URL { get; set; }
        [JsonProperty("subreddit_subscribers")]
        public int SubredditSubscribers { get; set; }
        [JsonProperty("created_utc")]
        public float CreatedUtc { get; set; }
        [JsonProperty("num_crossposts")]
        public int CountCrossposts { get; set; }
        [JsonProperty("is_video")]
        public bool IsVideo { get; set; }
    }

}
