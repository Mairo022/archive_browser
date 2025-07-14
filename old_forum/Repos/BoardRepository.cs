using Dapper;
using old_forum.Db;
using old_forum.Models;

namespace old_forum.Repos;

public class BoardRepository(IDbConnectionFactory connectionFactory)
{
    public async Task<IEnumerable<Board>> GetAll()
    {
        using var dbConnection = await connectionFactory.CreateConnectionAsync();
        return await dbConnection.QueryAsync<Board>("SELECT * FROM boards");
    }
}