namespace Twitter.Models
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;


    public class ApplicationUser : IdentityUser
    {
        private ICollection<Tweet> ownTweets;
        private ICollection<Tweet> wallTweets;
        private ICollection<ApplicationUser> followers;
        private ICollection<ApplicationUser> followedFriends;
        private ICollection<TweetFavorite> favoritesTweets;

        public ApplicationUser()
        {
            this.ownTweets = new HashSet<Tweet>();
            this.wallTweets = new HashSet<Tweet>();
            this.followers = new HashSet<ApplicationUser>();
            this.followedFriends = new HashSet<ApplicationUser>();
            this.favoritesTweets = new HashSet<TweetFavorite>();
        }

        public string Fullname { get; set; }

        public Gender Gender { get; set; }

        public string ProfileImageData { get; set; }

        public string ProfileImageDataMinified { get; set; }

        public string CoverImageData { get; set; }

        public virtual ICollection<Tweet> OwnTweets
        {
            get { return this.ownTweets; }
            set { this.ownTweets = value; }
        }

        public virtual ICollection<TweetFavorite> FavoritesTweets
        {
            get { return this.favoritesTweets; }
            set { this.favoritesTweets = value; }
        }

        public virtual ICollection<Tweet> WallTweets
        {
            get { return this.wallTweets; }
            set { this.wallTweets = value; }
        }
        public virtual ICollection<ApplicationUser> Followers
        {
            get { return this.followers; }
            set { this.followers = value; }
        }
        public virtual ICollection<ApplicationUser> FollowedFriends
        {
            get { return this.followedFriends; }
            set { this.followedFriends = value; }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(
            UserManager<ApplicationUser> manager,
            string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(
                this,
                authenticationType);

            return userIdentity;
        }
    }
}