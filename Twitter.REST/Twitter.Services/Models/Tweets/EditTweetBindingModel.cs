namespace Twitter.Services.Models.Tweets
{
    using System.ComponentModel.DataAnnotations;

    public class EditTweetBindingModel
    {
        [Required]
        [MinLength(5)]
        public string Content { get; set; }
    }
}