using Newtonsoft.Json;
using System.Collections.Generic;

namespace CrossVertical.Announcement.Models
{
    public class Team : DatabaseItem
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = nameof(Team);

        public string Name { get; set; }

        public List<Channel> Channels { get; set; } = new List<Channel>();

        public int MemberCount { get; set; }

        public static string GetTeamId(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;
            if (!id.Contains("@"))
                return id;

            return id.Replace("19:", "").Replace("@thread.skype", "");
        }
    }
}