namespace Podcastr.Helpers;

/// <summary>
///     Helper class for file operations, 
///     particularly for handling temporary files.
/// </summary>
internal static class FileHelper
{
    /// <summary>
    ///     Writes the provided byte array to a 
    ///     temporary folder as a ZIP file.
    /// </summary>
    /// <param name="fileBytes">The file content as a byte array. 
    ///                         Can be null.</param>
    /// <returns>The full path of the temporary file.</returns>
    public static async Task<string> WriteToTempFolderAsync(byte[]? fileBytes)
    {
        // Validate input
        if (fileBytes == null || fileBytes.Length == 0)
        {
            return string.Empty;
        }

        // Create a unique temporary file name with a .zip extension
        string tempFileName = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.zip");

        // Write the bytes to the temporary file
        await File.WriteAllBytesAsync(tempFileName, fileBytes);

        // Return the path of the created temporary file
        return tempFileName;
    }
}
