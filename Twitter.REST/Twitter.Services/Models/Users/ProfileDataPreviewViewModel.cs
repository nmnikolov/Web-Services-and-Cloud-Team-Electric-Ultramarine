namespace Twitter.Services.Models.Users
{
    using System;
    using System.Linq.Expressions;
    using Twitter.Models;

    public class ProfileDataPreviewViewModel
    {
        public string Username { get; set; }

        public string Fullname { get; set; }

        public Gender Gender { get; set; }

        public string ProfileImageData { get; set; } 

        public static Expression<Func<ApplicationUser, ProfileDataPreviewViewModel>> Create
        {
            get {
                return u => new ProfileDataPreviewViewModel
                {
                    Username = u.UserName,
                    Fullname = u.Fullname,
                    Gender = u.Gender,
                    ProfileImageData = u.ProfileImageData,
                }; 
            }
        }
    }
}