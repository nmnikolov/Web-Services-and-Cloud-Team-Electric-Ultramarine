﻿namespace Twitter.Services.Models.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Replies;
    using Tweets;
    using Twitter.Models;

    public class WallOwnerViewModel
    {
        public string Username { get; set; }

        public string Location { get; set; }

        public int TweetsCount { get; set; }

        public int FollowersCount { get; set; }

        public int FollowingCount { get; set; }

        public int FavoritesCount { get; set; }

        public IEnumerable<TweetViewModel> WallTweets { get; set; }

        public static Expression<Func<ApplicationUser, WallOwnerViewModel>> Create
        {
            get
            {
                return u => new WallOwnerViewModel()
                {
                    Username = u.UserName,
                    //Location = u.Location,
                    TweetsCount = u.WallTweets.Count,
                    FollowersCount = u.Followers.Count,
                    FollowingCount = u.FollowedFriends.Count,
                    FavoritesCount = (u.WallTweets.Sum(wp => wp.Favorites.Count)),
                    WallTweets = u.WallTweets.Select(p => new TweetViewModel()
                    {
                        Id = p.Id,
                        Content = p.Content,
                        Author = new UserViewModel()
                        {
                            Username = p.Author.UserName
                        },
                        PostedOn = p.PostedOn,
                        RepliesCount = p.Replies.Count(),
                        FavoritesCount = p.Favorites.Count(),
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
                    })
                };
            }
        }
    }
}