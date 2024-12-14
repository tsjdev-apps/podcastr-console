using Podcastr.Models;
using Podcastr.Utils;
using Spectre.Console;

namespace Podcastr.Helpers;

internal static class TableHelper
{
    public static void ShowTable(bool isImageGenerated)
    {
        // Retrieve token counts and audio/image generation flags
        int chatInputTokens = TokenUsageHelper.GetChatInputTokenCount();
        int chatOutputTokens = TokenUsageHelper.GetChatOutputTokenCount();
        int audioInputChars = TokenUsageHelper.GetAudioInputCharacters();

        // Define columns for Chat Table
        var chatColumns = new List<TableColumn>
        {
            new TableColumn("Category").Centered(),
            new TableColumn("Count").Centered(),
            new TableColumn("GPT-4o Mini").LeftAligned(),
            new TableColumn("GPT-4o").LeftAligned(),
            new TableColumn("GPT-4 Turbo").LeftAligned(),
            new TableColumn("GPT-4").LeftAligned()
        };

        // Define rows for Chat Table
        var chatRows = new List<Row>
        {
            new Row(
                "Chat Input",
                $"[yellow]{FormatNumber(chatInputTokens)}[/]",
                FormatCost(Math.Round(chatInputTokens / 1000m * Statics.Gpt4oMiniInputPrice, 2)),
                FormatCost(Math.Round(chatInputTokens / 1000m * Statics.Gpt4oInputPrice, 2)),
                FormatCost(Math.Round(chatInputTokens / 1000m * Statics.Gpt4TurboInputPrice, 2)),
                FormatCost(Math.Round(chatInputTokens / 1000m * Statics.Gpt4InputPrice, 2))
            ),
            new Row(
                "Chat Output",
                $"[yellow]{FormatNumber(chatOutputTokens)}[/]",
                FormatCost(Math.Round(chatOutputTokens / 1000m * Statics.Gpt4oMiniInputPrice, 2)),
                FormatCost(Math.Round(chatOutputTokens / 1000m * Statics.Gpt4oInputPrice, 2)),
                FormatCost(Math.Round(chatOutputTokens / 1000m * Statics.Gpt4TurboInputPrice, 2)),
                FormatCost(Math.Round(chatOutputTokens / 1000m * Statics.Gpt4InputPrice, 2))
            )
        };

        // Define columns for Audio Table
        var audioColumns = new List<TableColumn>
        {
            new TableColumn("Category").Centered(),
            new TableColumn("Count").Centered(),
            new TableColumn("TTS").LeftAligned(),
            new TableColumn("TTS-HD").LeftAligned()
        };

        // Define rows for Audio Table
        var audioRows = new List<Row>
        {
            new Row(
                "Audio",
                $"[yellow]{FormatNumber(audioInputChars)}[/]",
                FormatCost(Math.Round(audioInputChars / 1000m * Statics.TTSPrice, 2)),
                FormatCost(Math.Round(audioInputChars / 1000m * Statics.TTSHDPrice, 2))
            )
        };

        // Define columns for Image Table
        var imageColumns = new List<TableColumn>
        {
            new TableColumn("Category").Centered(),
            new TableColumn("Count").Centered(),
            new TableColumn("DALL-E-3 Standard").LeftAligned(),
            new TableColumn("DALL-E-3 HD").LeftAligned()
        };

        // Define rows for Image Table
        var imageRows = new List<Row>
        {
            new Row(
                "Image",
                $"[yellow]{(isImageGenerated ? 1 : 0)}[/]",
                FormatCost(isImageGenerated ? Statics.DallE3StandardPrice : 0),
                FormatCost(isImageGenerated ? Statics.DallE3HDPrice : 0)
            )
        };

        // Create tables
        var chatTable = CreateStyledTable("Chat Costs", chatColumns, chatRows);
        var audioTable = CreateStyledTable("Audio Costs", audioColumns, audioRows);
        var imageTable = CreateStyledTable("Image Costs", imageColumns, imageRows);

        // Render tables
        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine();
        AnsiConsole.Write(chatTable);
        AnsiConsole.WriteLine();
        AnsiConsole.Write(audioTable);
        AnsiConsole.WriteLine();
        AnsiConsole.Write(imageTable);
        AnsiConsole.WriteLine();
    }

    private static Table CreateStyledTable(string title, List<TableColumn> columns, List<Row> rows)
    {
        var table = new Table()
            .Title($"[bold underline blue]{title}[/]")
            .Border(TableBorder.Rounded)
            .Expand(); // Makes the table take available width

        // Add columns
        foreach (var column in columns)
        {
            table.AddColumn(column);
        }

        // Add rows
        foreach (var row in rows)
        {
            table.AddRow(row.Cells.ToArray());
        }

        return table;
    }

    // Helper function to format cost
    private static string FormatCost(decimal cost)
        => $"${cost:F2}";

    // Helper function to format numbers with thousand separators
    private static string FormatNumber(int number)
        => number.ToString("N0");
}
