namespace Twitter.Services.Models
{
    using System.ComponentModel.DataAnnotations;

    public class UserSearchBindingModel
    {
        [Required]
        public string Search { get; set; }
    }
}