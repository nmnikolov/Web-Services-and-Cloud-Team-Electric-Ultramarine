namespace Twitter.Services.Models.Users
{
    using System;
    using System.Linq.Expressions;
    using Twitter.Models;

    public class ProfileDataViewModel
    {
        public string UserName { get; set; }

        public string Fullname { get; set; }

        public string Email { get; set; }

        public Gender Gender { get; set; }

        public string ProfileImageData { get; set; } 

        public string CoverImageData { get; set; }

        public static Expression<Func<ApplicationUser, ProfileDataViewModel>> Create {
            get { 
                return u => new ProfileDataViewModel
                {
                    UserName = u.UserName,
                    Fullname = u.Fullname,
                    Email = u.Email,
                    Gender = u.Gender,
                    ProfileImageData = u.ProfileImageData,
                    CoverImageData = u.CoverImageData
                }; 
            }
        }
    }
}