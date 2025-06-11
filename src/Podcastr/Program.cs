using Azure.AI.OpenAI;
using OpenAI.Audio;
using OpenAI.Chat;
using OpenAI.Images;
using Podcastr.Exceptions;
using Podcastr.Helpers;
using Podcastr.Models;
using Podcastr.Utils;
using System.ClientModel;

// Clients and control flags
ChatClient? chatClient = null;
AudioClient? audioClient = null;
ImageClient? imageClient = null;
bool shouldRepeat = true;

// Display app header
ConsoleHelper.ShowHeader();

#region User Configuration

string azureOpenAIEndpoint = Secrets.AzureOpenAIEndpoint ??
    ConsoleHelper.GetUrlFromConsole(
        "Enter your [yellow]Azure OpenAI endpoint[/] URL:", false);

string azureOpenAIKey = Secrets.AzureOpenAIKey ??
    ConsoleHelper.GetStringFromConsole(
        "Enter your [yellow]Azure OpenAI key[/]:", false);

string chatModelName = Secrets.AzureOpenAIChatModelName ??
    ConsoleHelper.GetStringFromConsole(
        "Enter your [yellow]Chat model name[/]:", false);

string audioModelName = Secrets.AzureOpenAIAudioModelName ??
    ConsoleHelper.GetStringFromConsole(
        "Enter your [yellow]Audio model name[/]:", false);

string imageModelName = Secrets.AzureOpenAIImageModelName ??
    ConsoleHelper.GetStringFromConsole(
        "Enter your [yellow]Image model name[/]:", false);

#endregion

#region Client Initialization

AzureOpenAIClient azureOpenAIClient = new(
    new Uri(azureOpenAIEndpoint),
    new ApiKeyCredential(azureOpenAIKey));

chatClient = azureOpenAIClient.GetChatClient(chatModelName);
audioClient = azureOpenAIClient.GetAudioClient(audioModelName);
imageClient = azureOpenAIClient.GetImageClient(imageModelName);

#endregion

#region Main Loop

while (shouldRepeat)
{
    ConsoleHelper.ShowHeader();
    TokenUsageHelper.Reset();

    // === Collect Podcast Metadata ===
    string contentUrl =
        ConsoleHelper.GetStringFromConsole(
            "Enter the [yellow]URL[/] of the content:");

    string podcastName =
        ConsoleHelper.GetStringFromConsole(
            "Enter the [yellow]name[/] of the podcast:");
    string podcastLanguage =
        ConsoleHelper.SelectFromOptions(
            Statics.PodcastLanguages,
            "Select the [yellow]language[/] of the podcast");

    string podcastVoice =
        ConsoleHelper.SelectFromOptions(
            Statics.PodcastVoices,
            "Select the [yellow]voice[/] of the podcast");

    ConsoleHelper.ShowHeader();

    // === Load & Process Content ===
    string? content = await ExecuteWithHandlingAsync(
        "Loading content",
        () => WebsiteHelper.GetHtmlBodyAsync(
            contentUrl));

    if (!ValidateStep(content, "Loading content", ref shouldRepeat))
    {
        continue;
    }

    PodcastContent? podcastContent = await ExecuteWithHandlingAsync(
        "Generating podcast content",
        () => AzureOpenAIHelper.GetPodcastContentAsync(
            chatClient,
            content,
            podcastName,
            podcastLanguage));

    if (!ValidateStep(podcastContent, "Podcast content generation", ref shouldRepeat))
    {
        continue;
    }

    // === Parallel Processing ===
    Task<byte[]?> audioTask = ExecuteWithHandlingAsync(
        "Generating audio",
        () => AzureOpenAIHelper.GetPodcastAudioAsync(
            audioClient,
            podcastContent?.Script,
            podcastVoice));

    Task<byte[]?> imageTask = ExecuteWithHandlingAsync(
        "Generating image",
        () => AzureOpenAIHelper.GetPodcastCoverAsync(
            chatClient,
            imageClient,
            podcastContent?.Script));

    await Task.WhenAll(audioTask, imageTask);

    byte[]? podcastAudio = audioTask.Result;
    byte[]? podcastImage = imageTask.Result;

    if (!ValidateStep(podcastAudio, "Audio generation", ref shouldRepeat) ||
        !ValidateStep(podcastImage, "Image generation", ref shouldRepeat))
    {
        continue;
    }

    // === Create ZIP Archive ===
    byte[]? zipArchive = await ExecuteWithHandlingAsync(
        "Creating ZIP archive",
        () => Task.FromResult(ZipArchiveHelper.CreateZipArchive([
            new ZipElement(podcastContent?.Script, null, "podcast-script.txt"),
            new ZipElement(podcastContent?.Description, null, "podcast-description.txt"),
            new ZipElement(podcastContent?.SocialMediaPosts?.LinkedIn, null, "social-linkedin.txt"),
            new ZipElement(podcastContent?.SocialMediaPosts?.Facebook, null, "social-facebook.txt"),
            new ZipElement(podcastContent?.SocialMediaPosts?.Twitter, null, "social-twitter.txt"),
            new ZipElement(podcastContent?.SocialMediaPosts?.Threads, null, "social-threads.txt"),
            new ZipElement(null, podcastAudio, "podcast-audio.mp3"),
            new ZipElement(null, podcastImage, "podcast-image.png")
        ])));

    if (!ValidateStep(zipArchive, "ZIP archive creation", ref shouldRepeat))
    {
        continue;
    }

    string zipPath = await FileHelper.WriteToTempFolderAsync(zipArchive);
    ConsoleHelper.WriteMessage($"ZIP archive saved to [link][yellow]{zipPath}[/][/]");

    // === Show Cost Table ===
    TableHelper.ShowTable(podcastImage is not null);

    shouldRepeat = ConsoleHelper.GetConfirmation(
        "Do you want to create another podcast?", 
        false);
}

#endregion

#region Helpers

static async Task<T?> ExecuteWithHandlingAsync<T>(
    string description, 
    Func<Task<T>> operation)
{
    try
    {
        ConsoleHelper.WriteMessage($"{description}...");
        return await operation();
    }
    catch (AzureOpenAIException ex)
    {
        ConsoleHelper.WriteError($"{description} failed: {ex.Message}");
        return default;
    }
}

static bool ValidateStep<T>(T? result, string step, ref bool shouldRepeat)
{
    if (result is null || (result is string str && string.IsNullOrWhiteSpace(str)))
    {
        ConsoleHelper.WriteError($"{step} failed.");
        shouldRepeat = ConsoleHelper.GetConfirmation("Would you like to retry?", true);
        return false;
    }

    return true;
}

#endregion
