using OpenAI.Audio;
using OpenAI.Chat;
using OpenAI.Images;
using Podcastr.Exceptions;
using Podcastr.Models;
using Podcastr.Utils;
using System.ClientModel;
using System.Text.Json;

namespace Podcastr.Helpers;

/// <summary>
/// Helper methods for interacting with Azure OpenAI services 
/// to generate podcast content, audio, and cover images.
/// </summary>
internal static class AzureOpenAIHelper
{
    private static readonly JsonSerializerOptions _jsonSerializerOptions =
        new() { PropertyNameCaseInsensitive = true };

    /// <summary>
    /// Retrieves podcast content based on the provided HTML input.
    /// </summary>
    /// <param name="chatClient">The Azure OpenAI chat client.</param>
    /// <param name="htmlContent">The input HTML content.</param>
    /// <param name="podcastName">The name of the podcast.</param>
    /// <param name="podcastLanguage">The language of the podcast.</param>
    /// <returns>A <see cref="PodcastContent"/> object containing script, 
    /// description, and social posts; or null if deserialization fails.</returns>
    /// <exception cref="AzureOpenAIException">Thrown when the request to OpenAI fails.</exception>
    public static async Task<PodcastContent?> GetPodcastContentAsync(
        ChatClient chatClient,
        string? htmlContent,
        string podcastName,
        string podcastLanguage)
    {
        try
        {
            ChatCompletionOptions options = new()
            {
                Temperature = 0.7f,
                ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                    "podcast_content",
                    BinaryData.FromString(
                        /* language=JSON */
                        """
                        {
                          "type": "object",
                          "properties": {
                            "script": {
                              "type": "string",
                              "description": "The script of the podcast"
                            },
                            "description": {
                              "type": "string",
                              "description": "A brief description of the podcast"
                            },
                            "socialMediaPosts": {
                              "type": "object",
                              "properties": {
                                "linkedIn": { "type": "string" },
                                "twitter":  { "type": "string" },
                                "facebook": { "type": "string" },
                                "threads":  { "type": "string" }
                              },
                              "required": ["linkedIn", "twitter", "facebook", "threads"],
                              "additionalProperties": false
                            }
                          },
                          "required": ["script", "description", "socialMediaPosts"],
                          "additionalProperties": false
                        }
                        """))
            };

            SystemChatMessage systemChatMessage = ChatMessage.CreateSystemMessage(
                ChatMessageContentPart.CreateTextPart(
                    string.Format(Statics.PodcastPrompt, podcastName, podcastLanguage, htmlContent)));

            ClientResult<ChatCompletion> chatResult = await chatClient.CompleteChatAsync([systemChatMessage], options);

            ChatTokenUsage usage = chatResult.Value.Usage;
            TokenUsageHelper.AddChatInputTokenCount(usage.InputTokenCount);
            TokenUsageHelper.AddChatOutputTokenCount(usage.OutputTokenCount);

            using JsonDocument structuredJson = JsonDocument.Parse(chatResult.Value.Content[0].Text);

            return JsonSerializer.Deserialize<PodcastContent>(
                structuredJson.RootElement.ToString(), _jsonSerializerOptions);
        }
        catch (Exception ex)
        {
            throw new AzureOpenAIException("Error retrieving podcast content.", ex);
        }
    }

    /// <summary>
    /// Generates audio from the provided podcast script using 
    /// Azure OpenAI speech synthesis.
    /// </summary>
    /// <param name="audioClient">The audio generation client.</param>
    /// <param name="podcastScript">The text to synthesize.</param>
    /// <param name="voice">The name of the voice to use (e.g., 'Nova').</param>
    /// <returns>The synthesized audio as a byte array; 
    /// empty if generation fails.</returns>
    public static async Task<byte[]> GetPodcastAudioAsync(
        AudioClient audioClient,
        string? podcastScript,
        string voice)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(podcastScript))
            {
                return [];
            }

            ClientResult<BinaryData> audioResult = await audioClient.GenerateSpeechAsync(
                podcastScript,
                new GeneratedSpeechVoice(voice.ToLower()));

            TokenUsageHelper.AddAudioInputCharacters(podcastScript.Length);

            return audioResult.Value.ToArray();
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"Error generating podcast audio: {ex.Message}");
            return [];
        }
    }

    /// <summary>
    /// Generates a podcast cover image based on the script content.
    /// </summary>
    /// <param name="chatClient">The chat client used to generate the image prompt.</param>
    /// <param name="imageClient">The image client used to generate the image.</param>
    /// <param name="podcastScript">The podcast script used to derive the image description.</param>
    /// <returns>The generated cover image as a byte array; empty if generation fails.</returns>
    public static async Task<byte[]> GetPodcastCoverAsync(
        ChatClient chatClient,
        ImageClient imageClient,
        string? podcastScript)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(podcastScript))
            {
                return [];
            }

            ChatCompletionOptions chatOptions = new()
            {
                Temperature = 0.7f,
                MaxOutputTokenCount = 1000
            };

            SystemChatMessage systemChatMessage = ChatMessage.CreateSystemMessage(
                ChatMessageContentPart.CreateTextPart(
                    string.Format(Statics.PodcastCoverPreparationPrompt, podcastScript)));

            ClientResult<ChatCompletion> chatResult = 
                await chatClient.CompleteChatAsync([systemChatMessage], chatOptions);

            ChatTokenUsage usage = chatResult.Value.Usage;
            TokenUsageHelper.AddChatInputTokenCount(usage.InputTokenCount);
            TokenUsageHelper.AddChatOutputTokenCount(usage.OutputTokenCount);

            string imagePrompt = chatResult.Value.Content[0].Text;

            ImageGenerationOptions imageOptions = new()
            {
                Quality = GeneratedImageQuality.Standard,
                ResponseFormat = GeneratedImageFormat.Bytes,
                Style = GeneratedImageStyle.Vivid,
                Size = GeneratedImageSize.W1024xH1024
            };

            ClientResult<GeneratedImage> imageResult = 
                await imageClient.GenerateImageAsync(imagePrompt, imageOptions);

            return imageResult.Value.ImageBytes.ToArray();
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"Error generating podcast cover: {ex.Message}");
            return [];
        }
    }
}
