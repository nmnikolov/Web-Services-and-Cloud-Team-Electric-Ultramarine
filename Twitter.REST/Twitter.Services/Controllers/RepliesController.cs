using System;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Twitter.Models;
using Twitter.Services.Models;

namespace Twitter.Services.Controllers
{
    [Authorize]
    public class RepliesController : BaseApiController
    {
        // POST api/posts/{postId}/replies
        [HttpPost]
        [Route("api/posts/{postId}/replies")]
        public IHttpActionResult ReplyToPost(int postId, ReplyToPostBindingModel model)
        {
            var post = this.Data.Posts.Find(postId);
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

            var userId = this.User.Identity.GetUserId();

            if (userId == null)
            {
                return this.BadRequest("When you join Twitter, you can reply to anyone.");
            }

            var reply = new Reply()
            {
                Content = model.Content,
                PostedOn = DateTime.Now,
                AuthorId = userId
            };

            post.Replies.Add(reply);
            this.Data.SaveChanges();

            return this.Ok();
        }
    }
}
