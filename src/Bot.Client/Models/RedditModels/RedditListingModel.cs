using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bot.Client.Models.RedditModels
{
    public class RedditListingModel
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }
        [JsonProperty("data")]
        public RedditListingData Data { get; set; } = new RedditListingData();

    }

    public class RedditListingData
    {
        [JsonProperty("dist")]
        public int Count { get; set; }
        [JsonProperty("children")]
        public List<RedditPostModel> Children { get; set; } = new List<RedditPostModel>();
    }
}