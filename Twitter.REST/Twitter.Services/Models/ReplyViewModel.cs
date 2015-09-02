namespace Twitter.Services.Models
{
    using System;

    public class ReplyViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public UserViewModel Author { get; set; }
    }
}