namespace Twitter.Services.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using Microsoft.AspNet.Identity;
    using Models;

    [Authorize]
    public class ProfileController : BaseApiController
    {
        [HttpGet]
        [Route("api/profile/home")]
        public IHttpActionResult GetHomePage()
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.Data.Users.Find(loggedUserId);
            if (loggedUser == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            var candidatePosts = this.Data.Posts
                .Where(p => p.Author.Followers.Any(fr => fr.Id == loggedUserId) ||
                    p.WallOwner.Followers.Any(fr => fr.Id == loggedUserId))
                .OrderByDescending(p => p.PostedOn)
                .AsEnumerable();

            var homePosts = candidatePosts
                //.Take(feedModel.PageSize)
                .Select(p => PostViewModel.Create);

            return this.Ok(homePosts);
        }

        [HttpGet]
        [Route("api/profile/followers")]
        public IHttpActionResult Followers()
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.Data.Users.Find(loggedUserId);
            if (loggedUser == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            var followers = loggedUser.Followers.AsQueryable().Select(FollowersViewModel.Create);

            return this.Ok(followers);
        }

        [HttpGet]
        [Route("api/profile/following")]
        public IHttpActionResult Following()
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.Data.Users.Find(loggedUserId);
            if (loggedUser == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            var following = loggedUser.FollowedFriends.AsQueryable().Select(FollowersViewModel.Create);

            return this.Ok(following);
        }
    }
}