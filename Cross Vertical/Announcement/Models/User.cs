using Newtonsoft.Json;

namespace CrossVertical.Announcement.Models
{
    public class User : DatabaseItem
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = nameof(User);

        public string Name { get; set; }
        public string BotConversationId { get; set; }
    }
}