using Azure.AI.OpenAI;
using OpenAI.Audio;
using OpenAI.Chat;
using OpenAI.Images;
using Podcastr.Exceptions;
using Podcastr.Helpers;
using Podcastr.Models;
using Podcastr.Utils;
using System.ClientModel;

ChatClient? chatClient = null;
AudioClient? audioClient = null;
ImageClient? imageClient = null;
bool shouldRepeat = true;

// Show the application header
ConsoleHelper.ShowHeader();

// Get the Azure OpenAI endpoint URL from the user
var azureOpenAIEndpoint = Secrets.AzureOpenAIEndpoint ??
    ConsoleHelper.GetUrlFromConsole(
        "Enter your [yellow]Azure OpenAI endpoint[/] endpoint URL:",
        false);

// Get the Azure OpenAI key from the user
var azureOpenAIKey = Secrets.AzureOpenAIKey ??
    ConsoleHelper.GetStringFromConsole(
        "Enter your [yellow]Azure OpenAI key[/] key:",
        showHeader: false);

// Get the Azure OpenAI Chat model name
var chatModelName = Secrets.AzureOpenAIChatModelName ??
    ConsoleHelper.GetStringFromConsole(
        "Enter your [yellow]Azure OpenAI Chat model[/] name:",
        showHeader: false);

// Get the Azure OpenAI Audio model name
var audioModelName = Secrets.AzureOpenAIAudioModelName ??
    ConsoleHelper.GetStringFromConsole(
        "Enter your [yellow]Azure OpenAI Audio model[/] name:",
        showHeader: false);

// Get the Azure OpenAI Image model name
var imageModelName = Secrets.AzureOpenAIImageModelName ??
    ConsoleHelper.GetStringFromConsole(
        "Enter your [yellow]Azure OpenAI Image model[/] name:",
        showHeader: false);

// Create the Azure OpenAI Client
AzureOpenAIClient azureOpenAIClient = new(
    new Uri(azureOpenAIEndpoint),
    new ApiKeyCredential(azureOpenAIKey));

// Initialize service-specific clients
chatClient =
    azureOpenAIClient.GetChatClient(chatModelName);
audioClient =
    azureOpenAIClient.GetAudioClient(audioModelName);
imageClient =
    azureOpenAIClient.GetImageClient(imageModelName);


// Main loop for processing multiple podcasts
while (shouldRepeat)
{
    ConsoleHelper.ShowHeader();

    // Reset TokenUsage
    TokenUsageHelper.Reset();

    // Get an URL from the user containing the content
    var contentUrl =
        ConsoleHelper.GetStringFromConsole(
            "Enter the [yellow]URL[/] of the content:");

    // Get the name of the podcast
    var podcastName =
        ConsoleHelper.GetStringFromConsole(
            "Enter the [yellow]name[/] of the podcast:");

    // Get the language of the podcast
    var podcastLanguage =
        ConsoleHelper.SelectFromOptions(
            Statics.PodcastLanguages,
            "Select the [yellow]language[/] of the podcast");

    // Get the voice of the podcast
    var podcastVoice =
        ConsoleHelper.SelectFromOptions(
            Statics.PodcastVoices,
            "Select the [yellow]voice[/] of the podcast");

    // Show the header
    ConsoleHelper.ShowHeader();

    // Get the content from the URL
    var content = await ExecuteWithHandlingAsync("Loading content...",
        async () => await WebsiteHelper.GetHtmlBodyAsync(contentUrl));

    if (!ValidateStep(content, "Loading content", ref shouldRepeat))
        continue;

    // Get the podcast script
    var podcastScript = await ExecuteWithHandlingAsync("Generating podcast script...",
        async () => await AzureOpenAIHelper.GetPodcastContentAsync(
            chatClient,
            content,
            podcastName,
            podcastLanguage));

    if (!ValidateStep(podcastScript, "Podcast script generation", ref shouldRepeat))
        continue;

    // Start parallel tasks for remaining operations
    var descriptionTask = ExecuteWithHandlingAsync("Generating podcast description...",
        async () => await AzureOpenAIHelper.GetPodcastDescriptionAsync(
            chatClient,
            podcastScript,
            podcastLanguage));

    var socialMediaPostsTask = ExecuteWithHandlingAsync("Generating social media posts...",
        async () => await AzureOpenAIHelper.GetPodcastSocialMediaPostsAsync(
            chatClient,
            podcastScript,
            podcastLanguage));

    var audioTask = ExecuteWithHandlingAsync("Generating podcast audio...",
        async () => await AzureOpenAIHelper.GetPodcastAudioAsync(
            audioClient,
            podcastScript,
            podcastVoice));

    var imageTask = ExecuteWithHandlingAsync("Generating podcast image...",
        async () => await AzureOpenAIHelper.GetPodcastCoverAsync(
            chatClient,
            imageClient,
            podcastScript));

    // Wait for all tasks to complete
    await Task.WhenAll(descriptionTask, socialMediaPostsTask, audioTask, imageTask);

    // Get results from completed tasks
    var podcastDescription = descriptionTask.Result;
    var podcastSocialMediaPosts = socialMediaPostsTask.Result;
    var podcastAudio = audioTask.Result;
    var podcastImage = imageTask.Result;

    // Validate results
    if (!ValidateStep(podcastDescription, "Podcast description generation", ref shouldRepeat) ||
        !ValidateStep(podcastSocialMediaPosts, "Podcast social media posts generation", ref shouldRepeat) ||
        !ValidateStep(podcastAudio, "Podcast audio generation", ref shouldRepeat) ||
        !ValidateStep(podcastImage, "Podcast image generation", ref shouldRepeat))
    {
        continue;
    }

    // Create Zip Archive containing all the created files
    var zipArchive = await ExecuteWithHandlingAsync("Creating zip archive...",
        () => Task.FromResult(ZipArchiveHelper.CreateZipArchive(
            [
                new ZipElement(podcastScript, null, "podcast-script.txt"),
                new ZipElement(podcastDescription, null, "podcast-description.txt"),
                new ZipElement(podcastSocialMediaPosts, null, "podcast-socialmediaposts.txt"),
                new ZipElement(null, podcastAudio, "podcast-audio.mp3"),
                new ZipElement(null, podcastImage, "podcast-image.png")
            ])));

    if (!ValidateStep(zipArchive, "Creating zip archive", ref shouldRepeat))
        continue;

    // Save the zip archive to disk
    var zipFilePath = await FileHelper.WriteToTempFolderAsync(zipArchive);
    ConsoleHelper.WriteMessage($"Zip archive saved to [link][yellow]{zipFilePath}[/][/]");

    ConsoleHelper.RenderTokenUsageTable(podcastImage is not null);

    shouldRepeat = ConsoleHelper.GetConfirmation("Do you want to repeat the process?", false);
}

// Wrapper to execute operations with error handling
static async Task<T?> ExecuteWithHandlingAsync<T>(string operationDescription, Func<Task<T>> operation)
{
    try
    {
        ConsoleHelper.WriteMessage($"{operationDescription}...");
        return await operation();
    }
    catch (AzureOpenAIException ex)
    {
        ConsoleHelper.WriteError($"{operationDescription} failed: {ex.Message}");
        return default;
    }
}

// Validate step output and determine whether to continue
static bool ValidateStep<T>(T value, string stepDescription, ref bool shouldRepeat)
{
    if (value == null || (value is string str && string.IsNullOrEmpty(str)))
    {
        ConsoleHelper.WriteError($"{stepDescription} failed.");

        shouldRepeat =
            ConsoleHelper.GetConfirmation(
                "Do you want to restart the process?",
                true);

        return false;
    }

    return true;
}