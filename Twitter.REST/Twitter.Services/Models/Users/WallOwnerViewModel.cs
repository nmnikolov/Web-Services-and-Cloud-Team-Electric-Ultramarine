namespace Twitter.Services.Models.Users
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

        public string Fullname { get; set; }

        public string ProfileImageData { get; set; }

        public string CoverImageData { get; set; }
        
        public int TweetsCount { get; set; }

        public int FollowersCount { get; set; }

        public int FollowingCount { get; set; }

        public int FavoritesCount { get; set; }

        public IEnumerable<TweetViewModel> WallTweets { get; set; }

        public static Expression<Func<ApplicationUser, WallOwnerViewModel>> Create
        {
            get
            {
                return u => new WallOwnerViewModel
                {
                    Username = u.UserName,
                    Fullname = u.Fullname,
                    ProfileImageData = u.ProfileImageData,
                    CoverImageData = u.CoverImageData,
                    TweetsCount = u.WallTweets.Count,
                    FollowersCount = u.Followers.Count,
                    FollowingCount = u.FollowedFriends.Count,
                    FavoritesCount = u.FavoritesTweets.Count,
                    WallTweets = u.WallTweets.OrderByDescending(t => t.PostedOn).Select(t => new TweetViewModel()
                    {
                        Id = t.Id,
                        Content = t.Content,
                        Author = new UserViewModel
                        {
                            Username = t.Author.UserName,
                            Fullname = t.Author.Fullname,
                            ProfileImageData = t.Author.ProfileImageData
                        },
                        PostedOn = t.PostedOn,
                        RepliesCount = t.Replies.Count,
                        FavoritesCount = t.Favorites.Count,
                        Replies = t.Replies
                            .OrderBy(c => c.PostedOn)
                            .Take(3)
                            .Select(c => new ReplyViewModel
                            {
                                Id = c.Id,
                                Content = c.Content,
                                Author = new UserViewModel
                                {
                                    Username = c.Author.UserName,
                                    Fullname = c.Author.Fullname,
                                    ProfileImageData = c.Author.ProfileImageData
                                }
                            })
                    })
                };
            }
        }
    }
}