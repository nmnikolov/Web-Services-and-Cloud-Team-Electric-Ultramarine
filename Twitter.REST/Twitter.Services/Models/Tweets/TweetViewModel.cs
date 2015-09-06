namespace Twitter.Services.Models.Tweets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Replies;
    using Twitter.Models;
    using Users;

    public class TweetViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public UserViewModel WallOwner { get; set; }

        public UserViewModel Author { get; set; }

        public DateTime PostedOn { get; set; }

        public int RepliesCount { get; set; }

        public int RetweetsCount { get; set; }

        public int FavoritesCount { get; set; }

        public bool IsFavorited { get; set; }

        public IEnumerable<ReplyViewModel> Replies { get; set; }

        public static Expression<Func<Tweet, TweetViewModel>> Create
        {
            get
            {
                return p => new TweetViewModel
                {
                    Id = p.Id,
                    Content = p.Content,
                    Author = new UserViewModel
                    {
                        Username = p.Author.UserName,
                        Fullname = p.Author.Fullname,
                        ProfileImageData = p.Author.ProfileImageData
                    },
                    WallOwner = new UserViewModel
                    {
                        Username = p.WallOwner.UserName,
                        Fullname = p.WallOwner.Fullname,
                        ProfileImageData = p.WallOwner.ProfileImageData
                    },
                    PostedOn = p.PostedOn,
                    RepliesCount = p.Replies.Count,
                    FavoritesCount = p.Favorites.Count,
                    Replies = p.Replies
                        .OrderByDescending(r => r.PostedOn)
                        .Take(3)
                        .Select(r => new ReplyViewModel
                        {
                            Id = r.Id,
                            Content = r.Content,
                            Author = new UserViewModel
                            {
                                Username = r.Author.UserName,
                                Fullname = r.Author.Fullname,
                                ProfileImageData = r.Author.ProfileImageData
                            },
                            PostedOn = r.PostedOn
                        })
                };
            }
        }

        public static TweetViewModel CreateView(Tweet t, ApplicationUser loggedUser)
        {
            return new TweetViewModel
            {
                Id = t.Id,
                Content = t.Content,
                Author = new UserViewModel
                {
                    Username = t.Author.UserName,
                    Fullname = t.Author.Fullname,
                    ProfileImageData = t.Author.ProfileImageData
                },
                WallOwner = new UserViewModel
                {
                    Username = t.WallOwner.UserName,
                    Fullname = t.WallOwner.Fullname,
                    ProfileImageData = t.WallOwner.ProfileImageData
                },
                PostedOn = t.PostedOn,
                RepliesCount = t.Replies.Count,
                FavoritesCount = t.Favorites.Count,
                IsFavorited = t.Favorites.Any(),
                Replies = t.Replies
                    .OrderByDescending(r => r.PostedOn)
                    .Take(3)
                    .Select(r => new ReplyViewModel
                    {
                        Id = r.Id,
                        Content = r.Content,
                        Author = new UserViewModel
                        {
                            Username = r.Author.UserName,
                            Fullname = r.Author.Fullname,
                            ProfileImageData = r.Author.ProfileImageData
                        },
                        PostedOn = r.PostedOn
                    })
            };
        }
    }
}