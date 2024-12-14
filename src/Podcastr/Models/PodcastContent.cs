namespace Podcastr.Models;

/// <summary>
///     Represents the content of a podcast, 
///     including the script, description, and social media posts.
/// </summary>
/// <param name="Script">The script of the podcast.</param>
/// <param name="Description">The description of 
/// the podcast.</param>
/// <param name="SocialMediaPosts">The social media posts 
/// related to the podcast.</param>
internal record PodcastContent(
    string Script,
    string Description,
    SocialMediaPosts SocialMediaPosts);
