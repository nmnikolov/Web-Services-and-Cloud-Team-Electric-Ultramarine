using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.OData;
using Microsoft.AspNet.Identity;
using Twitter.Services.Models;

namespace Twitter.Services.Controllers
{
    public class UsersController : BaseApiController
    {
        [Route("api/users/{username}/wall")]
        [EnableQuery]
        [ResponseType(typeof(IQueryable<AddPostBindingModel>))]
        public IHttpActionResult GetUserWall(string username)
        {
            var user = this.Data.Users
                .FirstOrDefault(u => u.UserName == username);
            if (user == null)
            {
                return this.BadRequest();
            }

            var userWall = this.Data.Users
                .Where(u => u.Id == user.Id)
                .Select(WallOwnerViewModel.Create);

            return this.Ok(userWall);
        }

        //[Authorize]
        //[HttpPost]
        //public IHttpActionResult FollowUser(string username, [FromBody]AddPostBindingModel model)
        //{
        //    var wallOwner = this.Data.Users.FirstOrDefault(u => u.UserName == username);
        //    if (wallOwner == null)
        //    {
        //        return this.BadRequest();
        //    }

        //    var wallOwnerWall = this.Data.Posts
        //        .Where(p => p.WallOwnerId == wallOwner.Id)
        //        .Select(PostViewModel.Create);
        //    if (model == null)
        //    {
        //        return this.BadRequest("Model cannot be null (no data in request)");
        //    }

        //    if (!this.ModelState.IsValid)
        //    {
        //        return this.BadRequest(this.ModelState);
        //    }

        //    string loggedUserId = this.User.Identity.GetUserId();
        //    var loggedUser = this.Data.Users.Find(loggedUserId);
        //    if (loggedUserId==wallOwner.Id)
        //    {
        //        return this.BadRequest("You cannot follow yourself");
        //    }

        //    if (wallOwner.Followers.Contains(loggedUser))
        //    {
        //        return this.BadRequest(string.Format(
        //            "You already follow {0}",model.WallOwnerUsername));
        //    }

        //    wallOwner.Followers.Add(loggedUser);
        //    loggedUser.FollowedFriends.Add(wallOwner);
        //    this.Data.SaveChanges();
        //    return this.Ok();
        //}

        [HttpPost]
        [Route("api/users/{username}/follow")]
        public IHttpActionResult FollowUser(string username)
        {
            var userId = this.User.Identity.GetUserId();
            if (userId == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            var user = this.Data.Users.Find(userId);

            var followedUser = this.Data.Users.FirstOrDefault(u => u.UserName == username);

            if (followedUser == null)
            {
                return this.NotFound();
            }

            if (followedUser.Id == userId)
            {
                return this.BadRequest("You cannot follow yourself");
            }

            if (followedUser.Followers.Contains(user))
            {
                return this.BadRequest("Already followed this user");
            }

            followedUser.Followers.Add(user);
            this.Data.SaveChanges();

            return this.Ok();
        }

        [HttpPost]
        [Route("api/users/{username}/unfollow")]
        public IHttpActionResult UnfollowUser(string username)
        {
            var userId = this.User.Identity.GetUserId();
            if (userId == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            var user = this.Data.Users.Find(userId);

            var followedUser = this.Data.Users.FirstOrDefault(u => u.UserName == username);

            if (followedUser == null)
            {
                return this.NotFound();
            }

            if (followedUser.Id == userId)
            {
                return this.BadRequest("You cannot unfollow yourself");
            }

            if (!followedUser.Followers.Contains(user))
            {
                return this.BadRequest("Not following this user.");
            }

            followedUser.Followers.Remove(user);
            this.Data.SaveChanges();

            return this.Ok();
        }

        // GET api/users/search?name=..
        [ActionName("search")]
        [HttpGet]
        public IHttpActionResult SearchUser([FromUri]UserSearchBindingModel model)
        {
            var usersSearchResult = this.Data.Users.AsQueryable();

            if (model.Name != null)
            {
                usersSearchResult = usersSearchResult.Where(u => u.UserName.Contains(model.Name));
            }

            var finalSearchResult = usersSearchResult
                .OrderBy(u => u.UserName)
                .Select(u => new
                {
                    u.UserName,
                    u.Location
                });

            return this.Ok(finalSearchResult);
        }
    }
}
