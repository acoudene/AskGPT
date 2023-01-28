// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Mime;

Console.WriteLine("Hello, World!");

var serviceCollections = new ServiceCollection();
serviceCollections.AddHttpClient(Options.DefaultName, (provider, client) =>
{
  client.BaseAddress = new Uri("https://api.openai.com/v1/completions");
  client.DefaultRequestHeaders.Accept.Clear();
  client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
  client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");
});
