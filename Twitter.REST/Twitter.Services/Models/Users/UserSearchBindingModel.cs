namespace Twitter.Services.Models.Users
{
    using System.ComponentModel.DataAnnotations;

    public class UserSearchBindingModel
    {
        [Required]
        public string Search { get; set; }
    }
}