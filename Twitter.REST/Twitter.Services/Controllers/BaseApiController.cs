namespace Twitter.Services.Controllers
{
    using System.Web.Http;
    using Data;
    using Data.Contracts;

    public abstract class BaseApiController : ApiController
    {
        protected const int ProfileImageKilobyteLimit = 128;
        protected const int CoverImageKilobyteLimit = 1024;   

        protected BaseApiController()
            : this(new TwitterData())
        {
        }

        protected BaseApiController(ITwitterData data)
        {
            this.TwitterData = data;
        }

        protected ITwitterData TwitterData { get; set; }

        protected bool ValidateImageSize(string imageDataUrl, int kilobyteLimit)
        {
            // Image delete
            if (imageDataUrl == null)
            {
                return true;
            }

            // Every 4 bytes from Base64 is equal to 3 bytes
            if ((imageDataUrl.Length / 4) * 3 >= kilobyteLimit * 1024)
            {
                return false;
            }

            return true;
        }
    }
}