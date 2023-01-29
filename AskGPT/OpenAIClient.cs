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
  public partial class OpenAIClient
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

    public async Task<string> GetCompletionsAsync(string question)
    {
      Guard.IsNotNullOrWhiteSpace(question);

      using var client = _clientFactory.CreateClient();

      var request = new OpenAIRequest()
      {
        Model = _options.Model,
        Prompt = question,
        FrequencyPenalty = _options.FrequencyPenalty,
        MaxTokens = _options.MaxTokens,
        PresencePenalty = _options.PresencePenalty,
        //Stop = _options.Stop,
        Temperature = _options.Temperature,
        TopP = _options.TopP
      };

      string? payload = JsonConvert.SerializeObject(request);      
      var content = new StringContent(payload, Encoding.UTF8, MediaTypeNames.Application.Json);

      HttpResponseMessage response = await client.PostAsync(string.Empty, content);
      response.EnsureSuccessStatusCode();

      string responseString = await response.Content.ReadAsStringAsync();
      dynamic? dyData = JsonConvert.DeserializeObject<dynamic>(responseString);

      Guard.IsNotNull(dyData);

      string text = dyData!.choices[0].text;
      return GuessCommand(text);      
    }

    private static string GuessCommand(string raw)
    {      
      int lastIndex = raw.LastIndexOf('\n');
      string guess = raw.Substring(lastIndex + 1);      
      return guess;
    }
  }
}
