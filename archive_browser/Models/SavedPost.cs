namespace archive_browser.Models;

public class SavedPost
{
    public required int Id { get; init; }
    public required int PostId { get; init; }
}

public class SavedPostWithDetails
{
    public required int Id { get; init; }
    public required int UserId { get; init; }
    public required int BoardId { get; init; }
    public required int PostId { get; init; }
    public required int TopicId { get; init; }
    public required string Title { get; init; }
    public required DateTimeOffset Date { get; init; }
    public required string username { get; init; }
}