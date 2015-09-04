using System;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Twitter.Models;
using Twitter.Services.Models;

namespace Twitter.Services.Controllers
{
    [Authorize]
    public class RepliesController : BaseApiController
    {
        //GET api/tweets/replies
        [AllowAnonymous]
        [HttpGet]
        [Route("api/replies")]
        public IHttpActionResult GetAllReplies()
        {
            var replies = this.TwitterData.Replies.All()
                .Select(r => new ReplyViewModel()
                {
                    Id = r.Id,
                    Content = r.Content
                });

            return this.Ok(replies);
        }

        // POST api/tweets/{tweetId}/replies
        [HttpPost]
        [Route("api/tweets/{tweetId}/replies")]
        public IHttpActionResult ReplyToTweet(int tweetId, ReplyToTweetBindingModel model)
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

            if (model == null)
            {
                return this.BadRequest("Empty model is not allowed");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var reply = new Reply()
            {
                Content = model.Content,
                PostedOn = DateTime.UtcNow,
                AuthorId = loggedUserId
            };

            tweet.Replies.Add(reply);
            this.TwitterData.SaveChanges();

            return this.Ok();
        }

        // PUT api/tweets/{tweetId}/replies/{replyId}
        [HttpPut]
        [Route("api/tweets/{tweetId}/replies/{replyId}")]
        public IHttpActionResult EditReplay(int tweetId, int replyId, ReplyToTweetBindingModel model)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.TwitterData.Users.Find(loggedUserId);
            if (loggedUser == null)
            {
                return this.Unauthorized();
            }

            var tweet = this.TwitterData.Tweets.All().FirstOrDefault(p => p.Id == tweetId);
            if (tweet == null)
            {
                return this.NotFound();
            }

            var reply = this.TwitterData.Replies.All().FirstOrDefault(r => r.Id == replyId);
            if (reply == null)
            {
                return this.NotFound();
            }

            if (reply.AuthorId != loggedUser.Id)
            {
                return this.Unauthorized();
            }

            if (model == null)
            {
                return this.BadRequest("Empty model is not allowed");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }


            reply.Content = model.Content;
            this.TwitterData.SaveChanges();

            var replyFromDb = this.TwitterData.Replies.All().Where(r => r.Id == replyId)
                .Select(r => new ReplyViewModel()
                {
                    Id = r.Id,
                    Content = r.Content,
                    Author = new UserViewModel()
                    {
                        Username = r.Author.UserName
                    }
                });

            return this.Ok(replyFromDb);
        }

        [HttpDelete]
        [Route("api/tweets/{tweetId}/replies/{replyId}")]
        public IHttpActionResult DeleteReplay(int tweetId, int replyId)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.TwitterData.Users.Find(loggedUserId);
            if (loggedUser == null)
            {
                return this.Unauthorized();
            }

            var tweet = this.TwitterData.Tweets.All().FirstOrDefault(p => p.Id == tweetId);
            if (tweet == null)
            {
                return this.NotFound();
            }

            var reply = this.TwitterData.Replies.All().FirstOrDefault(r => r.Id == replyId);
            if (reply == null)
            {
                return this.NotFound();
            }

            if (reply.AuthorId != loggedUser.Id && loggedUser.Id != tweet.WallOwnerId)
            {
                return this.Unauthorized();
            }

            var replyFromDb = this.TwitterData.Replies.All().FirstOrDefault(r => r.Id == replyId);
            this.TwitterData.Replies.Delete(replyFromDb);
            this.TwitterData.SaveChanges();

            return this.Ok("Reply is deleted");
        }
    }
}