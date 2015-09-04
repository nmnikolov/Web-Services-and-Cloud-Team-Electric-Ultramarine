namespace Twitter.Data.Contracts
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;

    public interface ITwitterData
    {
        IRepository<ApplicationUser> Users { get; }

        IRepository<IdentityRole> UserRoles { get; }

        IRepository<Post> Posts { get; }

        IRepository<Reply> Replies { get; }
        
        IRepository<PostFavorite> PostsFavorites { get; }

        int SaveChanges();
    }
}