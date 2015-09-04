namespace Twitter.Data.Contracts
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;

    public interface ITwitterData
    {
        IRepository<ApplicationUser> Users { get; }

        IRepository<IdentityRole> UserRoles { get; }

        IRepository<Tweet> Tweets { get; }

        IRepository<Reply> Replies { get; }
        
        IRepository<TweetFavorite> TweetsFavorites { get; }

        int SaveChanges();
    }
}