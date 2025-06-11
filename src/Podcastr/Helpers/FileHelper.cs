namespace Podcastr.Helpers;

/// <summary>
/// Provides helper methods for file operations, 
/// such as writing content to temporary files.
/// </summary>
internal static class FileHelper
{
    /// <summary>
    /// Writes a byte array to a temporary file 
    /// with a <c>.zip</c> extension.
    /// </summary>
    /// <param name="fileBytes">The file content as a byte array. 
    /// Can be null or empty.</param>
    /// <returns>
    /// The full path of the created temporary file, or an empty string 
    /// if input is invalid or the operation fails.
    /// </returns>
    public static async Task<string> WriteToTempFolderAsync(byte[]? fileBytes)
    {
        if (fileBytes is null || fileBytes.Length == 0)
        {
            return string.Empty;
        }

        try
        {
            string tempFileName = Path.Combine(
                Path.GetTempPath(),
                $"{Guid.NewGuid()}.zip");

            await File.WriteAllBytesAsync(tempFileName, fileBytes);
            return tempFileName;
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"Failed to write temp file: {ex.Message}");
            return string.Empty;
        }
    }
}
