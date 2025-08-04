using archive_browser.Db;
using archive_browser.Models;
using Dapper;

namespace archive_browser.Repos;

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