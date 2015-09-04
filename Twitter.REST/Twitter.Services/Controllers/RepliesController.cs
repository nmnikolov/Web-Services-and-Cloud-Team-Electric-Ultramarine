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
        //GET api/posts/replies
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

        // POST api/posts/{postId}/replies
        [HttpPost]
        [Route("api/posts/{postId}/replies")]
        public IHttpActionResult ReplyToPost(int postId, ReplyToPostBindingModel model)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.TwitterData.Users.Find(loggedUserId);
            if (loggedUser == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            var post = this.TwitterData.Posts.Find(postId);
            if (post == null)
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

            post.Replies.Add(reply);
            this.TwitterData.SaveChanges();

            return this.Ok();
        }

        [HttpPut]
        [Route("api/posts/{postId}/replies/{replyId}")]
        public IHttpActionResult EditReplay(int postId, int replyId, ReplyToPostBindingModel model)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.TwitterData.Users.Find(loggedUserId);
            if (loggedUser == null)
            {
                return this.Unauthorized();
            }

            var post = this.TwitterData.Posts.All().FirstOrDefault(p => p.Id == postId);
            if (post == null)
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
        [Route("api/posts/{postId}/replies/{replyId}")]
        public IHttpActionResult DeleteReplay(int postId, int replyId, ReplyToPostBindingModel model)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.TwitterData.Users.Find(loggedUserId);
            if (loggedUser == null)
            {
                return this.Unauthorized();
            }

            var post = this.TwitterData.Posts.All().FirstOrDefault(p => p.Id == postId);
            if (post == null)
            {
                return this.NotFound();
            }

            var reply = this.TwitterData.Replies.All().FirstOrDefault(r => r.Id == replyId);
            if (reply == null)
            {
                return this.NotFound();
            }

            if (reply.AuthorId != loggedUser.Id && loggedUser.Id != post.WallOwnerId)
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

            var replyFromDb = this.TwitterData.Replies.All().FirstOrDefault(r => r.Id == replyId);
            this.TwitterData.Replies.Delete(replyFromDb);
            this.TwitterData.SaveChanges();

            return this.Ok("Reply is deleted");
        }
    }
}