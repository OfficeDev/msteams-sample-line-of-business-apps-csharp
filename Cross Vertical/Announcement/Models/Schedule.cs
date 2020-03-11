using System;

namespace CrossVertical.Announcement.Models
{
    public class Schedule
    {
        public DateTimeOffset ScheduledTime  { get; set; }

        public string ScheduleId { get; set; }

        public DateTime GetScheduleTimeUTC()
        {
            return ScheduledTime.UtcDateTime;
        }
    }
}