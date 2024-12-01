using Podcastr.Models;
using System.IO.Compression;
using System.Text;

namespace Podcastr.Helpers;

/// <summary>
///     Helper class for creating ZIP archives 
///     from text or byte content.
/// </summary>
internal static class ZipArchiveHelper
{
    /// <summary>
    ///     Creates a ZIP archive containing the provided elements.
    /// </summary>
    /// <param name="zipElements">A collection of elements to 
    ///                           include in the ZIP archive.</param>
    /// <returns>A byte array representing the ZIP archive.</returns>
    public static byte[] CreateZipArchive(
        IEnumerable<ZipElement> zipElements)
    {
        if (zipElements is null)
        {
            ConsoleHelper.WriteError("ZIP elements cannot be null.");
            return [];
        }

        // Create a memory stream to store the ZIP file
        using MemoryStream memoryStream = new();

        // Create the ZIP archive
        using (ZipArchive zipArchive =
            new(memoryStream, ZipArchiveMode.Create, leaveOpen: true))
        {
            foreach (ZipElement zipElement in zipElements)
            {
                // Validate file name
                if (string.IsNullOrWhiteSpace(zipElement.FileName))
                {
                    Console.WriteLine(
                        "FileName cannot be null or empty in a ZIP element.");
                    continue;
                }

                // Add text content if present
                if (!string.IsNullOrWhiteSpace(zipElement.TextContent))
                {
                    CreateTextZipEntry(
                        zipArchive,
                        zipElement.FileName,
                        zipElement.TextContent);
                }
                // Add byte content if present
                else if (zipElement.ByteContent is not null
                    && zipElement.ByteContent.Length > 0)
                {
                    CreateByteArrayZipEntry(
                        zipArchive,
                        zipElement.FileName,
                        zipElement.ByteContent);
                }
            }
        }

        // Reset the memory stream position and
        // return the ZIP file as a byte array
        memoryStream.Seek(0, SeekOrigin.Begin);
        return memoryStream.ToArray();
    }

    /// <summary>
    ///     Adds a text file entry to the ZIP archive.
    /// </summary>
    /// <param name="zipArchive">The ZIP archive to modify.</param>
    /// <param name="fileName">The name of the text file entry.</param>
    /// <param name="content">The text content to include.</param>
    private static void CreateTextZipEntry(
        ZipArchive zipArchive,
        string fileName,
        string content)
    {
        // Create a new entry for the text file
        ZipArchiveEntry entry = 
            zipArchive.CreateEntry(fileName);

        // Write the text content using a StreamWriter with UTF-8 encoding
        using StreamWriter writer = 
            new(entry.Open(), Encoding.UTF8);
        writer.Write(content);
    }

    /// <summary>
    /// A   dds a binary file entry to the ZIP archive.
    /// </summary>
    /// <param name="zipArchive">The ZIP archive to modify.</param>
    /// <param name="fileName">The name of the binary file entry.</param>
    /// <param name="content">The byte array content to include.</param>
    private static void CreateByteArrayZipEntry(
        ZipArchive zipArchive,
        string fileName,
        byte[] content)
    {
        // Create a new entry for the binary file
        ZipArchiveEntry entry = 
            zipArchive.CreateEntry(fileName);

        // Write the byte content directly to the entry's stream
        using Stream stream = 
            entry.Open();
        stream.Write(content, 0, content.Length);
    }
}
