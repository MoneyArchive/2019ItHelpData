using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.Configuration.Json;

namespace Day02_Computer_Vision
{
    class Program
    {
        // 指定要回傳的資訊
        private static readonly List<VisualFeatureTypes> features =
            new List<VisualFeatureTypes>()
        {
            VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
            VisualFeatureTypes.ImageType, VisualFeatureTypes.Tags
        };

        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            builder.AddUserSecrets<Program>();

            IConfigurationRoot configuration = builder.Build();

            // API Key
            string apiKey = configuration.GetSection("apiKey").Value;
            string imageUrl =
            "https://img.appledaily.com.tw/images/ReNews/20181016/640_32e5f4fe808e4669a337b7b30ce3df81.jpg";

            ComputerVisionClient client = new ComputerVisionClient(
                new ApiKeyServiceClientCredentials(apiKey),
                new System.Net.Http.DelegatingHandler[] { });

            // 根據 Azure Portal 上之資訊填寫
            client.Endpoint = "https://southeastasia.api.cognitive.microsoft.com/";

            Console.WriteLine("Images being analyzed ...");
            ImageAnalysis analysis =
                await client.AnalyzeImageAsync(imageUrl, features);
            Console.WriteLine("Text: " + analysis.Description.Captions[0].Text);
            Console.WriteLine("Confidence: " + analysis.Description.Captions[0].Confidence);
            Console.WriteLine("Categories: " + string.Join(", ", analysis.Categories.Select(x=>x.Name)));
            Console.WriteLine("Tags: " + string.Join(", ", analysis.Tags.Select(x=>x.Name)));
        }
    }
}
