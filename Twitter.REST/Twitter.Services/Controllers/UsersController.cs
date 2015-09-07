namespace Twitter.Services.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using Microsoft.AspNet.Identity;
    using Models.Tweets;
    using Models.Users;

    [Authorize]
    public class UsersController : BaseApiController
    {
        [HttpGet]
        [Route("api/users/{username}/wall")]
        public IHttpActionResult GetUserWallTweets(string username, [FromUri] WallBindingModel model)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.TwitterData.Users.Find(loggedUserId);

            if (loggedUser == null)
            {
                return this.BadRequest("Invalid session token.");
            }

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
                .Select(t => TweetViewModel.CreateView(t, loggedUser));

            return this.Ok(pagePosts);
        }

        [HttpGet]
        [Route("api/users/{username}/tweets")]
        public IHttpActionResult GetUserTweets(string username, [FromUri] UserTweetsBindingModel model)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.TwitterData.Users.Find(loggedUserId);

            if (loggedUser == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            if (model == null)
            {
                return this.BadRequest();
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var targetUser = this.TwitterData.Users.All()
                .FirstOrDefault(u => u.UserName == username);

            if (targetUser == null)
            {
                return this.NotFound();
            }

            var candidateTweets = targetUser.OwnTweets
                .OrderByDescending(p => p.PostedOn)
                .AsQueryable();

            if (model.StartPostId.HasValue)
            {
                candidateTweets = candidateTweets
                    .AsQueryable()
                    .SkipWhile(p => p.Id != model.StartPostId)
                    .Skip(1)
                    .AsQueryable();
            }

            var userTweets = candidateTweets
                .Take(model.PageSize)
                .Select(t => TweetViewModel.CreateView(t, loggedUser));

            return this.Ok(userTweets);
        }

        [HttpGet]
        [Route("api/users/{username}/favorite")]
        public IHttpActionResult GetUserFavoriteTweets(string username, [FromUri] UserTweetsBindingModel model)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.TwitterData.Users.Find(loggedUserId);

            if (loggedUser == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            if (model == null)
            {
                return this.BadRequest();
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var targetUser = this.TwitterData.Users.All()
                .FirstOrDefault(u => u.UserName == username);

            if (targetUser == null)
            {
                return this.NotFound();
            }

            var candidateTweets = targetUser.FavoritesTweets
                .OrderByDescending(p => p.PostedOn)
                .AsQueryable();

            if (model.StartPostId.HasValue)
            {
                candidateTweets = candidateTweets
                    .AsQueryable()
                    .SkipWhile(p => p.Id != model.StartPostId)
                    .Skip(1)
                    .AsQueryable();
            }

            var userTweets = candidateTweets
                .Take(model.PageSize)
                .Select(t => TweetViewModel.CreateView(t, loggedUser));

            return this.Ok(userTweets);
        }

        [HttpGet]
        [Route("api/users/{username}/following")]
        public IHttpActionResult GetFollowingUsers(string username, [FromUri] FollowingUsersBindingModel model)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.TwitterData.Users.Find(loggedUserId);

            if (loggedUser == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            if (model == null)
            {
                return this.BadRequest();
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var targetUser = this.TwitterData.Users.All()
                .FirstOrDefault(u => u.UserName == username);

            if (targetUser == null)
            {
                return this.NotFound();
            }

            var candidateUsers = targetUser.FollowedFriends
                .OrderBy(u => u.Fullname)
                .AsQueryable();

            if (model.StartUserId != null)
            {
                candidateUsers = candidateUsers
                    .AsQueryable()
                    .SkipWhile(p => p.Id != model.StartUserId)
                    .Skip(1)
                    .AsQueryable();
            }

            var users = candidateUsers
                .Take(model.PageSize)
                .Select(u => ProfileDataPreviewViewModel.Create(u, loggedUser));

            return this.Ok(new
            {
                totalUsers = targetUser.FollowedFriends.Count,
                users
            });
        }

        [HttpGet]
        [Route("api/users/{username}/followed")]
        public IHttpActionResult GetFollowedUsers(string username, [FromUri] FollowedUsersBindingModel model)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.TwitterData.Users.Find(loggedUserId);

            if (loggedUser == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            if (model == null)
            {
                return this.BadRequest();
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var targetUser = this.TwitterData.Users.All()
                .FirstOrDefault(u => u.UserName == username);

            if (targetUser == null)
            {
                return this.NotFound();
            }

            var candidateUsers = targetUser.Followers
                .OrderBy(u => u.Fullname)
                .AsQueryable();

            if (model.StartUserId != null)
            {
                candidateUsers = candidateUsers
                    .AsQueryable()
                    .SkipWhile(p => p.Id != model.StartUserId)
                    .Skip(1)
                    .AsQueryable();
            }

            var users = candidateUsers
                .Take(model.PageSize)
                .Select(u => ProfileDataPreviewViewModel.Create(u, loggedUser));

            return this.Ok(new
            {
                totalUsers = targetUser.Followers.Count,
                users
            });
        }

        [HttpGet]
        [Route("api/users/{username}")]
        public IHttpActionResult GetUserFullInfo(string username)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.TwitterData.Users.Find(loggedUserId);

            if (loggedUser == null)
            {
                return this.BadRequest("Invalid session token.");
            }

//            var wallOwner = this.TwitterData.Users.All()
//                .Where(u => u.UserName == username)
//                .Select(ProfileDataViewModel.Create)
//                .FirstOrDefault();

            var targetUser = this.TwitterData.Users.All()
                .FirstOrDefault(u => u.UserName == username);

            if (targetUser == null)
            {
                return this.NotFound();
            }

            var targetUserInfo = ProfileDataViewModel.Create(targetUser, loggedUser);

            return this.Ok(targetUserInfo);
        }

        [HttpGet]
        [Route("api/users/{username}/preview")]
        public IHttpActionResult GetUserPreviewInfo(string username)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.TwitterData.Users.Find(loggedUserId);
            if (loggedUser == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            var targetUser = this.TwitterData.Users.All()
               .FirstOrDefault(u => u.UserName == username);

            if (targetUser == null)
            {
                return this.NotFound();
            }

            var targetUserInfo = ProfileDataPreviewViewModel.Create(targetUser, loggedUser);

            return this.Ok(targetUserInfo);
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
                .Where(u => u.Fullname.Contains(model.Search))
                .OrderBy(u => u.Fullname)
                .Take(5)
                .Select(u => new
                {
                    u.UserName,
                    u.Fullname,
                    u.ProfileImageData
                });

            return this.Ok(usersSearchResult);
        }
    }
}