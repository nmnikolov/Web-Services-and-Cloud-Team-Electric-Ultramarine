namespace Twitter.Services.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Twitter.Models;


    public class WallOwnerViewModel
    {
        public string Username { get; set; }

        public string Location { get; set; }

        public int TweetsCount { get; set; }

        public int FollowersCount { get; set; }

        public int FollowingCount { get; set; }

        public IEnumerable<PostViewModel> OwnPosts { get; set; }

        public IEnumerable<PostViewModel> RetweetedPosts { get; set; }

        public static Expression<Func<ApplicationUser, WallOwnerViewModel>> Create
        {
            get
            {
                return u => new WallOwnerViewModel()
                {
                    Username = u.UserName,
                    Location = u.Location,
                    TweetsCount = u.OwnPosts.Count + u.RetweetedPosts.Count,
                    FollowersCount = u.Followers.Count,
                    FollowingCount = u.FollowedFriends.Count,
                    OwnPosts = u.OwnPosts.Select(p => new PostViewModel()
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
                    }),
                    RetweetedPosts = u.RetweetedPosts.Select(p => new PostViewModel()
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