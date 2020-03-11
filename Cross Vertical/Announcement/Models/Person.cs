using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrossVertical.Announcement.Models
{
    public class ListItem
    {
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public bool EnableLikeButton = false;
        public string ChatUrl { get; set; }
        public string DeepLinkUrl { get; set; }
    }

    public class ListItemsViewModel
    {
        public List<ListItem> ListItems { get; set; }
        public string StartTab { get; set; }
    }
}