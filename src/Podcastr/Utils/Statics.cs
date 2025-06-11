namespace Podcastr.Utils;

/// <summary>
/// Contains static constants and reusable lists used across the application.
/// </summary>
internal static class Statics
{
    // -----------------------------
    // Supported Options
    // -----------------------------

    /// <summary>Supported podcast languages.</summary>
    public static readonly List<string> PodcastLanguages =
    [
        "German",
        "English",
        "French",
        "Spanish"
    ];

    /// <summary>Available podcast voice options.</summary>
    public static readonly List<string> PodcastVoices =
    [
        "Alloy",
        "Ash",
        "Coral",
        "Echo",
        "Fable",
        "Onyx",
        "Nova",
        "Sage",
        "Shimmer"
    ];

    // -----------------------------
    // Token Prices (per 1000 tokens)
    // -----------------------------

    public static readonly decimal Gpt41InputPrice = 0.002m;
    public static readonly decimal Gpt41OutputPrice = 0.008m;

    public static readonly decimal Gpt41MiniInputPrice = 0.0004m;
    public static readonly decimal Gpt41MiniOutputPrice = 0.0016m;

    public static readonly decimal Gpt41NanoInputPrice = 0.0001m;
    public static readonly decimal Gpt41NanoOutputPrice = 0.0004m;

    // -----------------------------
    // Audio Prices (per 1000 characters)
    // -----------------------------

    public static readonly decimal TTSPrice = 0.015m;
    public static readonly decimal TTSHDPrice = 0.030m;
    public static readonly decimal GPT4oMiniTTSPrice = 0.01m;
    public static readonly decimal GPT4oTTSPrice = 0.04m;

    // -----------------------------
    // Image Prices (per image)
    // -----------------------------

    public static readonly decimal DallE3StandardPrice = 0.040m;
    public static readonly decimal DallE3HDPrice = 0.080m;

    // -----------------------------
    // Prompts
    // -----------------------------

    /// <summary>
    /// Prompt template for generating a podcast script, description, and social media content.
    /// </summary>
    public const string PodcastPrompt =
        "Please create an engaging and captivating podcast from the following text with the title {0}. The podcast script should be written in {1} and have a maximum reading duration of 5 minutes. It should include a brief, compelling introduction to the topic, followed by a clear and accessible presentation of the main content. The tone should be entertaining and aimed at a broad audience. Avoid stage directions or headings and focus directly on the podcast content. Please create also a concise and engaging description of the podcast based on the provided script. The description should briefly summarize the key topic, appeal to a broad audience, and be suitable for podcast directories. Create finally engaging social media posts based on the provided podcast script. The posts should be creative, captivating, and concise, while incorporating humor or emotion depending on the context. Use emojis effectively to enhance the tone and convey the message. The content must match the language of the original script (e.g., German or English). For LinkedIn, craft a professional yet personal post that highlights key takeaways, insights, or thought-provoking questions. Keep it under 280 words and include at least three relevant emojis. For Twitter (X), write a short and snappy post that grabs attention, staying within the 280-character limit. Use emojis strategically to draw attention. For Facebook, adopt a storytelling approach with a conversational tone, encouraging community interaction. This post can be up to 500 words and should creatively incorporate emojis to match the mood. For Threads create a concise and engaging post under 300 characters. Keep it light, expressive, and in tune with the platform’s casual, community-driven vibe. Include a clear call-to-action when appropriate, such as 'Join the discussion!' or 'Tune in now!'. Here is the content: {2}";

    /// <summary>
    /// Prompt template for generating an image description suitable for a podcast cover.
    /// </summary>
    public const string PodcastCoverPreparationPrompt =
        "Please transform the following text into an image description that adheres to safety guidelines. The description should be neutral, factual, and precise. Avoid any controversial or sensitive topics and ensure cultural respect is maintained. Any depiction of violence, including defensive or protective actions, is prohibited. Do not include the names of characters or real-life individuals in the description, and avoid using the word 'exotic'. The description should be written in English and serve as the basis for creating an appealing podcast cover. Here is the text: {0}";
}
