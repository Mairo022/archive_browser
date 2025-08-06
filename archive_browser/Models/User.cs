namespace archive_browser.Models;

public class User
{
    public required int Id { get; set; }
    public required string Username { get; set; }
}

public class UserInfo : User
{
    public required int TotalPosts { get; set; }
    public required DateTimeOffset LastPostTime { get; set; }
}