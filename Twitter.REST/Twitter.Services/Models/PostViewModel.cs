﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Twitter.Models;

namespace Twitter.Services.Models
{
    public class PostViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public UserViewModel Author { get; set; }

        public UserViewModel WallOwner { get; set; }

        public DateTime PostedOn { get; set; }

        public int RepliesCount { get; set; }

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
                    Author = new UserViewModel()
                    {
                        Username = p.Author.UserName
                    },
                    WallOwner = new UserViewModel()
                    {
                        Username = p.WallOwner.UserName,
                        FollowersCount = p.WallOwner.Followers.Count
                    },
                    PostedOn = p.PostedOn,
                    FavoritesCount = p.Favorites.Count(),
                    RepliesCount = p.Replies.Count(),
                    Replies = p.Replies
                        .OrderBy(c => c.PostedOn)
                        .Take(3)
                        .Select(c => new ReplyViewModel()
                        {
                            Id = c.Id,
                            Content = c.Content,
                            Author = new UserViewModel()
                            {
                                Username = c.Author.UserName
                            }
                        })
                };
            }
        }
    }
}