using System.ComponentModel.DataAnnotations;

namespace Twitter.Models
{
    using System;
    using System.Collections.Generic;

    public class Tweet
    {
        private ICollection<Reply> replies;
        private ICollection<TweetFavorite> favorites;

        public Tweet()
        {
            this.replies = new HashSet<Reply>();
            this.favorites = new HashSet<TweetFavorite>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(5)]
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
        public virtual ICollection<TweetFavorite> Favorites
        {
            get { return this.favorites; }
            set { this.favorites = value; }
        }

    }
}