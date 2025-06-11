using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace Podcastr.Helpers;

/// <summary>
/// Provides utility methods for retrieving and sanitizing website HTML content.
/// </summary>
internal static partial class WebsiteHelper
{
    // Regex to normalize multiple line breaks into one
    [GeneratedRegex(@"(\r?\n)+")]
    private static partial Regex ExtraLineBreakRegex();

    /// <summary>
    /// Downloads the HTML content from the specified URL and returns the cleaned body content.
    /// </summary>
    /// <param name="url">The full URL of the website to fetch.</param>
    /// <returns>
    /// A cleaned string containing the inner text of the HTML body,
    /// or an empty string if the request fails or no <body> tag is found.
    /// </returns>
    public static async Task<string> GetHtmlBodyAsync(
        string url)
    {
        try
        {
            using HttpClient httpClient = new();

            string html = await httpClient.GetStringAsync(url);

            HtmlDocument htmlDoc = new();
            htmlDoc.LoadHtml(html);

            HtmlNode? bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//body");

            if (bodyNode == null)
            {
                ConsoleHelper.WriteError("The <body> tag was not found.");
                return string.Empty;
            }

            return CleanHtmlBody(bodyNode.InnerText);
        }
        catch (HttpRequestException httpEx)
        {
            ConsoleHelper.WriteError($"HTTP error: {httpEx.Message}");
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError(
                $"An error occurred while processing the HTML: {ex.Message}");
        }

        return string.Empty;
    }

    /// <summary>
    /// Cleans up an HTML body string by removing excessive whitespace and line breaks.
    /// </summary>
    /// <param name="htmlBody">The raw inner text from the HTML body.</param>
    /// <returns>A trimmed and normalized version of the content.</returns>
    private static string CleanHtmlBody(string htmlBody)
    {
        if (string.IsNullOrWhiteSpace(htmlBody))
        {
            return string.Empty;
        }

        string normalizedWhitespace = ReplaceConsecutiveWhitespaceWithSingleSpace(htmlBody);
        return ExtraLineBreakRegex().Replace(normalizedWhitespace, "\n").Trim();
    }

    /// <summary>
    /// Replaces consecutive whitespace characters with a single space.
    /// </summary>
    /// <param name="input">The input string.</param>
    /// <returns>A cleaned string with no excessive spacing.</returns>
    private static string ReplaceConsecutiveWhitespaceWithSingleSpace(string input)
    {
        ReadOnlySpan<char> source = input.AsSpan();
        Span<char> result = stackalloc char[input.Length];
        int resultIndex = 0;
        bool lastWasWhitespace = false;

        foreach (char c in source)
        {
            if (char.IsWhiteSpace(c))
            {
                if (!lastWasWhitespace)
                {
                    result[resultIndex++] = ' ';
                    lastWasWhitespace = true;
                }
            }
            else
            {
                result[resultIndex++] = c;
                lastWasWhitespace = false;
            }
        }

        return new string(result[..resultIndex]);
    }
}
