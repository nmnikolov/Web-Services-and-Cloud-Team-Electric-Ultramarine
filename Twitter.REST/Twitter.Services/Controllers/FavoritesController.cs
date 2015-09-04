namespace Twitter.Services.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using Microsoft.AspNet.Identity;
    using Twitter.Models;

    public class FavoritesController : BaseApiController
    {
        // POST api/posts/{postId}/favorites
        [Route("api/posts/{postId}/favorites")]
        [HttpPost]
        public IHttpActionResult FavoritePost(int postId)
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

            var isAlreadyLiked = post.Favorites
                .Any(pf => pf.UserId == loggedUserId);
            if (isAlreadyLiked)
            {
                return this.BadRequest("You have already favorited this post");
            }

            if (post.AuthorId == loggedUserId)
            {
                return this.BadRequest("Cannot favorite own posts");
            }

            post.Favorites.Add(new PostFavorite()
            {
                UserId = loggedUserId
            });

            this.TwitterData.SaveChanges();

            return this.Ok();
        }
    }
}
