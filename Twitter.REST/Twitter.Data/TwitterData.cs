namespace Twitter.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using Contracts;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;

    public class TwitterData : ITwitterData
    {
        private readonly DbContext context;

        private readonly IDictionary<Type, object> repositories;

        public TwitterData()
            : this(new TwitterContext())
        {
        }

        public TwitterData(DbContext context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }

        public IRepository<ApplicationUser> Users
        {
            get
            {
                return this.GetRepository<ApplicationUser>();
            }
        }

        public IRepository<IdentityRole> UserRoles
        {
            get
            {
                return this.GetRepository<IdentityRole>();
            }
        }

        public IRepository<Tweet> Tweets
        {
            get
            {
                return this.GetRepository<Tweet>();
            }
        }

        public IRepository<Reply> Replies
        {
            get
            {
                return this.GetRepository<Reply>();
            }
        }

        public IRepository<TweetFavorite> TweetsFavorites
        {
            get
            {
                return this.GetRepository<TweetFavorite>();
            }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(EfRepository<T>);
                this.repositories.Add(typeof(T), Activator.CreateInstance(type, this.context));
            }

            return (IRepository<T>)this.repositories[typeof(T)];
        }
    }
}