namespace old_forum.Models;

public class Topic
{
    public required int Id { get; init; }
    public required int UserId { get; set; }
    public required int BoardId { get; set; }
    public required string Title { get; set; }
    public required DateTimeOffset Date { get; set; }
}

public class TopicWithUserTotal : Topic
{
    public required string username { get; init; }
    public required int totalRows { get; init; }
}