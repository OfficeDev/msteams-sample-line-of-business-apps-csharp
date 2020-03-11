using Newtonsoft.Json;
using System.Collections.Generic;

namespace CrossVertical.Announcement.Models
{
    public class Group : DatabaseItem
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = nameof(Group);

        public string Name { get; set; }

        public List<string> Users { get; set; }
    }
}