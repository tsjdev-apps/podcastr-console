namespace Podcastr.Models;

/// <summary>
/// Represents social media post texts associated with a podcast episode.
/// Typically used to share the episode on various platforms.
/// </summary>
/// <param name="LinkedIn">The LinkedIn post content.</param>
/// <param name="Twitter">The Twitter (X) post content.</param>
/// <param name="Facebook">The Facebook post content.</param>
/// <param name="Threads">The Threads post content.</param>
internal record SocialMediaPosts(
    string LinkedIn,
    string Twitter,
    string Facebook,
    string Threads);
