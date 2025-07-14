using Dapper;
using old_forum.Db;
using old_forum.Models;

namespace old_forum.Repos;

public class UserRepository(IDbConnectionFactory connectionFactory)
{
    public async Task<IEnumerable<User>> GetAll()
    {
        using var dbConnection = await connectionFactory.CreateConnectionAsync();
        return await dbConnection.QueryAsync<User>("SELECT * FROM users");
    }
    
    public async Task<IEnumerable<User>> GetById(int id)
    {
        using var dbConnection = await connectionFactory.CreateConnectionAsync();
        return await dbConnection.QueryAsync<User>("SELECT * FROM users WHERE user_id=@id");
    }
}