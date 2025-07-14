using Dapper;
using old_forum.Db;
using old_forum.Models;

namespace old_forum.Repos;

public class PostRepository(IDbConnectionFactory connectionFactory)
{
    public async Task<IEnumerable<Post>> GetByTopicId(int topicId)
    {
        using var dbConnection = await connectionFactory.CreateConnectionAsync();
        return await dbConnection.QueryAsync<Post>(
            $"""
             SELECT p.id, p.user_id, u.username, p.title, p.date, p.content
             FROM posts p
             INNER JOIN users u ON p.user_id = u.id
             WHERE topic_id={topicId} 
             ORDER BY date ASC
             """);
    }
}
