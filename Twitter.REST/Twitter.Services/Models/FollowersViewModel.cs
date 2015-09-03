namespace Twitter.Services.Models
{
    using System;
    using System.Linq.Expressions;
    using Twitter.Models;

    public class FollowersViewModel
    {
        public string Username { get; set; }

        public string Fullname { get; set; }

        public static Expression<Func<ApplicationUser, FollowersViewModel>> Create
        {
            get
            {
                return u => new FollowersViewModel
                {
                    Username = u.UserName,
                    Fullname = u.Fullname
                };
            }
        }
    }
}