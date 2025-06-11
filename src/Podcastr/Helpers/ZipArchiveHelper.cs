using Podcastr.Models;
using System.IO.Compression;
using System.Text;

namespace Podcastr.Helpers;

/// <summary>
/// Provides utility methods for creating ZIP archives 
/// from text and binary content.
/// </summary>
internal static class ZipArchiveHelper
{
    /// <summary>
    /// Creates a ZIP archive from the specified elements.
    /// </summary>
    /// <param name="zipElements">A collection of elements 
    /// to include in the ZIP archive.</param>
    /// <returns>A byte array representing the created ZIP archive. 
    /// Returns an empty array if input is null.</returns>
    public static byte[] CreateZipArchive(
        IEnumerable<ZipElement> zipElements)
    {
        if (zipElements is null)
        {
            ConsoleHelper.WriteError("ZIP elements cannot be null.");
            return [];
        }

        using MemoryStream memoryStream = new();

        using (ZipArchive zipArchive = new(
            memoryStream, 
            ZipArchiveMode.Create, 
            leaveOpen: true))
        {
            foreach (ZipElement element in zipElements)
            {
                if (string.IsNullOrWhiteSpace(element.FileName))
                {
                    ConsoleHelper.WriteError(
                        "ZIP element is missing a valid file name.");
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(element.TextContent))
                {
                    AddTextEntry(
                        zipArchive, element.FileName, element.TextContent);
                }
                else if (element.ByteContent is { Length: > 0 })
                {
                    AddBinaryEntry(
                        zipArchive, element.FileName, element.ByteContent);
                }
                else
                {
                    ConsoleHelper.WriteError(
                        $"ZIP element '{element.FileName}' has no content.");
                }
            }
        }

        memoryStream.Seek(0, SeekOrigin.Begin);
        return memoryStream.ToArray();
    }

    /// <summary>
    /// Adds a text file entry to the ZIP archive.
    /// </summary>
    private static void AddTextEntry(
        ZipArchive archive, 
        string fileName, 
        string content)
    {
        ZipArchiveEntry entry = archive.CreateEntry(fileName);
        using StreamWriter writer = new(entry.Open(), Encoding.UTF8);
        writer.Write(content);
    }

    /// <summary>
    /// Adds a binary file entry to the ZIP archive.
    /// </summary>
    private static void AddBinaryEntry(
        ZipArchive archive, 
        string fileName, 
        byte[] content)
    {
        ZipArchiveEntry entry = archive.CreateEntry(fileName);
        using Stream stream = entry.Open();
        stream.Write(content, 0, content.Length);
    }
}
