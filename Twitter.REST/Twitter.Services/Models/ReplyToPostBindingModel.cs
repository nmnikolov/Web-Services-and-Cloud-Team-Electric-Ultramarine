namespace Twitter.Services.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ReplyToPostBindingModel
    {
        [Required]
        [MinLength(5)]
        public string Content { get; set; }
    }
}