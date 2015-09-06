namespace Twitter.Services.Models.Users
{
    using System.Linq;
    using Twitter.Models;

    public class ProfileDataViewModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Fullname { get; set; }

        public string Email { get; set; }

        public Gender Gender { get; set; }

        public string ProfileImageData { get; set; } 

        public string CoverImageData { get; set; }

        public int TweetsCount { get; set; }

        public int FollowingUsersCount { get; set; }

        public int FollowersCount { get; set; }

        public int FavoritesCount { get; set; }

        public bool IsFollowing { get; set; }

//        public static Expression<Func<ApplicationUser, ProfileDataViewModel>> Create {
//            get { 
//                return u => new ProfileDataViewModel
//                {
//                    UserName = u.UserName,
//                    Fullname = u.Fullname,
//                    Email = u.Email,
//                    Gender = u.Gender,
//                    ProfileImageData = u.ProfileImageData,
//                    CoverImageData = u.CoverImageData,
//                    IsFollowing = false
//                }; 
//            }
//        }

        public static ProfileDataViewModel Create(ApplicationUser targetUser, ApplicationUser loggedUser)
        {
            return new ProfileDataViewModel
                {
                    Id = targetUser.Id,
                    Username = targetUser.UserName,
                    Fullname = targetUser.Fullname,
                    Email = targetUser.Email,
                    Gender = targetUser.Gender,
                    ProfileImageData = targetUser.ProfileImageData,
                    CoverImageData = targetUser.CoverImageData,
                    TweetsCount = targetUser.OwnTweets.Count,
                    FollowingUsersCount = targetUser.FollowedFriends.Count,
                    FollowersCount = targetUser.Followers.Count,
                    FavoritesCount = targetUser.FavoritesTweets.Count,
                    IsFollowing = targetUser.Followers.Any(f => f.Id == loggedUser.Id)
                }; 
        }
    }
}