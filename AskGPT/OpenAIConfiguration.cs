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

    public string Key { get; set; } = "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";

    public string Url { get; set; } = "https://api.openai.com/v1/completions";

    public string Model { get; set; } = "text-davinci-003";

    public int Temperature { get; set; } = 0;

    public int MaxTokens { get; set; } = 100;

    public float TopP { get; set; } = 1.0f;

    public float FrequencyPenalty { get; set; } = 0.2f;

    public float PresencePenalty { get; set; } = 0.0f;

    public string[] Stop { get; set; } = { "\n" };

  }
}
