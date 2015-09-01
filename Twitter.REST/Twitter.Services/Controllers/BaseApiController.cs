namespace SocialNetwork.Services.Controllers
{
    using System.Web.Http;
    using Twitter.Data;

    public class BaseApiController : ApiController
    {
        public BaseApiController()
            : this(new TwitterContext())
        {
        }

        public BaseApiController(TwitterContext data)
        {
            this.Data = data;
        }

        protected TwitterContext Data { get; set; }
    }
}
