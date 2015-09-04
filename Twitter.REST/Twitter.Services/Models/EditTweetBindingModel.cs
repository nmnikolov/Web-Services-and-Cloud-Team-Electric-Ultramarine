using System.ComponentModel.DataAnnotations;

namespace Twitter.Services.Models
{
    public class EditTweetBindingModel
    {
        [Required]
        [MinLength(5)]
        public string Content { get; set; }
    }
}