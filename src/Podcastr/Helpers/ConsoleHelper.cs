using Spectre.Console;

namespace Podcastr.Helpers;

/// <summary>
///     Helper class for creating interactive console 
///     UI with Spectre.Console.
/// </summary>
internal static class ConsoleHelper
{
    /// <summary>
    ///     Displays a styled header with a 
    ///     title and description.
    /// </summary>
    public static void ShowHeader()
    {
        // Clear the console for a clean display
        AnsiConsole.Clear();

        // Create a grid layout for the header
        Grid grid = new();
        grid.AddColumn();

        // Add the main title with Figlet text style
        grid.AddRow(
            new FigletText("Podcastr").Centered().Color(Color.Red));

        // Add a description below the title
        grid.AddRow(
            Align.Center(
                new Panel("[red]Sample by Thomas Sebastian Jensen ([link]https://www.tsjdev-apps.de[/])[/]")));

        // Render the grid to the console
        AnsiConsole.Write(grid);
        AnsiConsole.WriteLine();
    }

    /// <summary>
    ///     Prompts the user to select from a list of options.
    /// </summary>
    /// <param name="options">The list of options to display.</param>
    /// <param name="prompt">The prompt message for the selection.</param>
    /// <param name="showHeader">Whether to display the header before the prompt.</param>
    /// <returns>The selected option as a string.</returns>
    public static string SelectFromOptions(
        List<string> options,
        string prompt,
        bool showHeader = true)
    {
        if (showHeader)
        {
            ShowHeader();
        }

        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(prompt)
                .AddChoices(options));
    }

    /// <summary>
    ///     Prompts the user to input a valid URL with validation.
    /// </summary>
    /// <param name="prompt">The prompt message for the input.</param>
    /// <param name="showHeader">Whether to display the header before the prompt.</param>
    /// <returns>The validated URL as a string.</returns>
    public static string GetUrlFromConsole(
        string prompt,
        bool showHeader = true)
    {
        if (showHeader)
        {
            ShowHeader();
        }

        return AnsiConsole.Prompt(
            new TextPrompt<string>(prompt)
                .PromptStyle("white")
                .ValidationErrorMessage("[red]Invalid prompt[/]")
                .Validate(input =>
                {
                    // Check if the input length is within valid range
                    if (input.Length < 3)
                    {
                        return ValidationResult.Error("[red]URL too short[/]");
                    }

                    if (input.Length > 250)
                    {
                        return ValidationResult.Error("[red]URL too long[/]");
                    }

                    // Validate if the input is a proper HTTPS URL
                    if (Uri.TryCreate(input, UriKind.Absolute, out Uri? uri)
                        && uri.Scheme == Uri.UriSchemeHttps)
                    {
                        return ValidationResult.Success();
                    }

                    return ValidationResult.Error("[red]No valid URL[/]");
                }));
    }

    /// <summary>
    ///     Prompts the user to input a string with optional length validation.
    /// </summary>
    /// <param name="prompt">The prompt message for the input.</param>
    /// <param name="validateLength">Whether to validate the length of the input.</param>
    /// <param name="showHeader">Whether to display the header before the prompt.</param>
    /// <returns>The validated string input.</returns>
    public static string GetStringFromConsole(
        string prompt,
        bool validateLength = true,
        bool showHeader = true)
    {
        if (showHeader)
        {
            ShowHeader();
        }

        return AnsiConsole.Prompt(
            new TextPrompt<string>(prompt)
                .PromptStyle("white")
                .ValidationErrorMessage("[red]Invalid prompt[/]")
                .Validate(input =>
                {
                    // Validate minimum length
                    if (input.Length < 3)
                    {
                        return ValidationResult.Error("[red]Value too short[/]");
                    }

                    // Validate maximum length if enabled
                    if (validateLength && input.Length > 200)
                    {
                        return ValidationResult.Error("[red]Value too long[/]");
                    }

                    return ValidationResult.Success();
                }));
    }

    /// <summary>
    ///     Prompts the user to confirm a choice with a yes/no question.
    /// </summary>
    /// <param name="prompt">The confirmation prompt.</param>
    /// <param name="defaultValue">The default value if no input is given.</param>
    /// <returns>True if the user confirms; otherwise, false.</returns>
    public static bool GetConfirmation(
        string prompt,
        bool defaultValue)
    {
        WriteMessage(string.Empty);

        return AnsiConsole.Confirm(
            prompt,
            defaultValue);
    }

    /// <summary>
    ///     Displays an error message in red text.
    /// </summary>
    /// <param name="errorMessage">The error message to display.</param>
    public static void WriteError(
        string errorMessage)
    {
        AnsiConsole.MarkupLine($"[red]{errorMessage}[/]");
    }

    /// <summary>
    ///     Displays a standard message in white text.
    /// </summary>
    /// <param name="message">The message to display.</param>
    public static void WriteMessage(
        string message)
    {
        AnsiConsole.MarkupLine($"[white]{message}[/]");
    }

    public static void RenderTokenUsageTable(bool isImageGenerated)
    {
        AnsiConsole.WriteLine();

        // Create a table
        var table = new Table();

        // Add some columns
        table.AddColumn("");
        table.AddColumn(new TableColumn("Count").Centered());
        table.AddColumn(new TableColumn("approx. Price").LeftAligned());

        var inputPrice = Math.Round(TokenUsageHelper.GetChatInputTokenCount() / 1000 * 0.00250, 2);
        var outputPrice = Math.Round(TokenUsageHelper.GetChatOutputTokenCount() / 1000 * 0.01000, 2);
        var audioPrice = Math.Round(TokenUsageHelper.GetAudioInputCharacters() / 1000 * 0.015, 2);
        var imagePrice = Math.Round(isImageGenerated ? 0.040 : 0, 2);

        // Add some rows
        table.AddRow(
            "Chat Input", 
            $"[yellow]{TokenUsageHelper.GetChatInputTokenCount()}[/]", 
            $"${inputPrice} with GPT-4o-Mini");
        table.AddRow(
            "Chat Output", 
            $"[yellow]{TokenUsageHelper.GetChatOutputTokenCount()}[/]", 
            $"${outputPrice} with GPT-4o-Mini");
        table.AddRow(
            "Audio", 
            $"[yellow]{TokenUsageHelper.GetAudioInputCharacters()}[/]", 
            $"${audioPrice} with TTS");
        table.AddRow(
            "Image", 
            $"[yellow]{(isImageGenerated ? 1 : 0)}[/]",
            $"${imagePrice} with DALL-E-3 Standard");

        // Render the table to the console
        AnsiConsole.Write(table);

        AnsiConsole.WriteLine();
    }
}