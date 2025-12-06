using Podcastr.Models;
using Podcastr.Utils;
using Spectre.Console;

namespace Podcastr.Helpers;

/// <summary>
/// Provides methods for displaying usage metrics in the console.
/// </summary>
internal static class TableHelper
{
    /// <summary>
    /// Displays usage metrics for chat, audio, and (optionally) image generation.
    /// </summary>
    /// <param name="includeImageMetrics">Indicates whether 
    /// image generation metrics should be shown.</param>
    public static void ShowTable(bool includeImageMetrics)
    {
        // Retrieve usage metrics
        int chatInputTokens = TokenUsageHelper.GetChatInputTokenCount();
        int chatOutputTokens = TokenUsageHelper.GetChatOutputTokenCount();
        int audioInputChars = TokenUsageHelper.GetAudioInputCharacters();

        // CHAT TABLE
        List<TableColumn> chatColumns =
        [
            new TableColumn("Category").Centered(),
            new TableColumn("Count").Centered()
        ];

        List<Row> chatRows =
        [
            new(
                "Chat Input Tokens",
                $"[yellow]{FormatNumber(chatInputTokens)}[/]"
            ),
            new(
                "Chat Output Tokens",
                $"[yellow]{FormatNumber(chatOutputTokens)}[/]"
            )
        ];

        // AUDIO TABLE
        List<TableColumn> audioColumns =
        [
            new TableColumn("Category").Centered(),
            new TableColumn("Count").Centered()
        ];

        List<Row> audioRows =
        [
            new(
                "Audio Characters",
                $"[yellow]{FormatNumber(audioInputChars)}[/]"
            )
        ];

        // IMAGE TABLE (only if generated)
        Table? imageTable = null;
        if (includeImageMetrics)
        {
            List<TableColumn> imageColumns =
            [
                new TableColumn("Category").Centered(),
                new TableColumn("Count").Centered()
            ];

            List<Row> imageRows =
            [
                new(
                    "Images Generated",
                    "[yellow]1[/]"
                )
            ];

            imageTable = CreateStyledTable("Image Metrics", imageColumns, imageRows);
        }

        // Render all tables
        AnsiConsole.WriteLine();
        AnsiConsole.Write(CreateStyledTable("Chat Metrics", chatColumns, chatRows));
        AnsiConsole.WriteLine();
        AnsiConsole.Write(CreateStyledTable("Audio Metrics", audioColumns, audioRows));

        if (imageTable is not null)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.Write(imageTable);
        }

        AnsiConsole.WriteLine();
    }

    /// <summary>
    /// Builds a reusable, styled table with specified columns and rows.
    /// </summary>
    private static Table CreateStyledTable(string title, List<TableColumn> columns, List<Row> rows)
    {
        Table table = new Table()
            .Title($"[bold underline blue]{title}[/]")
            .Border(TableBorder.Rounded)
            .Expand();

        foreach (TableColumn column in columns)
        {
            table.AddColumn(column);
        }

        foreach (Row row in rows)
        {
            table.AddRow(row.Cells.ToArray());
        }

        return table;
    }

    /// <summary>
    /// Formats a number with thousand separators.
    /// </summary>
    private static string FormatNumber(int number)
        => number.ToString("N0");
}
