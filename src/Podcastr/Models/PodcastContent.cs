namespace Podcastr.Models;

/// <summary>
/// Represents the generated content of a podcast,
/// including its script, description, and associated social media posts.
/// </summary>
/// <param name="Script">
/// The full script of the podcast episode.</param>
/// <param name="Description">
/// A short textual summary describing the episode.</param>
/// <param name="SocialMediaPosts">
/// Platform-specific social media post texts to promote the episode.</param>
internal record PodcastContent(
    string Script,
    string Description,
    SocialMediaPosts SocialMediaPosts);
