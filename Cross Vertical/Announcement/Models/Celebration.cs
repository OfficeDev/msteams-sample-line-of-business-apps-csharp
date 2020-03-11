using AdaptiveCards;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CrossVertical.Announcement.Models
{
    /// <summary>
    /// POCO for celebration.
    /// </summary>
    public class Celebration : AnnouncementBase
    {
        public string Title { get; set; }
        public string CelebrationBannerImage { get; set; }
        public string Author { get; set; }
        public string Body { get; set; }

        public bool IsAcknowledgementRequested { get; set; }
        public MessageSensitivity Sensitivity { get; set; }
        public bool IsContactAllowed { get; set; }
        public override string Type { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override AdaptiveCard GetCreateNewCard(List<Group> groups, List<Team> teams, bool isEditCard)
        {
            throw new NotImplementedException();
        }

        public override AdaptiveCard GetPreviewCard()
        {
            throw new NotImplementedException();
        }
    }
}