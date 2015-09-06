namespace Twitter.Services.Models.Tweets
{
    using System.ComponentModel.DataAnnotations;

    public class AddTweetBindingModel
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public string WallOwnerUsername { get; set; }
    }
}