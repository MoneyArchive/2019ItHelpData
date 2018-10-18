using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.Configuration.Json;

namespace Day03_FaceAPI
{
    class Program
    {
        private static readonly FaceAttributeType[] faceAttributes =
        {
            FaceAttributeType.Age, FaceAttributeType.Gender, FaceAttributeType.Accessories, FaceAttributeType.Blur,
            FaceAttributeType.Emotion, FaceAttributeType.Exposure, FaceAttributeType.FacialHair,
            FaceAttributeType.Glasses, FaceAttributeType.Hair, FaceAttributeType.HeadPose, FaceAttributeType.Makeup,
            FaceAttributeType.Noise, FaceAttributeType.Occlusion, FaceAttributeType.Smile
        };

        static async System.Threading.Tasks.Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            builder.AddUserSecrets<Program>();

            IConfigurationRoot configuration = builder.Build();

            // API Key
            string apiKey = configuration.GetSection("apiKey").Value;
            string imageUrl = "https://cdn-images-1.medium.com/max/1200/0*WVev4lO0ZXp6tDed";

            FaceClient faceClient = new FaceClient(
                new ApiKeyServiceClientCredentials(apiKey),
                new System.Net.Http.DelegatingHandler[] { });
            faceClient.Endpoint = "https://southeastasia.api.cognitive.microsoft.com/";

            Console.WriteLine("Faces being detected ...");
            IList<DetectedFace> faceList =
                    await faceClient.Face.DetectWithUrlAsync(
                        imageUrl, true, false, faceAttributes);

            var face = faceList[0];
            Console.WriteLine($"FaceId: {face.FaceId}");
            Console.WriteLine($"Age: {face.FaceAttributes.Age}");
            Console.WriteLine($"Blur: {face.FaceAttributes.Blur.BlurLevel}, {face.FaceAttributes.Blur.Value}");
            Console.WriteLine($"Emotion: {{Anger: {face.FaceAttributes.Emotion.Anger}, Contempt: {face.FaceAttributes.Emotion.Contempt}, Disgust: {face.FaceAttributes.Emotion.Disgust}, Fear: {face.FaceAttributes.Emotion.Fear}, Happiness: {face.FaceAttributes.Emotion.Happiness}, Neutral: {face.FaceAttributes.Emotion.Neutral}, Sadness: {face.FaceAttributes.Emotion.Sadness}, Surprise: {face.FaceAttributes.Emotion.Surprise}}}");
            Console.WriteLine($"Exposure: {face.FaceAttributes.Exposure.ExposureLevel}, {face.FaceAttributes.Exposure.Value}");
            Console.WriteLine($"FacialHair: {{Beard: {face.FaceAttributes.FacialHair.Beard}, Moustache: {face.FaceAttributes.FacialHair.Moustache}, Sideburns: {face.FaceAttributes.FacialHair.Sideburns}}}");
            Console.WriteLine($"Gender: {face.FaceAttributes.Gender}");
            Console.WriteLine($"Glasses: {face.FaceAttributes.Glasses}");
            Console.WriteLine($"Hair: {{HairColor: {face.FaceAttributes.Hair.HairColor[0].Color}:{face.FaceAttributes.Hair.HairColor[0].Confidence}, Bald:{face.FaceAttributes.Hair.Bald}, Invisible:{face.FaceAttributes.Hair.Invisible}}}");
            Console.WriteLine($"HeadPose: {{ Pitch: {face.FaceAttributes.HeadPose.Pitch}, Roll: {face.FaceAttributes.HeadPose.Roll}, Yaw:{face.FaceAttributes.HeadPose.Yaw}}}");
            Console.WriteLine($"Makeup: {{ EyeMakeup: {face.FaceAttributes.Makeup.EyeMakeup}, LipMakeup: {face.FaceAttributes.Makeup.LipMakeup}}}");
            Console.WriteLine($"Noise: {{ NoiseLevel: {face.FaceAttributes.Noise.NoiseLevel}, Value: {face.FaceAttributes.Noise.Value}}}");
            Console.WriteLine($"Occlusion: {{ EyeOccluded: {face.FaceAttributes.Occlusion.EyeOccluded}, ForeheadOccluded: {face.FaceAttributes.Occlusion.ForeheadOccluded}, MouthOccluded: {face.FaceAttributes.Occlusion.MouthOccluded}}}");
            Console.WriteLine($"Smile: {face.FaceAttributes.Smile}");
        }
    }
}
