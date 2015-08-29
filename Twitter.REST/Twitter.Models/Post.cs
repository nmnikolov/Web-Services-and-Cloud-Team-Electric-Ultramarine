namespace Twitter.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Post
    {
        public int Id { get; set; }
        
        public string Content { get; set; }

        public int OwnerId { get; set; }    

        public ApplicationUser Owner { get; set; }
    }
}
