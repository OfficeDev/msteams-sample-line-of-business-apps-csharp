using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrossVertical.Announcement.Models
{
    public class PostDetails
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public MessageSensitivity MessageSensitivity { get; set; }
        public int LikeCount { get; set; }
        public int AckCount { get; set; }
        public DateTime Date { get; set; }
        public Status Status { get; set; }
        public string Recipients { get; set; }
        public string RecipientChannelNames { get; set; }
        public string RecipientCount { get; set; }
        public int RecipientChannelCount { get; set; }

    }
}