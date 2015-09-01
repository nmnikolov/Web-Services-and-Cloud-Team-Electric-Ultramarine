using System.ComponentModel.DataAnnotations;

namespace Twitter.Services.Models
{
    public class EditPostBindingModel
    {
        [Required]
        [MinLength(5)]
        public string Content { get; set; }
    }
}