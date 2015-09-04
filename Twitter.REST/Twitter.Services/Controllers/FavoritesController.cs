namespace Twitter.Services.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using Microsoft.AspNet.Identity;
    using Twitter.Models;

    public class FavoritesController : BaseApiController
    {
        // POST api/tweets/{tweetId}/favorites
        [Route("api/tweets/{tweetId}/favorites")]
        [HttpPost]
        public IHttpActionResult FavoriteTweet(int tweetId)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.TwitterData.Users.Find(loggedUserId);
            if (loggedUser == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            var tweet = this.TwitterData.Tweets.Find(tweetId);
            if (tweet == null)
            {
                return this.NotFound();
            }

            var isAlreadyLiked = tweet.Favorites
                .Any(pf => pf.UserId == loggedUserId);
            if (isAlreadyLiked)
            {
                return this.BadRequest("You have already favorited this tweet");
            }

            if (tweet.AuthorId == loggedUserId)
            {
                return this.BadRequest("Cannot favorite own tweets");
            }

            tweet.Favorites.Add(new TweetFavorite()
            {
                UserId = loggedUserId
            });

            this.TwitterData.SaveChanges();

            return this.Ok();
        }
    }
}
