namespace Podcastr.Utils;

/// <summary>
///     Contains static constants and lists used across the application.
/// </summary>
internal static class Statics
{
    /// <summary>
    ///     List of supported podcast languages.
    /// </summary>
    public static readonly List<string> PodcastLanguages =
    [
        "German",
        "English",
        "French",
        "Spanish"
    ];

    /// <summary>
    ///     List of available podcast voice options.
    /// </summary>
    public static readonly List<string> PodcastVoices =
    [
        "Alloy",
        "Echo",
        "Fable",
        "Onyx",
        "Nova",
        "Shimmer"
    ];

    /// <summary>
    ///     Price per 1000 token for GPT-4o Mini input.
    /// </summary>
    public static readonly decimal Gpt4oMiniInputPrice
        = 0.000150m;
    /// <summary>
    ///     Price per 1000 token for GPT-4o Mini output.
    /// </summary>
    public static readonly decimal Gpt4oMiniOutputPrice
        = 0.000600m;

    /// <summary>
    ///     Price per 1000 token for GPT-4o input.
    /// </summary>
    public static readonly decimal Gpt4oInputPrice
        = 0.00250m;
    /// <summary>
    ///     Price per 1000 token for GPT-4o output.
    /// </summary>
    public static readonly decimal Gpt4oOutputPrice
        = 0.01000m;

    /// <summary>
    ///     Price per 1000 token for GPT-4 Turbo input.
    /// </summary>
    public static readonly decimal Gpt4TurboInputPrice
        = 0.0100m;
    /// <summary>
    ///     Price per 1000 token for GPT-4 Turbo output.
    /// </summary>
    public static readonly decimal Gpt4TurboOutputPrice
        = 0.03000m;

    /// <summary>
    ///     Price per 1000 token for GPT-4 input.
    /// </summary>
    public static readonly decimal Gpt4InputPrice
        = 0.0300m;
    /// <summary>
    ///     Price per 1000 token for GPT-4 output.
    /// </summary>
    public static readonly decimal Gpt4OutputPrice
        = 0.0600m;

    /// <summary>
    ///     Price per 1000 characters for TTS.
    /// </summary>
    public static readonly decimal TTSPrice
        = 0.015m;
    /// <summary>
    ///     Price per 1000 characters for TTS.
    /// </summary>
    public static readonly decimal TTSHDPrice
        = 0.030m;

    /// <summary>
    ///     Price per image for DALL-E-3 Standard.
    /// </summary>
    public static readonly decimal DallE3StandardPrice
        = 0.040m;
    /// <summary>
    ///     Price per image for DALL-E-3 HD.
    /// </summary>
    public static readonly decimal DallE3HDPrice
        = 0.080m;

    /// <summary>
    ///     Prompt for generating a podcast script based on input text.
    /// </summary>
    public const string PodcastPrompt =
        "Please create an engaging and captivating podcast from the following text with the title {0}. The podcast should be written in {1} and have a maximum reading duration of 5 minutes. It should include a brief, compelling introduction to the topic, followed by a clear and accessible presentation of the main content. The tone should be entertaining and aimed at a broad audience. Avoid stage directions or headings and focus directly on the podcast content. Please create also a concise and engaging description of the podcast based on the provided script. The description should briefly summarize the key topic, appeal to a broad audience, and be suitable for podcast directories. Create finally engaging social media posts based on the provided podcast script. The posts should be creative, captivating, and concise, while incorporating humor or emotion depending on the context. Use emojis effectively to enhance the tone and convey the message. The content must match the language of the original script (e.g., German or English). For LinkedIn, craft a professional yet personal post that highlights key takeaways, insights, or thought-provoking questions. Keep it under 280 words and include at least three relevant emojis. For Twitter (X), write a short and snappy post that grabs attention, staying within the 280-character limit. Use emojis strategically to draw attention. For Facebook, adopt a storytelling approach with a conversational tone, encouraging community interaction. This post can be up to 500 words and should creatively incorporate emojis to match the mood. Each platform’s unique audience and tone should be considered. Include a clear call-to-action when appropriate, such as 'Join the discussion!' or 'Tune in now!'. Here is the content: {2}";

    /// <summary>
    ///     Prompt for generating an image description for creating a podcast cover.
    /// </summary>
    public const string PodcastCoverPreparationPrompt =
        "Please transform the following text into an image description that adheres to safety guidelines. The description should be neutral, factual, and precise. Avoid any controversial or sensitive topics and ensure cultural respect is maintained. Any depiction of violence, including defensive or protective actions, is prohibited. Do not include the names of characters or real-life individuals in the description, and avoid using the word 'exotic'. The description should be written in English and serve as the basis for creating an appealing podcast cover. Here is the text: {0}";
}
