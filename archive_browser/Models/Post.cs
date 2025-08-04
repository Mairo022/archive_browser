namespace archive_browser.Models;

public class PostFull
{
    public required int Id { get; init; }
    public required int UserId { get; set; }
    public required int BoardId { get; set; }
    public required int TopicId { get; set; }
    public required string Title { get; set; }
    public required DateTimeOffset Date { get; set; }
    public required string Content { get; set; }
}

public class Post
{
    public required int Id { get; init; }
    public required int UserId { get; set; }
    public required string Username { get; set; }
    public required string Title { get; set; }
    public required DateTimeOffset Date { get; set; }
    public required string Content { get; set; }
}