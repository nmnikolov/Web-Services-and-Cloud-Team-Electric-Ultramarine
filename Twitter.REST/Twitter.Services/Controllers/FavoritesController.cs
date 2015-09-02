using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Twitter.Models;

namespace Twitter.Services.Controllers
{
    public class FavoritesController : BaseApiController
    {
        // POST api/posts/{postId}/favorites
        [Route("api/posts/{postId}/favorites")]
        [HttpPost]
        public IHttpActionResult FavoritePost(int postId)
        {
            var post = this.Data.Posts.Find(postId);
            if (post == null)
            {
                return this.NotFound();
            }

            var loggedUserId = this.User.Identity.GetUserId();

            if (loggedUserId==null)
            {
                return this.BadRequest("Join Twitter and favorite all the stuff you like.");
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

            this.Data.SaveChanges();

            return this.Ok();
        }
    }
}
