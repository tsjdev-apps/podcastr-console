using Podcastr.Models;
using Podcastr.Utils;
using Spectre.Console;

namespace Podcastr.Helpers;

/// <summary>
/// Provides methods for displaying usage cost tables in the console.
/// </summary>
internal static class TableHelper
{
    /// <summary>
    /// Displays cost breakdown tables for chat, audio, 
    /// and (optionally) image generation.
    /// </summary>
    /// <param name="includeImageCosts">Indicates whether 
    /// image generation costs should be shown.</param>
    public static void ShowTable(bool includeImageCosts)
    {
        // Retrieve usage metrics
        int chatInputTokens = TokenUsageHelper.GetChatInputTokenCount();
        int chatOutputTokens = TokenUsageHelper.GetChatOutputTokenCount();
        int audioInputChars = TokenUsageHelper.GetAudioInputCharacters();

        // CHAT TABLE
        List<TableColumn> chatColumns =
        [
            new TableColumn("Category").Centered(),
            new TableColumn("Count").Centered(),
            new TableColumn("GPT-4o Mini").LeftAligned(),
            new TableColumn("GPT-4o").LeftAligned(),
            new TableColumn("GPT-4 Turbo").LeftAligned(),
            new TableColumn("GPT-4").LeftAligned()
        ];

        List<Row> chatRows =
        [
            CreateChatRow("Chat Input", chatInputTokens),
            CreateChatRow("Chat Output", chatOutputTokens)
        ];

        // AUDIO TABLE
        List<TableColumn> audioColumns =
        [
            new TableColumn("Category").Centered(),
            new TableColumn("Count").Centered(),
            new TableColumn("TTS").LeftAligned(),
            new TableColumn("TTS-HD").LeftAligned()
        ];

        List<Row> audioRows =
        [
            new(
                "Audio",
                $"[yellow]{FormatNumber(audioInputChars)}[/]",
                FormatCost(audioInputChars / 1000m * Statics.TTSPrice),
                FormatCost(audioInputChars / 1000m * Statics.TTSHDPrice))
        ];

        // IMAGE TABLE (only if generated)
        Table? imageTable = null;
        if (includeImageCosts)
        {
            List<TableColumn> imageColumns =
            [
                new TableColumn("Category").Centered(),
                new TableColumn("Count").Centered(),
                new TableColumn("DALL-E-3 Standard").LeftAligned(),
                new TableColumn("DALL-E-3 HD").LeftAligned()
            ];

            List<Row> imageRows =
            [
                new(
                    "Image",
                    "[yellow]1[/]",
                    FormatCost(Statics.DallE3StandardPrice),
                    FormatCost(Statics.DallE3HDPrice))
            ];

            imageTable = CreateStyledTable("Image Costs", imageColumns, imageRows);
        }

        // Render all tables
        AnsiConsole.WriteLine();
        AnsiConsole.Write(CreateStyledTable("Chat Costs", chatColumns, chatRows));
        AnsiConsole.WriteLine();
        AnsiConsole.Write(CreateStyledTable("Audio Costs", audioColumns, audioRows));

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
    /// Creates a chat-related cost row with token count and model costs.
    /// </summary>
    private static Row CreateChatRow(string label, int tokens) =>
        new(
            label,
            $"[yellow]{FormatNumber(tokens)}[/]",
            FormatCost(tokens / 1000m * Statics.Gpt4oMiniInputPrice),
            FormatCost(tokens / 1000m * Statics.Gpt4oInputPrice),
            FormatCost(tokens / 1000m * Statics.Gpt4TurboInputPrice),
            FormatCost(tokens / 1000m * Statics.Gpt4InputPrice)
        );

    /// <summary>
    /// Formats a cost value as a currency string (USD).
    /// </summary>
    private static string FormatCost(decimal cost)
        => $"${cost:F2}";

    /// <summary>
    /// Formats a number with thousand separators.
    /// </summary>
    private static string FormatNumber(int number)
        => number.ToString("N0");
}
