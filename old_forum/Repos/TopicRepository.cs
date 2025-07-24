using Dapper;
using old_forum.Db;
using old_forum.Models;
using old_forum.Pages;

namespace old_forum.Repos;

public class TopicRepository(IDbConnectionFactory connectionFactory)
{
    public async Task<IEnumerable<TopicWithUserTotal>> Get(int boardId, int offset, int limit,
        string? start, string? author, string? topic, Order order)
    {
        using var dbConnection = await connectionFactory.CreateConnectionAsync();
        
        var condition = $"t.board_id = {boardId}";
        
        if (!string.IsNullOrEmpty(start))
        {
            condition += $" AND t.date >= '{start}'";
            order = Order.ASC;
        }
        
        if (!string.IsNullOrEmpty(author)) condition += $" AND u.username = '{author}'";
        if (!string.IsNullOrEmpty(topic)) condition += $" AND t.title ~* '{topic}'";
        
        var data =  await dbConnection.QueryAsync<TopicWithUserTotal>(
            $"""
             SELECT 
                 t.id as Id, 
                 t.title as Title, 
                 t.date as Date, 
                 t.board_id as BoardId, 
                 t.user_id as UserId,
                 t.total_posts as TotalPosts,
                 u.username as username,
                 COUNT(*) OVER() AS totalRows
             FROM public.topics t
             INNER JOIN users u ON t.user_id = u.id
             WHERE {condition}
             ORDER BY t.date {order.ToString()}
             LIMIT {limit} OFFSET {offset}
             """);
        
        return data;
    }

    public async Task<IEnumerable<TopicWithUserTotal>> GetByPoster(int boardId, int offset, int limit,
        string? start, string? author, string? topic, Order order, string poster)
    {
        if (string.IsNullOrEmpty(poster)) return [];
        
        var where = $"u.username = '{poster}' AND t.board_id = {boardId}";
        
        if (!string.IsNullOrEmpty(start))
        {
            where += $" AND t.date >= '{start}'";
            order = Order.ASC;
        }
        
        if (!string.IsNullOrEmpty(author)) where += $" AND t_author.username = '{author}'";
        if (!string.IsNullOrEmpty(topic)) where += $" AND t.title ~* '{topic}'";
        
        using var dbConnection = await connectionFactory.CreateConnectionAsync();
        
        var data =  await dbConnection.QueryAsync<TopicWithUserTotal>(
            $"""
             SELECT 
                 t.id as Id, 
                 t.title as Title, 
                 t.date as Date, 
                 t.board_id as BoardId, 
                 t.user_id as UserId,
                 t.total_posts as TotalPosts,
                 t_author.username as username,
                 COUNT(*) OVER() AS totalRows
             FROM public.users u
             INNER JOIN posts p ON p.user_id = u.id
             INNER JOIN topics t ON t.id = p.topic_id
             INNER JOIN users t_author ON t.user_id = t_author.id
             WHERE {where}
             GROUP BY p.title, t.id, t_author.username
             ORDER BY t.date {order.ToString()}
             LIMIT {limit} OFFSET {offset}
             """);

        return data;
    }
}