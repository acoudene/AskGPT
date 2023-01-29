// See https://aka.ms/new-console-template for more information
using AskGPT;
using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Mime;

if (!args.Any())
{
  Console.WriteLine("You must provide a question in arguments");
  return;
}

var builder = Host.CreateApplicationBuilder(args);

builder.Services
  .Configure<OpenAIConfiguration>(builder.Configuration.GetSection(OpenAIConfiguration.OpenAIConfigurationKey));

builder.Services
  .AddHttpClient(Options.DefaultName, (serviceProvider, client) =>
  {
    var openAIConfiguration = serviceProvider
    .GetRequiredService<IOptions<OpenAIConfiguration>>()?
    .Value;

    Guard.IsNotNull(openAIConfiguration);
    Guard.IsNotNullOrWhiteSpace(openAIConfiguration.Url);
    Guard.IsNotNullOrWhiteSpace(openAIConfiguration.Key);

    client.BaseAddress = new Uri(openAIConfiguration.Url);
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", openAIConfiguration.Key);
  });

builder.Services
  .AddTransient<OpenAIClient>();

using IHost host = builder.Build();

string guess = "Error";

try
{
  string question = args[0];
  var client = host.Services.GetRequiredService<OpenAIClient>();
  guess = await client.GetCompletionsAsync(question);
 
  Console.ForegroundColor = ConsoleColor.Green;
  Console.WriteLine($"{guess}");
  Console.ResetColor();

  TextCopy.ClipboardService.SetText(guess);
}
catch (Exception ex)
{
  Console.ForegroundColor = ConsoleColor.Red;
  Console.WriteLine($"{ex.Message}");
  Console.ResetColor();

}




