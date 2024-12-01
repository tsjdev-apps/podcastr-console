using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace Podcastr.Helpers;

/// <summary>
///     Helper class for handling website-related operations, 
///     such as fetching and cleaning HTML content.
/// </summary>
internal static partial class WebsiteHelper
{
    // Regex for identifying and removing multiple line breaks
    [GeneratedRegex(@"(\r?\n)+")]
    private static partial Regex ExtraLineBreakRegex();

    /// <summary>
    ///     Fetches the HTML body of a website and returns 
    ///     it as a cleaned string.
    /// </summary>
    /// <param name="url">The URL of the website.</param>
    /// <returns>A cleaned version of the website's HTML body content.</returns>
    public static async Task<string> GetHtmlBodyAsync(string url)
    {
        try
        {
            // Initialize HttpClient for fetching website content
            using var httpClient = new HttpClient();

            // Get the raw HTML content as a string
            var response = await httpClient.GetStringAsync(url);

            // Load the HTML document using HtmlAgilityPack
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(response);

            // Select the <body> node from the document
            var bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//body");
            if (bodyNode == null)
            {
                ConsoleHelper.WriteError(
                    "The body tag could not be found in the provided HTML.");
                return string.Empty;
            }

            // Clean and return the inner text of the body
            return CleanHtmlBody(bodyNode.InnerText);
        }
        catch (Exception ex)
        {
            // Handle and log exceptions
            ConsoleHelper.WriteError(
                $"An error occurred while fetching HTML content: {ex.Message}");
            return string.Empty;
        }
    }

    /// <summary>
    /// Cleans the HTML body by removing extra whitespace and line breaks.
    /// </summary>
    /// <param name="htmlBody">The raw HTML body content as a string.</param>
    /// <returns>A cleaned version of the HTML body content.</returns>
    private static string CleanHtmlBody(string htmlBody)
    {
        // Return immediately if the input is null or empty
        if (string.IsNullOrWhiteSpace(htmlBody))
        {
            return htmlBody;
        }

        // Replace all additional whitespace with a single space
        htmlBody = ReplaceWhitespaces(htmlBody);

        // Replace multiple line breaks with a single line break
        htmlBody = ExtraLineBreakRegex().Replace(htmlBody, "\n");

        // Trim leading and trailing whitespace
        return htmlBody.Trim();
    }

    /// <summary>
    /// Replaces all consecutive whitespace characters with a single space.
    /// </summary>
    /// <param name="input">The string to process.</param>
    /// <returns>A string with reduced whitespace.</returns>
    private static string ReplaceWhitespaces(string input)
    {
        // Use Span<char> for efficient in-memory operations
        ReadOnlySpan<char> inputSpan = input.AsSpan();

        // Optimized memory allocation
        Span<char> resultSpan = stackalloc char[input.Length];

        int resultIndex = 0;
        bool lastWasWhitespace = false;

        foreach (char c in inputSpan)
        {
            // Check for any whitespace character
            if (char.IsWhiteSpace(c))
            {
                // Add a single space if not repeating
                if (!lastWasWhitespace)
                {
                    resultSpan[resultIndex++] = ' ';
                    lastWasWhitespace = true;
                }
            }
            else
            {
                // Add non-whitespace character
                resultSpan[resultIndex++] = c;
                lastWasWhitespace = false;
            }
        }

        // Convert Span<char> to string
        return new string(resultSpan[..resultIndex]);
    }
}
