using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Twitter.Services.Models
{
    public class ReplyViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public UserViewModel Author { get; set; }
    }
}