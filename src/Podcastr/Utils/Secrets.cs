namespace Podcastr.Utils;

/// <summary>
///     Utility class for accessing secrets such as API keys and endpoints.
/// </summary>
internal static class Secrets
{
    /// <summary>
    ///     The Azure OpenAI endpoint. Retrieved from environment variables.
    /// </summary>
    public const string? AzureOpenAIEndpoint 
        = null;

    /// <summary>
    ///     The Azure OpenAI API key. Retrieved from environment variables.
    /// </summary>
    public const string? AzureOpenAIKey 
        = null;

    /// <summary>
    ///     The name of the Azure OpenAI chat model.
    /// </summary>
    public const string? AzureOpenAIChatModelName 
        = null;

    /// <summary>
    ///     The name of the Azure OpenAI audio model.
    /// </summary>
    public const string? AzureOpenAIAudioModelName 
        = null;

    /// <summary>
    ///     The name of the Azure OpenAI image model.
    /// </summary>
    public const string? AzureOpenAIImageModelName 
        = null;
}
