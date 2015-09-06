namespace Twitter.Services.Models.Users
{
    using Twitter.Models;

    public class UserViewModel
    {
        public string Username { get; set; }

        public string Fullname { get; set; }

        public string ProfileImageData { get; set; }

        public bool IsFollowing { get; set; }

        public static UserViewModel Create(ApplicationUser targetUser, ApplicationUser loggedUser)
        {
            return new UserViewModel
            {
                
            };
        }
    }
}