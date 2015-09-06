namespace Twitter.Services.Models.Replies
{
    using System.ComponentModel.DataAnnotations;

    public class ReplyToTweetBindingModel
    {
        [Required]
        public string Content { get; set; }
    }
}