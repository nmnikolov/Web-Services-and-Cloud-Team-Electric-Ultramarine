namespace Twitter.Services.Controllers
{
    using System.Web.Http;
    using Data;

    public abstract class BaseApiController : ApiController
    {
        protected BaseApiController()
            : this(new TwitterContext())
        {
        }

        protected BaseApiController(TwitterContext data)
        {
            this.Data = data;
        }

        protected TwitterContext Data { get; set; }
    }
}