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
        private ICollection<Post> retweetedPosts;
        private ICollection<ApplicationUser> followers;

        public ApplicationUser()
        {
            this.ownPosts = new HashSet<Post>();
            this.retweetedPosts = new HashSet<Post>();
            this.followers = new HashSet<ApplicationUser>();
        }

        public string Location { get; set; }

        public int? Age { get; set; }

        public virtual ICollection<Post> OwnPosts
        {
            get { return this.ownPosts; }
            set { this.ownPosts = value; }
        }

        public virtual ICollection<Post> RetweetedPosts
        {
            get { return this.retweetedPosts; }
            set { this.retweetedPosts = value; }
        }
        public virtual ICollection<ApplicationUser> Followers
        {
            get { return this.followers; }
            set { this.followers = value; }
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