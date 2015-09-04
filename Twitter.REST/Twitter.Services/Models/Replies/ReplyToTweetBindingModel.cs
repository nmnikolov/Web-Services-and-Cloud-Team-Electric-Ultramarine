namespace Twitter.Services.Models.Replies
{
    using System.ComponentModel.DataAnnotations;

    public class ReplyToTweetBindingModel
    {
        [Required]
        [MinLength(5)]
        public string Content { get; set; }
    }
}