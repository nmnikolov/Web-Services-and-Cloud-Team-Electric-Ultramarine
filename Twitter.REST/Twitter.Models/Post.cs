using System.ComponentModel.DataAnnotations;

namespace Twitter.Models
{
    using System;
    using System.Collections.Generic;

    public class Post
    {
        private ICollection<Reply> replies;
        private ICollection<PostFavorite> favorites;

        public Post()
        {
            this.replies = new HashSet<Reply>();
            this.favorites = new HashSet<PostFavorite>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        public string Content { get; set; }

        public DateTime PostedOn { get; set; }

        public string AuthorId { get; set; }

        public ApplicationUser Author { get; set; }
        
        public string WallOwnerId { get; set; }

        public virtual ApplicationUser WallOwner { get; set; }

        public virtual ICollection<Reply> Replies
        {
            get { return this.replies; }
            set { this.replies = value; }
        }
        public virtual ICollection<PostFavorite> Favorites
        {
            get { return this.favorites; }
            set { this.favorites = value; }
        }

    }
}