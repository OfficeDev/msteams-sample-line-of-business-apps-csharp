namespace CrossVertical.Announcement.Models
{
    /// <summary>
    /// POCO for campaign.
    /// </summary>
    public partial class Campaign : AnnouncementBase
    {
        public override string Type { get; set; } = nameof(Campaign);

        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string ImageUrl { get; set; }
        public Author Author { get; set; } = new Author();
        public string Preview { get; set; }
        public string Body { get; set; }

        public bool IsAcknowledgementRequested { get; set; }
        public bool IsContactAllowed { get; set; }
        public bool ShowAllDetailsButton { get; set; }
        public MessageSensitivity Sensitivity { get; set; }
    }

}