using System.Collections.Generic;

namespace CrossVertical.Announcement.ViewModels
{
    public class TabListViewModel
    {
        public bool SelectFirstTab { get; set; }
        public ListDetails FirstTab { get; set; } = new ListDetails();
        public ListDetails SecondTab { get; set; } = new ListDetails();
    }

    public class ListDetails
    {
        public List<Item> Items { get; set; } = new List<Item>();
        public string Title { get; set; }
        public string Type { get; set; }
        public string TenantId { get; set; }
    }

    public class Item
    {
        public string Id { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public bool EnableLikeButton = false;
        public string ChatUrl { get; set; }
        public string DeepLinkUrl { get; set; }
    }
}