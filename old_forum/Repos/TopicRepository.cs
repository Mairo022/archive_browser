using Dapper;
using old_forum.Db;
using old_forum.Models;

namespace old_forum.Repos;

public class TopicRepository(IDbConnectionFactory connectionFactory)
{
    public async Task<IEnumerable<TopicWithUserTotal>> Get(int boardId, int offset, int limit)
    {
        using var dbConnection = await connectionFactory.CreateConnectionAsync();
        
        var data =  await dbConnection.QueryAsync<TopicWithUserTotal>(
            $"""
             SELECT 
                 t.id as Id, t.title as Title, t.date as Date, t.board_id as BoardId, t.user_id as UserId, 
                 u.username as username,
                 COUNT(*) OVER() AS totalRows
             FROM public.topics t
             INNER JOIN users u ON t.user_id = u.id
             WHERE t.board_id = {boardId}
             ORDER BY t.date ASC
             LIMIT {limit} OFFSET {offset}
             """);
        
        return data;
    }
}