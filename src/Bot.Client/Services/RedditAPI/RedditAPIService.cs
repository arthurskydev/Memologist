using Bot.Client.Models.RedditModels;
using Bot.Common.Contract;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bot.Client.Services.RedditAPI
{
    public class RedditAPIService : IRedditAPIService
    {
        private readonly IStringService _strinService;

        public RedditAPIService(IStringService stringService)
        {
            _strinService = stringService;
        }

        public async Task<RedditPostModel> GetRedditPostAsync(string subReddit, ResultMethod method, int number, TopOf topOf)
        {
            if (number > 100)
            {
                throw new Exception(message: _strinService["numbertoohighreddit"]);
            }

            var client = new HttpClient();
            string result = await client.GetStringAsync($"https://reddit.com/r/{subReddit}/{method.ToString().ToLower()}.json?limit={number}&t={topOf.ToString().ToLower()}");

            RedditListingModel redditListing;

            if (method == ResultMethod.Random)
            {
                JArray array = JArray.Parse(result);
                redditListing = JsonConvert.DeserializeObject<RedditListingModel>(array[0].ToString());
            }
            else
            {
                redditListing = JsonConvert.DeserializeObject<RedditListingModel>(result);
            }

            if (redditListing == null || redditListing?.Kind != "Listing" || number > redditListing.Data?.Children.Count)
            {
                throw new Exception(message: _strinService["errorcllingapi"]);
            }
            else
            {
                return redditListing.Data.Children[number - 1];
            }
        }
    }
}
