namespace Twitter.Services.Models.Users
{
    using System.Linq;
    using Twitter.Models;

    public class ProfileDataPreviewViewModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Fullname { get; set; }

        public Gender Gender { get; set; }

        public string ProfileImageData { get; set; }

        public bool IsFollowing { get; set; }

//        public static Expression<Func<ApplicationUser, ProfileDataPreviewViewModel>> Create
//        {
//            get {
//                return u => new ProfileDataPreviewViewModel
//                {
//                    Username = u.UserName,
//                    Fullname = u.Fullname,
//                    Gender = u.Gender,
//                    ProfileImageData = u.ProfileImageData,
//                }; 
//            }
//        }

        public static ProfileDataPreviewViewModel Create(ApplicationUser targetUser, ApplicationUser loggedUser)
        {
            return new ProfileDataPreviewViewModel
            {
                Id = targetUser.Id,
                Username = targetUser.UserName,
                Fullname = targetUser.Fullname,
                Gender = targetUser.Gender,
                ProfileImageData = targetUser.ProfileImageData,
                IsFollowing = targetUser.Followers.Any(f => f.Id == loggedUser.Id)
            };
        }
    }
}