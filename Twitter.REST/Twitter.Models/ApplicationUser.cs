using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Twitter.Models
{
    public class ApplicationUser : IdentityUser
    {
        private ICollection<Post> ownPosts;
        private ICollection<Post> wallPosts;
        private ICollection<ApplicationUser> followers;
        private ICollection<ApplicationUser> followedFriends;

        public ApplicationUser()
        {
            this.ownPosts = new HashSet<Post>();
            this.wallPosts = new HashSet<Post>();
            this.followers = new HashSet<ApplicationUser>();
            this.followedFriends = new HashSet<ApplicationUser>();
        }

        public string Fullname { get; set; }

        public string Location { get; set; }

        public int? Age { get; set; }

        public virtual ICollection<Post> OwnPosts
        {
            get { return this.ownPosts; }
            set { this.ownPosts = value; }
        }

        public virtual ICollection<Post> WallPosts
        {
            get { return this.wallPosts; }
            set { this.wallPosts = value; }
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