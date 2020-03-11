using CrossVertical.Announcement.Helper;
using System.Web;

namespace CrossVertical.Announcement.Helpers
{
    public class Constants
    {

        public const string Configure = "Configure";
        public const string ShowAllDrafts = "ShowDrafts";
        public const string ShowAnnouncement = "ShowAnnouncementDraft";
        public const string Cancel = "Cancel";

        public const string CreateOrEditAnnouncement = "CreateOrEditAnnouncement";
        public const string EditAnnouncementFromTab = "EditAnnouncementFromTab";
        public const string ScheduleAnnouncement = "ScheduleAnnouncement";
        public const string ShowMoreDetails = "ShowMoreDetails";
        public const string SendAnnouncement = "SendAnnouncement";

        public const string ShowEditAnnouncementTaskModule = "ShowEditAnnouncementTaskModule";

        public const string Acknowledge = "Acknowledge";
        public const string ContactSender = "Contact Sender";
        public const string ShowWelcomeScreen = "ShowWelcomeScreen";

        public static string HistoryTabDeeplink { get; set; } =
    $"https://teams.microsoft.com/l/entity/{ApplicationSettings.AppId}/com.contoso.Announcement.history?webUrl={HttpUtility.UrlEncode(ApplicationSettings.BaseUrl + "/history?tid={tid}")}&label=History";

    }
}