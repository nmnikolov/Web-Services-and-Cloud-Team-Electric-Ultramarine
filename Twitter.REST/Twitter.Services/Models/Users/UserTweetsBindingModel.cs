namespace Twitter.Services.Models.Users
{
    using System.ComponentModel.DataAnnotations;

    public class UserTweetsBindingModel
    {
        public int? StartPostId { get; set; }

        [Required]
        [Range(0, 10)]
        public int PageSize { get; set; }
    }
}