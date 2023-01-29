using Newtonsoft.Json;

namespace AskGPT
{
  public class OpenAIRequest
  {
    [JsonProperty(PropertyName = "model", Required=Required.Always)]
    public string Model { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "prompt", Required=Required.Always)]
    public string Prompt { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "temperature", NullValueHandling = NullValueHandling.Ignore)]
    public int? Temperature { get; set; }

    [JsonProperty(PropertyName = "max_tokens", NullValueHandling = NullValueHandling.Ignore)]
    public int? MaxTokens { get; set; }

    [JsonProperty(PropertyName = "top_p", NullValueHandling = NullValueHandling.Ignore)]
    public float? TopP { get; set; }

    [JsonProperty(PropertyName = "frequency_penalty", NullValueHandling = NullValueHandling.Ignore)]
    public float? FrequencyPenalty { get; set; }

    [JsonProperty(PropertyName = "presence_penalty", NullValueHandling = NullValueHandling.Ignore)]
    public float? PresencePenalty { get; set; }

    [JsonProperty(PropertyName = "stop", NullValueHandling = NullValueHandling.Ignore)]
    public string[]? Stop { get; set; }
  }

}
