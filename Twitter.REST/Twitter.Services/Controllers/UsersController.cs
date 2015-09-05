namespace Twitter.Services.Controllers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;
    using System.Web.Http.OData;
    using Microsoft.AspNet.Identity;
    using Models.Tweets;
    using Models.Users;

    [Authorize]
    public class UsersController : BaseApiController
    {
        [Route("api/users/{username}/wall")]
        [EnableQuery]
        [AllowAnonymous]
        public IHttpActionResult GetUserTweets(string username, [FromUri] WallBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest();
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var wallOwner = this.TwitterData.Users.All()
                .FirstOrDefault(u => u.UserName == username);

            if (wallOwner == null)
            {
                return this.NotFound();
            }

            var candidatePosts = wallOwner.WallTweets
                .OrderByDescending(p => p.PostedOn)
                .AsQueryable();

            if (model.StartPostId.HasValue)
            {
                candidatePosts = candidatePosts
                    .AsQueryable()
                    .SkipWhile(p => p.Id != model.StartPostId)
                    .Skip(1)
                    .AsQueryable();
            }

            var pagePosts = candidatePosts
                .Take(model.PageSize)
                .Select(TweetViewModel.Create);

            return this.Ok(pagePosts);
        }

        [Route("api/users/{username}")]
        [EnableQuery]
        [AllowAnonymous]
        public IHttpActionResult GetUserFullInfo(string username)
        {
            var wallOwner = this.TwitterData.Users.All()
                .Where(u => u.UserName == username)
                .Select(ProfileDataViewModel.Create)
                .FirstOrDefault();

            if (wallOwner == null)
            {
                return this.NotFound();
            }

            return this.Ok(wallOwner);
        }

        [Route("api/users/{username}/preview")]
        [EnableQuery]
        [AllowAnonymous]
        public IHttpActionResult GetUserPreviewInfo(string username)
        {
            var wallOwner = this.TwitterData.Users.All()
                .Where(u => u.UserName == username)
                .Select(ProfileDataPreviewViewModel.Create)
                .FirstOrDefault();

            if (wallOwner == null)
            {
                return this.NotFound();
            }

            return this.Ok(wallOwner);
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

        //    var wallOwnerWall = this.Data.Tweets
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
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.TwitterData.Users.Find(loggedUserId);
            if (loggedUser == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            var followedUser = this.TwitterData.Users.All().FirstOrDefault(u => u.UserName == username);

            if (followedUser == null)
            {
                return this.NotFound();
            }

            if (followedUser.Id == loggedUserId)
            {
                return this.BadRequest("You cannot follow yourself");
            }

            if (followedUser.Followers.Contains(loggedUser))
            {
                return this.BadRequest("Already followed this user");
            }

            followedUser.Followers.Add(loggedUser);
            loggedUser.FollowedFriends.Add(followedUser);
            this.TwitterData.SaveChanges();

            return this.Ok();
        }

        [HttpPost]
        [Route("api/users/{username}/unfollow")]
        public IHttpActionResult UnfollowUser(string username)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.TwitterData.Users.Find(loggedUserId);
            if (loggedUser == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            var followedUser = this.TwitterData.Users.All().FirstOrDefault(u => u.UserName == username);

            if (followedUser == null)
            {
                return this.NotFound();
            }

            if (followedUser.Id == loggedUserId)
            {
                return this.BadRequest("You cannot unfollow yourself");
            }

            if (!followedUser.Followers.Contains(loggedUser))
            {
                return this.BadRequest("Not following this user.");
            }

            followedUser.Followers.Remove(loggedUser);
            loggedUser.FollowedFriends.Remove(followedUser);
            this.TwitterData.SaveChanges();

            return this.Ok();
        }

        // GET api/users?search=..
        [HttpGet]
        [Route("api/users/search")]
        [AllowAnonymous]
        public IHttpActionResult SearchUser([FromUri]UserSearchBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Model cannot be null (no data in request)");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var usersSearchResult = this.TwitterData.Users.All()
                .Where(u => u.UserName.Contains(model.Search) || u.Fullname.Contains(model.Search))
                .OrderBy(u => u.UserName)
                .Select(u => new
                {
                    u.UserName,
                    u.Fullname,
                    u.ProfileImageData
                    //u.Location
                });

            return this.Ok(usersSearchResult);
        }
    }
}