﻿namespace Twitter.Services.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Twitter.Models;

    public class PostViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public UserViewModel Author { get; set; }

        public DateTime PostedOn { get; set; }

        public int RepliesCount { get; set; }

        public int RetweetsCount { get; set; }

        public int FavoritesCount { get; set; }

        public IEnumerable<ReplyViewModel> Replies { get; set; }

        public static Expression<Func<Post, PostViewModel>> Create
        {
            get
            {
                return p => new PostViewModel
                {
                    Id = p.Id,
                    Content = p.Content,
                    Author = new UserViewModel
                    {
                        Username = p.Author.UserName
                    },
                    PostedOn = p.PostedOn,
                    RepliesCount = p.Replies.Count,
                    FavoritesCount = p.Favorites.Count,
                    Replies = p.Replies
                        .OrderBy(c => c.PostedOn)
                        .Take(3)
                        .Select(c => new ReplyViewModel
                        {
                            Id = c.Id,
                            Content = c.Content,
                            Author = new UserViewModel
                            {
                                Username = c.Author.UserName
                            }
                        })
                };
            }
        }
    }
}