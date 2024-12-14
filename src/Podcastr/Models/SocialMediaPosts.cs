namespace Podcastr.Models;

/// <summary>
///     Represents social media posts with links to 
///     LinkedIn, Twitter, and Facebook.
/// </summary>
/// <param name="LinkedIn">The LinkedIn post link.</param>
/// <param name="Twitter">The Twitter post link.</param>
/// <param name="Facebook">The Facebook post link.</param>
internal record SocialMediaPosts(
    string LinkedIn,
    string Twitter,
    string Facebook);
