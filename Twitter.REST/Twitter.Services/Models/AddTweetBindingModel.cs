using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Twitter.Services.Models
{
    public class AddTweetBindingModel
    {
        [Required]
        [MinLength(5)]
        public string Content { get; set; }

        [Required]
        public string WallOwnerUsername { get; set; }
    }
}