using System.ComponentModel.DataAnnotations;

namespace Twitter.Models
{
    public class PostFavorite
    {
        public int Id { get; set; }

        public int PostId { get; set; }

        public virtual Post Post { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}