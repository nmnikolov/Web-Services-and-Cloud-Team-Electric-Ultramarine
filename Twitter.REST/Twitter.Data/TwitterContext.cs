namespace Twitter.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Twitter.Models;
    using Migrations;

    public class TwitterContext : IdentityDbContext<ApplicationUser>
    {
        public TwitterContext()
            : base("TwitterContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<TwitterContext, Configuration>());
        }

        public virtual IDbSet<Post> Posts { get; set; }

        public static TwitterContext Create()
        {
            return new TwitterContext();
        }
    }
}