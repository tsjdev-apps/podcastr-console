namespace Podcastr.Exceptions;

/// <summary>
/// Represents errors that occur during operations with Azure OpenAI services.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="AzureOpenAIException"/> class 
/// with a specified error message and a reference to the inner exception that is the cause of this exception.
/// </remarks>
/// <param name="message">The message that describes the error.</param>
/// <param name="innerException">
/// The exception that is the cause of the current exception, or <c>null</c> if no inner exception is specified.
/// </param>
public class AzureOpenAIException(
    string? message,
    Exception? innerException) : Exception(message, innerException);