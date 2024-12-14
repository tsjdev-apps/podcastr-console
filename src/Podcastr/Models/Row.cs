namespace Podcastr.Models;

internal class Row(params string[] cells)
{
    public List<string> Cells { get; } 
        = new List<string>(cells);
}
