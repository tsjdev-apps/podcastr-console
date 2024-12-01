namespace Podcastr.Exceptions;

/// <summary>
///     Custom exception for handling errors related to 
///     Azure OpenAI services.
/// </summary>
/// <remarks>
///     Initializes a new instance of the 
///     <see cref="AzureOpenAIException"/> class.
/// </remarks>
/// <param name="message">The error message.</param>
/// <param name="innerException">The inner exception.</param>
public class AzureOpenAIException(
    string? message, 
    Exception? innerException) 
    : Exception(message, innerException)
{
}
