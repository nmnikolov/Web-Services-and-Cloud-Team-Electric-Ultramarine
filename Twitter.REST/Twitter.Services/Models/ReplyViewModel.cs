﻿using System.Linq.Expressions;
using Twitter.Models;

namespace Twitter.Services.Models
{
    using System;

    public class ReplyViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public UserViewModel Author { get; set; }

        public static Expression<Func<Reply, ReplyViewModel>> Create
        {
            get
            {
                return r => new ReplyViewModel
                {
                    Id = r.Id,
                    Content = r.Content,
                    //Author = new UserViewModel()
                    //{
                    //    Username = r.Author.UserName
                    //}
                };
            }
        }
    }
}