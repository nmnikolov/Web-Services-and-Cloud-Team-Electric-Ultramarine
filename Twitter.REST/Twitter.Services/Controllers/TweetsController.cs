namespace Twitter.Services.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using Microsoft.AspNet.Identity;
    using Twitter.Models;
    using Models.Tweets;

    [Authorize]
    [RoutePrefix("api/tweets")]
    public class TweetsController : BaseApiController
    {
        // GET api/tweets/{tweetId}
        [AllowAnonymous]
        [HttpGet]
        [Route("{tweetId}")]
        public IHttpActionResult GetTweetById(int tweetId)
        {
            var tweet = this.TwitterData.Tweets.All()
                .Where(t => t.Id == tweetId)
                .Select(TweetViewModel.Create)
                .FirstOrDefault();

            if (tweet == null)
            {
                return this.NotFound();
            }

            return this.Ok(tweet);
        }

        // POST api/tweets/{username}
        [HttpPost]
        [Route("")]
        public IHttpActionResult AddTweet([FromBody]AddTweetBindingModel model)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.TwitterData.Users.Find(loggedUserId);
            if (loggedUser == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            if (model == null)
            {
                return this.BadRequest("Model cannot be null (no data in request)");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var wallOwner = this.TwitterData.Users.All().FirstOrDefault(u => u.UserName == model.WallOwnerUsername);
                
            if (wallOwner == null)
            {
                return this.BadRequest(string.Format(
                    "User {0} does not exist",
                    model.WallOwnerUsername));
            }

            var tweet = new Tweet()
            {
                AuthorId = loggedUserId,
                WallOwnerId = wallOwner.Id,
                Content = model.Content,
                PostedOn = DateTime.UtcNow,
            };

            this.TwitterData.Tweets.Add(tweet);
            this.TwitterData.SaveChanges();

            var data = this.TwitterData.Tweets.All()
                .Where(p => p.Id == tweet.Id)
                .Select(TweetViewModel.Create)
                .FirstOrDefault();

            return this.Ok(data);
        }

        // PUT api/tweets/{id}
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult EditTweet(int id, [FromBody]EditTweetBindingModel model)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.TwitterData.Users.Find(loggedUserId);
            if (loggedUser == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            if (model == null)
            {
                return this.BadRequest("Model is empty");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var tweet = this.TwitterData.Tweets.Find(id);
            if (tweet == null)
            {
                return this.NotFound();
            }

            if (tweet.AuthorId != loggedUserId)
            {
                return this.Unauthorized();
            }

            tweet.Content = model.Content;
            this.TwitterData.SaveChanges();

            var data = this.TwitterData.Tweets.All()
                .Where(p => p.Id == tweet.Id)
                .Select(TweetViewModel.Create)
                .FirstOrDefault();

            return this.Ok(data);
        }

        // DELETE api/tweets/{id}
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteTweet(int id)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.TwitterData.Users.Find(loggedUserId);
            if (loggedUser == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            var tweet = this.TwitterData.Tweets.Find(id);
            if (tweet == null)
            {
                return this.NotFound();
            }

            if (tweet.AuthorId != loggedUserId && tweet.WallOwnerId != loggedUserId)
            {
                return this.Unauthorized();
            }
                
            this.TwitterData.Tweets.Delete(tweet);
            this.TwitterData.SaveChanges();

            return this.Ok();
        }

        // POST api/tweets/{id}/retweet
        [HttpPost]
        [Route("{id}/retweet")]
        public IHttpActionResult RetweetTweet(int id)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.TwitterData.Users.Find(loggedUserId);
            if (loggedUser == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            var tweet = this.TwitterData.Tweets.Find(id);
            if (tweet==null)
            {
                return this.BadRequest("Tweet does not exist");
            }

            var wallOwner = this.TwitterData.Users.All().FirstOrDefault(u => u.Id == tweet.WallOwnerId);
            var retweetedTweet = new Tweet()
            {
                AuthorId = tweet.AuthorId,
                WallOwner = loggedUser,
                Content = tweet.Content,
                PostedOn = DateTime.UtcNow,
            };
            this.TwitterData.Tweets.Add(retweetedTweet);
            this.TwitterData.SaveChanges();

            var loggedUserWall = this.TwitterData.Tweets.All()
            .Where(p => p.WallOwnerId == loggedUserId)
            .Select(TweetViewModel.Create);

            return this.Ok(loggedUserWall);
        }
    }
}