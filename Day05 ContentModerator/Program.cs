using Microsoft.Azure.CognitiveServices.ContentModerator;
using Microsoft.CognitiveServices.ContentModerator;
using Microsoft.CognitiveServices.ContentModerator.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Microsoft.Extensions.Configuration;

namespace Day05_ContentModerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            builder.AddUserSecrets<Program>();

            IConfigurationRoot configuration = builder.Build();

            // API Key
            string apiKey = configuration.GetSection("apiKey").Value;
            string imageUrl = "http://pic.pimg.tw/k110107632/1387547248-3785354604.jpg";

            ContentModeratorClient client = new ContentModeratorClient(new ApiKeyServiceClientCredentials(apiKey));
            client.Endpoint = "https://southeastasia.api.cognitive.microsoft.com";

            var imageModeration =
                client.ImageModeration.EvaluateUrlInput("", new BodyModel("URL", imageUrl));
            Console.WriteLine($"IsImageAdultClassified: {imageModeration.IsImageAdultClassified}");
            Console.WriteLine($"AdultClassificationScore: {imageModeration.AdultClassificationScore}");
            Console.WriteLine($"IsImageRacyClassified: {imageModeration.IsImageRacyClassified}");
            Console.WriteLine($"RacyClassificationScore: {imageModeration.RacyClassificationScore}");
        }
    }
}
