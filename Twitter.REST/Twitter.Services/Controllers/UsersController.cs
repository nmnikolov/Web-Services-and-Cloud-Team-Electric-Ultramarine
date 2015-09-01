using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.OData;
using SocialNetwork.Services.Controllers;
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

            var userWall = this.Data.Posts
                .Where(p => p.WallOwnerId == user.Id)
                .Select(PostViewModel.Create);

            return this.Ok(userWall);
        }
    }
}
