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
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.Data.Users.Find(loggedUserId);
            if (loggedUser == null)
            {
                return this.BadRequest("Invalid session token.");
            }

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

            var reply = new Reply()
            {
                Content = model.Content,
                PostedOn = DateTime.Now,
                AuthorId = loggedUserId
            };

            post.Replies.Add(reply);
            this.Data.SaveChanges();

            return this.Ok();
        }
    }
}
