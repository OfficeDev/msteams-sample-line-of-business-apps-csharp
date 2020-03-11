namespace CrossVertical.Announcement.Models
{
    public class ListCard
    {
        public Content content { get; set; }

        public string contentType { get; set; } = "application/vnd.microsoft.teams.card.list";
    }

    public class Content
    {
        public string title { get; set; }
        public Item[] items { get; set; }
        public Button[] buttons { get; set; }
    }

    public class Item
    {
        public string type { get; set; }
        public string title { get; set; }
        public string id { get; set; }
        public string subtitle { get; set; }
        public Tap tap { get; set; }
        public string icon { get; set; }
    }

    public class Tap
    {
        public string type { get; set; }
        public string title { get; set; }
        public string value { get; set; }
    }

    public class Button
    {
        public string type { get; set; }
        public string title { get; set; }
        public string value { get; set; }
    }
}