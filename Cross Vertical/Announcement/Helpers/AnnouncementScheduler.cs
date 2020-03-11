using CrossVertical.Announcement.Repository;
using System;
using System.Threading.Tasks;

namespace CrossVertical.Announcement.Helpers
{
    public static class AnnouncementScheduler
    {
        public static async Task InitializeSchedulesFromDB()
        {
            Scheduler.InitializeScheduler(new TimeSpan(0, 30, 0));
            var tenats = await Cache.Tenants.GetAllItemsAsync();

            foreach (var tenant in tenats)
            {
                var tenatInfo = await Cache.Tenants.GetItemAsync(tenant.Id);
                foreach (var announcementId in tenatInfo.Announcements)
                {
                    var announcement = await Cache.Announcements.GetItemAsync(announcementId);
                    if (announcement != null &&
                        announcement.Schedule != null &&
                        announcement.Status == Models.Status.Scheduled)
                    {
                        // Schedule task.
                        announcement.Schedule.ScheduleId = Scheduler.AddSchedule(
                            announcement.Schedule.GetScheduleTimeUTC(),
                            new AnnouncementSender()
                            {
                                AnnouncementId = announcement.Id
                            }.Execute);

                        await Cache.Announcements.AddOrUpdateItemAsync(announcement.Id, announcement);
                    }
                }
            }
        }
    }
}