namespace Podcastr.Helpers;

/// <summary>
/// Provides methods to track and manage token usage 
/// and audio input character counts.
/// </summary>
public static class TokenUsageHelper
{
    private static int _chatInputTokenCount;
    private static int _chatOutputTokenCount;
    private static int _audioInputCharacters;

    /// <summary>
    /// Resets all tracked usage counters to zero.
    /// </summary>
    public static void Reset()
    {
        Interlocked.Exchange(ref _chatInputTokenCount, 0);
        Interlocked.Exchange(ref _chatOutputTokenCount, 0);
        Interlocked.Exchange(ref _audioInputCharacters, 0);
    }

    /// <summary>
    /// Gets the total number of chat input tokens recorded.
    /// </summary>
    public static int GetChatInputTokenCount() 
        => _chatInputTokenCount;

    /// <summary>
    /// Gets the total number of chat output tokens recorded.
    /// </summary>
    public static int GetChatOutputTokenCount() 
        => _chatOutputTokenCount;

    /// <summary>
    /// Gets the total number of audio input characters recorded.
    /// </summary>
    public static int GetAudioInputCharacters() 
        => _audioInputCharacters;

    /// <summary>
    /// Adds to the total count of chat input tokens.
    /// </summary>
    /// <param name="count">The number of input tokens to add.</param>
    public static void AddChatInputTokenCount(int count)
        => Interlocked.Add(ref _chatInputTokenCount, count);

    /// <summary>
    /// Adds to the total count of chat output tokens.
    /// </summary>
    /// <param name="count">The number of output tokens to add.</param>
    public static void AddChatOutputTokenCount(int count)
        => Interlocked.Add(ref _chatOutputTokenCount, count);

    /// <summary>
    /// Adds to the total count of audio input characters.
    /// </summary>
    /// <param name="count">The number of audio input characters to add.</param>
    public static void AddAudioInputCharacters(int count)
        => Interlocked.Add(ref _audioInputCharacters, count);
}
