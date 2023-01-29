using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json.Serialization;

namespace AskGPT
{
  public class OpenAIClient
  {
    private readonly OpenAIConfiguration _options;
    private readonly IHttpClientFactory _clientFactory;

    public OpenAIClient(IOptions<OpenAIConfiguration> options,
                        IHttpClientFactory clientFactory)
    {
      Guard.IsNotNull(options);
      Guard.IsNotNull(options.Value);
      Guard.IsNotNull(clientFactory);

      _options = options.Value;
      _clientFactory = clientFactory;
    }

    public class OpenAIRequest
    {
      [JsonProperty(PropertyName ="model")]
      public string Model { get; set; } = string.Empty;
      [JsonProperty(PropertyName = "prompt")]
      public string Prompt { get; set; } = string.Empty;
    }

    public async Task<string> GetCompletionsAsync(string question)
    {
      Guard.IsNotNullOrWhiteSpace(question);

      using var client = _clientFactory.CreateClient();

      var request = new OpenAIRequest() { Model = _options.Model, Prompt = question };
      string? payload = JsonConvert.SerializeObject(request);      
      var content = new StringContent(payload, Encoding.UTF8, MediaTypeNames.Application.Json);

      HttpResponseMessage response = await client.PostAsJsonAsync(string.Empty, content);
      response.EnsureSuccessStatusCode();

      string responseString = await response.Content.ReadAsStringAsync();
      dynamic? dyData = JsonConvert.DeserializeObject<dynamic>(responseString);

      Guard.IsNotNull(dyData);

      return GuessCommand(dyData!.choices[0].text);      
    }

    private static string GuessCommand(string raw)
    {      
      int lastIndex = raw.LastIndexOf('\n');
      string guess = raw.Substring(lastIndex + 1);      
      return guess;
    }
  }
}
