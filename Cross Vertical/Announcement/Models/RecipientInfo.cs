using System.Collections.Generic;

namespace CrossVertical.Announcement.Models
{
    public class RecipientInfo
    {
        public string TenantId { get; set; }

        public string ServiceUrl { get; set; }

        public List<GroupRecipient> Groups { get; set; } = new List<Models.GroupRecipient>();

        public List<ChannelRecipient> Channels { get; set; } = new List<Models.ChannelRecipient>();
    }
}