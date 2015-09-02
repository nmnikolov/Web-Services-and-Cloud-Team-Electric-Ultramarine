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
            var userId = this.User.Identity.GetUserId();
            if (userId == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            //var user = this.Data.Users.Find(userId);

            var candidatePosts = this.Data.Posts
                .Where(p => p.Author.Followers.Any(fr => fr.Id == userId) ||
                    p.WallOwner.Followers.Any(fr => fr.Id == userId))
                .OrderByDescending(p => p.PostedOn)
                .AsEnumerable();

            var homePosts = candidatePosts
                //.Take(feedModel.PageSize)
                .Select(p => PostViewModel.Create);

            return this.Ok(homePosts);
        }
    }
}