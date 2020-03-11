using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrossVertical.Announcement.Models
{
    public class NotificationSendStatus
    {
        public bool IsSuccessful { get; set; }
        public string FailureMessage { get; set; }
        public string MessageId { get; set; }
    }
}