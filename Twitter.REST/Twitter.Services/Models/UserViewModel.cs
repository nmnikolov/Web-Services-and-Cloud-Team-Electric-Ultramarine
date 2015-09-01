using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Twitter.Services.Models
{
    public class UserViewModel
    {
        public string Username { get; set; }

        public string Location { get; set; }

        public int TweetsCount { get; set; }

        public int FollowersCount { get; set; }

        public int FollowingCount { get; set; }
    }
}