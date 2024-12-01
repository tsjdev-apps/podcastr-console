using OpenAI.Audio;
using OpenAI.Chat;
using OpenAI.Images;
using Podcastr.Exceptions;
using Podcastr.Utils;
using System.ClientModel;

namespace Podcastr.Helpers;

/// <summary>
///     Helper class for interacting with Azure OpenAI services 
///     for podcast-related tasks.
/// </summary>
internal static class AzureOpenAIHelper
{
    /// <summary>
    ///     Retrieves the podcast content asynchronously using 
    ///     the Azure OpenAI API.
    /// </summary>
    /// <param name="chatClient">The Azure OpenAI chat client.</param>
    /// <param name="htmlContent">The content from the website.</param>
    /// <param name="podcastName">The name of the podcast.</param>
    /// <param name="podcastLanguage">The language of the podcast.</param>
    /// <returns>The podcast content as a string.</returns>
    public static async Task<string> GetPodcastContentAsync(
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
                MaxOutputTokenCount = 4096,
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

            // Extract and return the content from the response
            return chatResult.Value.Content[0].Text;
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
    ///     Generates a podcast description based on the provided script.
    /// </summary>
    /// <param name="chatClient">The Azure OpenAI chat client.</param>
    /// <param name="podcastScript">The script of the podcast.</param>
    /// <returns>A description of the podcast.</returns>
    public static async Task<string> GetPodcastDescriptionAsync(
        ChatClient chatClient,
        string? podcastScript,
        string podcastLanguage)
    {
        try
        {
            // Configure chat completion options
            ChatCompletionOptions options = new()
            {
                MaxOutputTokenCount = 1000,
                Temperature = 0.7f,
            };

            // Prepare the system message with context
            SystemChatMessage systemChatMessage =
                ChatMessage.CreateSystemMessage(
                    ChatMessageContentPart.CreateTextPart(
                        string.Format(
                            Statics.PodcastDescriptionPrompt,
                            podcastLanguage,
                            podcastScript)));

            // Request description from the OpenAI service
            ClientResult<ChatCompletion> chatResult =
                await chatClient.CompleteChatAsync(
                    [systemChatMessage],
                    options);

            // Extract and return the description from the response
            return chatResult.Value.Content[0].Text;
        }
        catch (Exception ex)
        {
            // Wrap and rethrow exceptions for better error tracing
            throw new AzureOpenAIException(
                $"Error retrieving podcast description: {ex.Message}",
                ex);
        }
    }


    /// <summary>
    ///    Generates social media posts based on the provided script.
    /// </summary>
    /// <param name="chatClient">The Azure OpenAI chat client.</param>
    /// <param name="podcastScript">The script of the podcast.</param>
    /// <returns>A description of the podcast.</returns>
    public static async Task<string> GetPodcastSocialMediaPostsAsync(
        ChatClient chatClient,
        string? podcastScript,
        string podcastLanguage)
    {
        try
        {
            // Configure chat completion options
            ChatCompletionOptions options = new()
            {
                MaxOutputTokenCount = 1000,
                Temperature = 0.7f,
            };

            // Prepare the system message with context
            SystemChatMessage systemChatMessage =
                ChatMessage.CreateSystemMessage(
                    ChatMessageContentPart.CreateTextPart(
                        string.Format(
                            Statics.PodcastSocialMediaPostsPrompt,
                            podcastLanguage,
                            podcastScript)));

            // Request description from the OpenAI service
            ClientResult<ChatCompletion> chatResult =
                await chatClient.CompleteChatAsync(
                    [systemChatMessage],
                    options);

            // Extract and return the description from the response
            return chatResult.Value.Content[0].Text;
        }
        catch (Exception ex)
        {
            // Wrap and rethrow exceptions for better error tracing
            throw new AzureOpenAIException(
                $"Error retrieving podcast social media posts: {ex.Message}",
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
