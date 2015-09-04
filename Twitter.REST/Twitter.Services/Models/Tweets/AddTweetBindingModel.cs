namespace Twitter.Services.Models.Tweets
{
    using System.ComponentModel.DataAnnotations;

    public class AddTweetBindingModel
    {
        [Required]
        [MinLength(5)]
        public string Content { get; set; }

        [Required]
        public string WallOwnerUsername { get; set; }
    }
}