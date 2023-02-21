using AskGPT;
using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MudBlazor.Services;
using System.Net.Http.Headers;
using System.Net.Mime;

namespace AskGPTApp
{
  public static class MauiProgram
  {
    public static MauiApp CreateMauiApp()
    {
      var builder = MauiApp.CreateBuilder();
      builder
        .UseMauiApp<App>()
        .ConfigureFonts(fonts =>
        {
          fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
        });

      builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

      builder.Services.AddMudServices();

      builder.Services
        .Configure<OpenAIConfiguration>(option =>
        {
          option.MaxTokens = 2000;
          option.Stop = new string[] { "###" };
        });

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

      return builder.Build();
    }
  }
}