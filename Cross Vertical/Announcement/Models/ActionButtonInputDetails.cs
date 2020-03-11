using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrossVertical.Announcement.Models
{
    public class ActionDetails
    {
        public string ActionType { get; set; }
    }

    public class AnnouncementActionDetails : ActionDetails
    {
        public string Id { get; set; }
    }

    public class ScheduleAnnouncementActionDetails : AnnouncementActionDetails
    {
        public string Date { get; set; }

        public string Time { get; set; }
    }

    public class AnnouncementAcknowledgeActionDetails : AnnouncementActionDetails
    {
        public string GroupId { get; set; }

        public string UserId { get; set; }
    }

    public class CreateNewAnnouncementData : AnnouncementActionDetails
    {
        public string Groups { get; set; }
        public string Channels { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string AuthorAlias { get; set; }
        public string Image { get; set; }
        public string Preview { get; set; }
        public string Body { get; set; }
        public string Acknowledge { get; set; }
        public string AllowContactIns { get; set; }
        public string MessageType { get; set; }
    }

}