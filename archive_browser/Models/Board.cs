using System.ComponentModel.DataAnnotations;

namespace archive_browser.Models;

public class Board
{
    public required int Id { get; init; }
    
    public required string Name { get; init; }
}