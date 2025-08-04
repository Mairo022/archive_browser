using archive_browser.Db;
using archive_browser.Models;
using Dapper;

namespace archive_browser.Repos;

public class SavedPostsRepository(IDbConnectionFactory connectionFactory)
{
    public async Task<IEnumerable<SavedPost>> GetAll()
    {
        using var dbConnection = await connectionFactory.CreateConnectionAsync();
        return await dbConnection.QueryAsync<SavedPost>("SELECT * FROM saved_posts");
    }

    // SavedPostWithDetails
    public async Task<IEnumerable<SavedPostWithDetails>> GetAllWithDetails()
    {
        using var dbConnection = await connectionFactory.CreateConnectionAsync();
        
        var data = await dbConnection.QueryAsync<SavedPostWithDetails>(
            $"""
             SELECT 
                 sp.id, 
                 u.id as UserId, 
                 p.board_id as BoardId,
                 sp.post_id as postId, 
                 t.id as topicId, 
                 t.title, 
                 p.date, 
                 u.username
             FROM saved_posts sp
             INNER JOIN posts p ON sp.post_id = p.id
             INNER JOIN users u ON p.user_id = u.id
             INNER JOIN topics t ON p.topic_id = t.id
             ORDER BY sp.id DESC
             """);

        return data;
    }

    public async Task<bool> InsertOne(int postId)
    {
        using var dbConnection = await connectionFactory.CreateConnectionAsync();

        var result = await dbConnection.ExecuteAsync($"INSERT INTO saved_posts (post_id) VALUES ({postId})");

        return result == 1;
    }
    
    public async Task<bool> DeleteOne(int savedPostId)
    {
        using var dbConnection = await connectionFactory.CreateConnectionAsync();

        var result = await dbConnection.ExecuteAsync($"DELETE FROM saved_posts WHERE id={savedPostId}");

        return result == 1;
    }
}