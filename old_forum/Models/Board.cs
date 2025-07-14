using System.ComponentModel.DataAnnotations;

namespace old_forum.Models;

public class Board
{
    public required int Id { get; init; }
    
    public required string Name { get; init; }
}