using Bot.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bot.Services.RedditAPIService
{
    public class RedditAPIService : IRedditAPIService
    {
        public async Task<RedditPostModel> GetRedditPostAsync(string subReddit, ResultMethod method, int number, TopOf topOf)
        {
            var client = new HttpClient();
            string result = await client.GetStringAsync($"https://reddit.com/r/{subReddit}/{method.ToString().ToLower()}.json?limit={number}&t={topOf.ToString().ToLower()}");

            RedditListingModel redditListing = new RedditListingModel();

            if (method == ResultMethod.Random)
            {
                JArray array = JArray.Parse(result);
                redditListing = JsonConvert.DeserializeObject<RedditListingModel>(array[0].ToString());
            }
            else
            {
                 redditListing = JsonConvert.DeserializeObject<RedditListingModel>(result);
            }

            if (redditListing == null || redditListing?.kind != "Listing" || number > redditListing.data?.children.Count)
            {
                throw new Exception(message: "Error while calling API.");
            }
            else
            {
                return redditListing.data.children[number - 1] ?? new RedditPostModel();
            }
        }
    }
}
