namespace Podcastr.Helpers;

/// <summary>
///     Helper class to track and manage token usage and 
///     audio input characters.
/// </summary>
public static class TokenUsageHelper
{
    private static int _chatInputTokenCount = 0;
    private static int _chatOutputTokenCount = 0;
    private static int _audioInputCharacters = 0;

    /// <summary>
    ///     Resets the token counts and audio input characters to zero.
    /// </summary>
    public static void Reset()
    {
        _chatInputTokenCount = 0;
        _chatOutputTokenCount = 0;
        _audioInputCharacters = 0;
    }

    /// <summary>
    ///     Gets the count of chat input tokens.
    /// </summary>
    /// <returns>The count of chat input tokens.</returns>
    public static int GetChatInputTokenCount()
        => _chatInputTokenCount;

    /// <summary>
    ///     Gets the count of chat output tokens.
    /// </summary>
    /// <returns>The count of chat output tokens.</returns>
    public static int GetChatOutputTokenCount()
        => _chatOutputTokenCount;

    /// <summary>
    ///     Gets the count of audio input characters.
    /// </summary>
    /// <returns>The count of audio input characters.</returns>
    public static int GetAudioInputCharacters()
        => _audioInputCharacters;

    /// <summary>
    ///     Adds to the count of chat input tokens.
    /// </summary>
    /// <param name="chatInputTokenCount">The number of chat input tokens to add.</param>
    public static void AddChatInputTokenCount(int chatInputTokenCount)
    {
        _chatInputTokenCount += chatInputTokenCount;
    }

    /// <summary>
    ///     Adds to the count of chat output tokens.
    /// </summary>
    /// <param name="chatOutputTokenCount">The number of chat output tokens to add.</param>
    public static void AddChatOutputTokenCount(int chatOutputTokenCount)
    {
        _chatOutputTokenCount += chatOutputTokenCount;
    }

    /// <summary>
    ///     Adds to the count of audio input characters.
    /// </summary>
    /// <param name="audioInputCharacters">The number of audio input characters to add.</param>
    public static void AddAudioInputCharacters(int audioInputCharacters)
    {
        _audioInputCharacters += audioInputCharacters;
    }
}
