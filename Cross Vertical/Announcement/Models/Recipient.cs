using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrossVertical.Announcement.Models
{
    public class RecipientDetails
    {
        public string MessageId { get; set; }
        public string Id { get; set; }
        public string FailureMessage { get; set; }
        public bool IsAcknoledged { get; set; }
        public int LikeCount { get; set; }
        
    }

    public class GroupRecipient
    {
        public string GroupId { get; set; }
        public List<RecipientDetails> Users { get; set; } = new List<RecipientDetails>();
    }

    public class ChannelRecipient
    {
        public string TeamId { get; set; }
        public RecipientDetails Channel { get; set; }
        public List<string> LikedUsers { get; set; } = new List<string>();
    }

}