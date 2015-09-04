using System;
using System.ComponentModel.DataAnnotations;

namespace Twitter.Models
{
    public class Reply
    {
        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        public string Content { get; set; }

        public DateTime PostedOn { get; set; }

        public int TweetId { get; set; }

        public virtual Tweet Tweet { get; set; }

        [Required]
        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }
    }
}

