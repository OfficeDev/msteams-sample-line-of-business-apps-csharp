using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace TaskModule
{
    public class TaskInfo
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("height")]
        public object Height { get; set; }

        [JsonProperty("width")]
        public object Width { get; set; }

        [JsonProperty("fallbackUrl")]
        public string FallbackUrl { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("card")]
        public Attachment Card { get; set; }

        [JsonProperty("completionBotId")]
        public string CompletionBotId { get; set; }

    }

    public class TaskEnvelope
    {
        [JsonProperty("task")]
        public Task Task { get; set; }
    }

    public class Task
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public TaskInfo TaskInfo { get; set; }
    }

    public enum TaskType
    {
        /// <summary>
        /// Teams will display the value of value in a popup message box.
        /// </summary>
        [EnumMember(Value = "message")]
        Message,

        /// <summary>
        /// Allows you to "chain" sequences of Adaptive cards together in a wizard/multi-step experience.
        /// </summary>
        [EnumMember(Value = "continue")]
        Continue
    }
}