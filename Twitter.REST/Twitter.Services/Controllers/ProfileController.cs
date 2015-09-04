namespace Twitter.Services.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using Microsoft.AspNet.Identity;
    using Models.Tweets;
    using Models.Users;
    using Utils;

    [Authorize]
    public class ProfileController : BaseApiController
    {
        [HttpGet]
        [Route("api/profile/home")]
        public IHttpActionResult GetHomePage()
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.TwitterData.Users.Find(loggedUserId);
            if (loggedUser == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            var candidateTweets = this.TwitterData.Tweets.All()
                .Where(p => p.Author.Followers.Any(fr => fr.Id == loggedUserId) ||
                            p.WallOwner.Followers.Any(fr => fr.Id == loggedUserId))
                .OrderByDescending(p => p.PostedOn);

            var homeTweets = candidateTweets
                //.Take(1)
                .Select(TweetViewModel.Create);

            return this.Ok(homeTweets);
        }

        [HttpGet]
        [Route("api/profile/followers")]
        public IHttpActionResult Followers()
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.TwitterData.Users.Find(loggedUserId);
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
            var loggedUser = this.TwitterData.Users.Find(loggedUserId);
            if (loggedUser == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            var following = loggedUser.FollowedFriends.AsQueryable().Select(FollowersViewModel.Create);

            return this.Ok(following);
        }

        [HttpPut]
        [Route("api/profile/edit")]
        public IHttpActionResult EditProfile(EditUserBindingModel model)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.TwitterData.Users.Find(loggedUserId);
            if (loggedUser == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            if (model == null)
            {
                return this.BadRequest("Model is empty");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var emailHolder = this.TwitterData.Users.All()
                .FirstOrDefault(u => u.Email == model.Email);

            if (emailHolder != null && emailHolder.Id != loggedUserId)
            {
                return this.BadRequest("Email is already taken.");
            }

            if (!this.ValidateImageSize(model.ProfileImageData, ProfileImageKilobyteLimit))
            {
                return this.BadRequest(string.Format("Profile image size should be less than {0}kb.", ProfileImageKilobyteLimit));
            }

            if (!this.ValidateImageSize(model.CoverImageData, CoverImageKilobyteLimit))
            {
                return this.BadRequest(string.Format("Cover image size should be less than {0}kb.", CoverImageKilobyteLimit));
            }

            loggedUser.Fullname = model.Fullname;
            loggedUser.Email = model.Email;
            loggedUser.Gender = model.Gender;

            if (model.ProfileImageData != null && model.ProfileImageData.IndexOf(',') == -1)
            {
                model.ProfileImageData = string.Format("{0}{1}", "data:image/jpg;base64,", model.ProfileImageData);
            }

            loggedUser.ProfileImageData = model.ProfileImageData;

            try
            {
                string source = model.ProfileImageData;
                if (source != null)
                {
                    string base64 = source.Substring(source.IndexOf(',') + 1);
                    loggedUser.ProfileImageDataMinified = string.Format("{0}{1}",
                        "data:image/jpg;base64,", ImageUtility.Resize(base64, 100, 100));
                }
                else
                {
                    loggedUser.ProfileImageDataMinified = null;
                }
            }
            catch (FormatException)
            {
                return this.BadRequest("Invalid Base64 string format.");
            }

            if (model.CoverImageData != null && model.CoverImageData.IndexOf(',') == -1)
            {
                model.CoverImageData = string.Format("{0}{1}", "data:image/jpg;base64,", model.CoverImageData);
            }

            loggedUser.CoverImageData = model.CoverImageData;

            this.TwitterData.SaveChanges();

            return this.Ok(new
            {
                message = "User profile edited successfully."
            });
        }
    }
}