using System.ComponentModel.DataAnnotations;

namespace Twitter.Models
{
    using System;
    using System.Collections.Generic;

    public class Tweet
    {
        private ICollection<Reply> replies;
        private ICollection<ApplicationUser> favorites;

        public Tweet()
        {
            this.replies = new HashSet<Reply>();
            this.favorites = new HashSet<ApplicationUser>();
        }

        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime PostedOn { get; set; }

        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }
        
        public string WallOwnerId { get; set; }

        public virtual ApplicationUser WallOwner { get; set; }

        public virtual ICollection<Reply> Replies
        {
            get { return this.replies; }
            set { this.replies = value; }
        }

        public virtual ICollection<ApplicationUser> Favorites
        {
            get { return this.favorites; }
            set { this.favorites = value; }
        }
    }
}