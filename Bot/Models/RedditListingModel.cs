using System.Collections.Generic;

namespace Bot.Models
{
    public class RedditListingModel
    {
        public string kind { get; set; }
        public RedditListingData data { get; set; } = new RedditListingData();

    }

    public class RedditListingData
    {
        public int dist { get; set; }
        public List<RedditPostModel> children { get; set; } = new List<RedditPostModel>();
    }
}