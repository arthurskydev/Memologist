using Bot.Common.StringService;
using Bot.Models.RedditModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
                throw new Exception(message: _strinService["errorcllingapi"]);
            }
            else
            {
                return redditListing.data.children[number - 1] ?? new RedditPostModel();
            }
        }
    }
}
