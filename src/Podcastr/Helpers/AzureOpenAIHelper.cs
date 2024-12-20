﻿using OpenAI.Audio;
using OpenAI.Chat;
using OpenAI.Images;
using Podcastr.Exceptions;
using Podcastr.Models;
using Podcastr.Utils;
using System.ClientModel;
using System.Text.Json;

namespace Podcastr.Helpers;

/// <summary>
///     Helper class for interacting with Azure OpenAI services 
///     for podcast-related tasks.
/// </summary>
internal static class AzureOpenAIHelper
{
    /// <summary>
    ///     JSON serializer options with case-insensitive property name matching.
    /// </summary>
    private static readonly JsonSerializerOptions _jsonSerializerOptions =
        new() { PropertyNameCaseInsensitive = true };

    /// <summary>
    ///     Retrieves podcast content asynchronously using the provided chat client.
    /// </summary>
    /// <param name="chatClient">The Azure OpenAI chat client.</param>
    /// <param name="htmlContent">The HTML content to be processed.</param>
    /// <param name="podcastName">The name of the podcast.</param>
    /// <param name="podcastLanguage">The language of the podcast.</param>
    /// <returns>A task that represents the asynchronous operation. 
    /// The task result contains the podcast content.</returns>
    /// <exception cref="AzureOpenAIException">Thrown when there is an 
    /// error retrieving the podcast content.</exception>
    public static async Task<PodcastContent?> GetPodcastContentAsync(
        ChatClient chatClient,
        string? htmlContent,
        string podcastName,
        string podcastLanguage)
    {
        try
        {
            // Configure chat completion options
            ChatCompletionOptions options = new()
            {
                ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                    "podcast_content",
                    jsonSchema: BinaryData.FromString(
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
                                "linkedIn": {
                                  "type": "string",
                                  "description": "The LinkedIn post for the podcast"
                                },
                                "twitter": {
                                  "type": "string",
                                  "description": "The Twitter post for the podcast"
                                },
                                "Facebook": {
                                  "type": "string",
                                  "description": "The Facebook post for the podcast"
                                }
                              },
                              "required": ["linkedIn", "twitter", "facebook"],
                              "addionalProperties": false,
                              "description": "Social media posts for various platforms"
                            }
                          },
                          "required": ["script", "description", "socialMediaPosts"],
                          "additionalProperties": false
                        }
                        """)),
                Temperature = 0.7f,
            };

            // Prepare the system message with relevant context
            SystemChatMessage systemChatMessage =
                ChatMessage.CreateSystemMessage(
                    ChatMessageContentPart.CreateTextPart(
                        string.Format(
                            Statics.PodcastPrompt,
                            podcastName,
                            podcastLanguage,
                            htmlContent)));

            // Request completion from the OpenAI service
            ClientResult<ChatCompletion> chatResult =
                await chatClient.CompleteChatAsync(
                    [systemChatMessage],
                    options);

            // Get Input Tokens and Output Tokens
            ChatTokenUsage usage = chatResult.Value.Usage;
            TokenUsageHelper.AddChatInputTokenCount(usage.InputTokenCount);
            TokenUsageHelper.AddChatOutputTokenCount(usage.OutputTokenCount);

            // Extract and return the content from the response
            using JsonDocument structuredJson =
                JsonDocument.Parse(chatResult.Value.Content[0].Text);

            return JsonSerializer.Deserialize<PodcastContent>(
                structuredJson.RootElement.ToString(),
                _jsonSerializerOptions);
        }
        catch (Exception ex)
        {
            // Wrap and rethrow exceptions for better error tracing
            throw new AzureOpenAIException(
                $"Error retrieving podcast content: {ex.Message}",
                ex);
        }
    }

    /// <summary>
    ///     Generates podcast audio based on the provided script and voice settings.
    /// </summary>
    /// <param name="audioClient">The Azure OpenAI audio client.</param>
    /// <param name="podcastScript">The script of the podcast.</param>
    /// <param name="voice">The desired voice for the audio.</param>
    /// <returns>A byte array containing the generated audio.</returns>
    public static async Task<byte[]> GetPodcastAudioAsync(
        AudioClient audioClient,
        string? podcastScript,
        string voice)
    {
        try
        {
            // Generate speech from the script
            ClientResult<BinaryData> audioResult =
                await audioClient.GenerateSpeechAsync(
                    podcastScript,
                    new GeneratedSpeechVoice(voice.ToLower()));

            // Add Audio Characters
            TokenUsageHelper.AddAudioInputCharacters(podcastScript?.Length ?? 0);

            // Return the audio as a byte array
            return audioResult.Value.ToArray();
        }
        catch (Exception ex)
        {
            // Log and return an empty array in case of an error
            ConsoleHelper.WriteError($"Error generating podcast audio: {ex.Message}");
            return [];
        }
    }

    /// <summary>
    ///     Generates a podcast cover image based on the provided script.
    /// </summary>
    /// <param name="chatClient">The Azure OpenAI chat client.</param>
    /// <param name="imageClient">The Azure OpenAI image client.</param>
    /// <param name="podcastScript">The script of the podcast.</param>
    /// <returns>A byte array containing the generated cover image.</returns>
    public static async Task<byte[]> GetPodcastCoverAsync(
        ChatClient chatClient,
        ImageClient imageClient,
        string? podcastScript)
    {
        try
        {
            // Configure chat completion options for generating an image prompt
            ChatCompletionOptions chatOptions = new()
            {
                MaxOutputTokenCount = 1000,
                Temperature = 0.7f,
            };

            // Generate a descriptive prompt for the cover image
            SystemChatMessage systemChatMessage =
                ChatMessage.CreateSystemMessage(
                    ChatMessageContentPart.CreateTextPart(
                        string.Format(
                            Statics.PodcastCoverPreparationPrompt,
                            podcastScript)));

            ClientResult<ChatCompletion> chatResult =
                await chatClient.CompleteChatAsync(
                    [systemChatMessage],
                    chatOptions);

            // Get Input Tokens and Output Tokens
            ChatTokenUsage usage = chatResult.Value.Usage;
            TokenUsageHelper.AddChatInputTokenCount(usage.InputTokenCount);
            TokenUsageHelper.AddChatOutputTokenCount(usage.OutputTokenCount);

            // Extract the generated image prompt
            string imagePrompt = chatResult.Value.Content[0].Text;

            // Configure image generation options
            ImageGenerationOptions imageOptions = new()
            {
                Quality = GeneratedImageQuality.Standard,
                ResponseFormat = GeneratedImageFormat.Bytes,
                Style = GeneratedImageStyle.Vivid,
                Size = GeneratedImageSize.W1024xH1024,
            };

            // Generate the image based on the prompt
            ClientResult<GeneratedImage> imageResult =
                await imageClient.GenerateImageAsync(imagePrompt, imageOptions);

            // Return the generated image as a byte array
            return imageResult.Value.ImageBytes.ToArray();
        }
        catch (Exception ex)
        {
            // Log and return an empty array in case of an error
            ConsoleHelper.WriteError($"Error generating podcast cover: {ex.Message}");
            return [];
        }
    }
}
