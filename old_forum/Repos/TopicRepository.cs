using Dapper;
using old_forum.Db;
using old_forum.Models;

namespace old_forum.Repos;

public class TopicRepository(IDbConnectionFactory connectionFactory)
{
    public async Task<IEnumerable<TopicWithUserTotal>> Get(int boardId, int offset, int limit,
        string? start, string? author, string? poster, string? topic)
    {
        using var dbConnection = await connectionFactory.CreateConnectionAsync();

        var order = "DESC";
        var condition = $"t.board_id = {boardId}";
        
        if (!string.IsNullOrEmpty(start))
        {
            condition += $" AND t.date >= '{start}'";
            order = "ASC";
        }
        
        if (!string.IsNullOrEmpty(author)) condition += $" AND u.username = '{author}'";
        if (!string.IsNullOrEmpty(topic)) condition += $" AND t.title ~* '{topic}'";
        
        var data =  await dbConnection.QueryAsync<TopicWithUserTotal>(
            $"""
             SELECT 
                 t.id as Id, t.title as Title, t.date as Date, t.board_id as BoardId, t.user_id as UserId,
                 t.total_posts as TotalPosts,
                 u.username as username,
                 COUNT(*) OVER() AS totalRows
             FROM public.topics t
             INNER JOIN users u ON t.user_id = u.id
             WHERE {condition}
             ORDER BY t.date {order}
             LIMIT {limit} OFFSET {offset}
             """);
        
        return data;
    }
}