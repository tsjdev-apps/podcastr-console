namespace Podcastr.Models;

/// <summary>
///     Represents an element to be included in a ZIP archive, 
///     containing either text or binary content with an associated file name.
/// </summary>
/// <param name="TextContent">The text content of the file. 
///                           Null if the file is binary.</param>
/// <param name="ByteContent">The binary content of the file. 
///                           Null if the file is text.</param>
/// <param name="FileName">The name of the file in the ZIP archive.</param>
public record class ZipElement(
    string? TextContent,
    byte[]? ByteContent,
    string FileName);
