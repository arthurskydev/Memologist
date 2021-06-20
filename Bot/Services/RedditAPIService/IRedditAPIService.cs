using Bot.Models;
using System.Threading.Tasks;

namespace Bot.Services.RedditAPIService
{
    public enum ResultMethod
    {
        Random,
        Top,
        New,
        Hot
    }

    public enum TopOf
    {
        Day,
        Week,
        Month,
        Year,
        All
    }

    public interface IRedditAPIService
    {
        /// <summary>
        /// Gets a random post from specified subreddit.
        /// </summary>
        /// <param name="subReddit">String name of the subreddit (Without r/).</param>
        /// <param name="method">Method of sorting when getting post.</param>
        /// <param name="number">Which post should be picked from recieved list (to randomize top postst for example).</param>
        /// <returns>A RedditPostModel.</returns>
        Task<RedditPostModel> GetRedditPostAsync(string subReddit, ResultMethod method, int number = 1, TopOf topOf = TopOf.Day);
    }
}
