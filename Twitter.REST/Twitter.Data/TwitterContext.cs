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
        public virtual IDbSet<Reply> Replies { get; set; }
        public virtual IDbSet<PostFavorite> PostFavorites { get; set; }

        public static TwitterContext Create()
        {
            return new TwitterContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Followers)
                .WithMany()
                .Map(m =>
                {
                    m.MapLeftKey("ApplicationUserId");
                    m.MapRightKey("FollowerId");
                    m.ToTable("UsersFriends");
                });

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.RetweetedPosts)
                .WithRequired(p => p.WallOwner)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.OwnPosts)
                .WithRequired(p => p.Author)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}