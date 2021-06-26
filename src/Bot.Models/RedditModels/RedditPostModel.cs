namespace Bot.Models.RedditModels
{
    public class RedditPostModel
    {
        public string kind { get; set; }
        public RedditPostData data { get; set; } = new RedditPostData();
    }

    public class RedditPostData
    {
        public string subreddit { get; set; }
        public string title { get; set; }
        public bool hide_score { get; set; }
        public float upvote_ratio { get; set; }
        public int total_awards_received { get; set; }
        public bool is_original_content { get; set; }
        public object secure_media { get; set; }
        public int score { get; set; }
        public string thumbnail { get; set; }
        public bool edited { get; set; }
        public string post_hint { get; set; }
        public bool over_18 { get; set; }
        public bool spoiler { get; set; }
        public string subreddit_id { get; set; }
        public string id { get; set; }
        public string author { get; set; }
        public int num_comments { get; set; }
        public string permalink { get; set; }
        public string url { get; set; }
        public int subreddit_subscribers { get; set; }
        public float created_utc { get; set; }
        public int num_crossposts { get; set; }
        public object media { get; set; }
        public bool is_video { get; set; }
    }

}
