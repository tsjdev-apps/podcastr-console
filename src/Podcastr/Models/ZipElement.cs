namespace Podcastr.Models;

/// <summary>
/// Represents a file to be included in a ZIP archive. 
/// The file can contain either text or binary content.
/// </summary>
/// <param name="TextContent">
/// The text content of the file (used for plain text files). 
/// Must be <c>null</c> if <paramref name="ByteContent"/> is provided.
/// </param>
/// <param name="ByteContent">
/// The binary content of the file (used for media or binary files). 
/// Must be <c>null</c> if <paramref name="TextContent"/> is provided.
/// </param>
/// <param name="FileName">The name of the file as it should 
/// appear in the ZIP archive.</param>
public record class ZipElement(
    string? TextContent,
    byte[]? ByteContent,
    string FileName);
