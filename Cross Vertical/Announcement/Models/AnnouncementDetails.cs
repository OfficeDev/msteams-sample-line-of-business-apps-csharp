using AdaptiveCards.Rendering.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrossVertical.Announcement.Models
{
    public class AnnouncementDetails
    {
        public int LikeCount { get; set; }
        public int AckCount { get; set; }
        public string Title { get; set; }
        public int ViewPercent { get; set; }
        public string RecipientsGroups { get; set; }
        public string RecipientChannels { get; set; }
        public HtmlTag html { get; set; }
        public static List<ReplyList> Replies = new List<ReplyList>()
        {
            new ReplyList(){ImagePath="~/",Name="MJ Price",Text="Looks great",Time=DateTime.Now},
            new ReplyList(){ImagePath="~/",Name="Marie Beaudoin",Text="Wonderful",Time=DateTime.Now}
        };
    }
    public class ReplyList
    {
        public string ImagePath { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }
    }
}