using System;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Twitter.Models;
using Twitter.Services.Models;

namespace Twitter.Services.Controllers
{
      [Authorize]
      [RoutePrefix("api/posts")]
        public class PostsController : BaseApiController
        {
            // GET api/posts
            [AllowAnonymous]
            public IHttpActionResult GetAllPosts()
            {
                var data = this.Data.Posts
                    .OrderBy(p => p.PostedOn)
                    .Select(PostViewModel.Create);

                return this.Ok(data);
            }

            // POST api/posts/{username}
            [HttpPost]
            public IHttpActionResult AddPost([FromBody]AddPostBindingModel model)
            {
                var loggedUserId = this.User.Identity.GetUserId();
                var loggedUser = this.Data.Users.Find(loggedUserId);
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

                var wallOwner = this.Data.Users.FirstOrDefault(u => u.UserName == model.WallOwnerUsername);
                
                if (wallOwner == null)
                {
                    return this.BadRequest(string.Format(
                        "User {0} does not exist",
                        model.WallOwnerUsername));
                }

                var post = new Post()
                {
                    AuthorId = loggedUserId,
                    WallOwner = wallOwner,
                    Content = model.Content,
                    PostedOn = DateTime.UtcNow,
                };

                this.Data.Posts.Add(post);
                this.Data.SaveChanges();

                var data = this.Data.Posts
                    .Where(p => p.Id == post.Id)
                    .Select(PostViewModel.Create)
                    .FirstOrDefault();

                return this.Ok(data);
            }

            // PUT api/posts/{id}
            [HttpPut]
            [Route("{id}")]
            public IHttpActionResult EditPost(int id, [FromBody]EditPostBindingModel model)
            {
                var loggedUserId = this.User.Identity.GetUserId();
                var loggedUser = this.Data.Users.Find(loggedUserId);
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

                var post = this.Data.Posts.Find(id);
                if (post == null)
                {
                    return this.NotFound();
                }

                if (post.AuthorId != loggedUserId)
                {
                    return this.Unauthorized();
                }

                post.Content = model.Content;
                this.Data.SaveChanges();

                var data = this.Data.Posts
                    .Where(p => p.Id == post.Id)
                    .Select(PostViewModel.Create)
                    .FirstOrDefault();

                return this.Ok(data);
            }

            // DELETE api/posts/{id}
            [HttpDelete]
            [Route("{id}")]
            public IHttpActionResult DeletePost(int id)
            {
                var loggedUserId = this.User.Identity.GetUserId();
                var loggedUser = this.Data.Users.Find(loggedUserId);
                if (loggedUser == null)
                {
                    return this.BadRequest("Invalid session token.");
                }

                var post = this.Data.Posts.Find(id);
                if (post == null)
                {
                    return this.NotFound();
                }

                if (post.AuthorId != loggedUserId && post.WallOwnerId != loggedUserId)
                {
                    return this.Unauthorized();
                }

                this.Data.Posts.Remove(post);
                this.Data.SaveChanges();

                return this.Ok();
            }

            // POST api/posts/{id}/retweet
            [HttpPost]
            [Route("{id}/retweet")]
            public IHttpActionResult RetweetPost(int id)
            {
                var loggedUserId = this.User.Identity.GetUserId();
                var loggedUser = this.Data.Users.Find(loggedUserId);
                if (loggedUser == null)
                {
                    return this.BadRequest("Invalid session token.");
                }

                var post = this.Data.Posts.Find(id);
                if (post==null)
                {
                    return this.BadRequest("Post does not exist");
                }

                var wallOwner = this.Data.Users.FirstOrDefault(u => u.Id == post.WallOwnerId);
                var retweetedPost = new Post()
                {
                    AuthorId = post.AuthorId,
                    WallOwner = loggedUser,
                    Content = post.Content,
                    PostedOn = DateTime.UtcNow,
                };
                this.Data.Posts.Add(retweetedPost);
                this.Data.SaveChanges();

                var loggedUserWall = this.Data.Posts
                .Where(p => p.WallOwnerId == loggedUserId)
                .Select(PostViewModel.Create);

                return this.Ok(loggedUserWall);
            }
        }
    }