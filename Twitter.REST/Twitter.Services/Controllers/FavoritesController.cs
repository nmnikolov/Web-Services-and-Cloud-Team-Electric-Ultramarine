namespace Twitter.Services.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using Microsoft.AspNet.Identity;
    using Twitter.Models;

    public class FavoritesController : BaseApiController
    {
        // POST api/tweets/{tweetId}/favorites
        [HttpPost]
        [Route("api/tweets/{tweetId}/favorites")]
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
                .Any(tf => tf.Id == loggedUserId);

            if (isAlreadyLiked)
            {
                return this.BadRequest("You have already favorited this tweet");
            }

            if (tweet.AuthorId == loggedUserId)
            {
                return this.BadRequest("Cannot favorite own tweets");
            }

            tweet.Favorites.Add(loggedUser);
            this.TwitterData.SaveChanges();

            return this.Ok();
        }

        // POST api/tweets/{tweetId}/favorites
        [HttpDelete]
        [Route("api/tweets/{tweetId}/favorites")]
        public IHttpActionResult RemoveFavoriteTweet(int tweetId)
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
                .Any(tf => tf.Id == loggedUserId);

            if (tweet.AuthorId == loggedUserId)
            {
                return this.BadRequest("Cannot favorite or remove favorite on own tweets");
            }

            if (!isAlreadyLiked)
            {
                return this.BadRequest("You have not favorited this tweet");
            }

            tweet.Favorites.Remove(loggedUser);
            this.TwitterData.SaveChanges();

            return this.Ok();
        }
    }
}
