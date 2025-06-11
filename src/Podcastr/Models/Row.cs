namespace Podcastr.Models;

/// <summary>
/// Represents a row of cell values used for rendering console tables or reports.
/// </summary>
/// <param name="cells">The cell values to include in the row.</param>
internal class Row(params string[] cells)
{
    /// <summary>
    /// Gets the list of cell values in the row.
    /// </summary>
    public List<string> Cells { get; }
        = [.. cells];
}
