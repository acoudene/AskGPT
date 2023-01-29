using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskGPT
{
  public class OpenAIConfiguration
  {
    public const string OpenAIConfigurationKey = "OPENAI";

    public string Key { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public string Prompt { get; set; } = string.Empty;

    public int Temperature { get; set; }

    public int MaxTokens { get; set; }

    public float TopP { get; set; }

    public float FrequencyPenalty { get; set; }

    public float PresencePenalty { get; set; }

    public string[] Stop { get; set; } = Array.Empty<string>();

  }
}
