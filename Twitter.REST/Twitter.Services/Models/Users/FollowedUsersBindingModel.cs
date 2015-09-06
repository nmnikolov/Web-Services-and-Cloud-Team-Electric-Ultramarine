namespace Twitter.Services.Models.Users
{
    using System.ComponentModel.DataAnnotations;

    public class FollowedUsersBindingModel
    {
        public string StartUserId { get; set; }

        [Required]
        [Range(0, 20)]
        public int PageSize { get; set; }
    }
}