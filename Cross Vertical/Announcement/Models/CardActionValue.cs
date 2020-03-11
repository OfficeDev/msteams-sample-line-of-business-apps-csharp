using Newtonsoft.Json;

namespace TaskModule
{
    public class BotFrameworkCardValue<T>
    {
        [JsonProperty("type")]
        public object Type { get; set; } = "task/fetch";

        [JsonProperty("data")]
        public T Data { get; set; }
    }

    public class AdaptiveCardValue<T>
    {
        [JsonProperty("msteams")]
        public object Type { get; set; } = JsonConvert.DeserializeObject("{\"type\": \"task/fetch\" }");

        [JsonProperty("data")]
        public T Data { get; set; }
    }

    public class TaskModuleActionData<T>
    {
        [JsonProperty("data")]
        public BotFrameworkCardValue<T> Data { get; set; }
    }

}