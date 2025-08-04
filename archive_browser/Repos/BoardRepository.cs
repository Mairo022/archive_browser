using archive_browser.Db;
using archive_browser.Models;
using Dapper;

namespace archive_browser.Repos;

public class BoardRepository(IDbConnectionFactory connectionFactory)
{
    public async Task<IEnumerable<Board>> GetAll()
    {
        using var dbConnection = await connectionFactory.CreateConnectionAsync();
        return await dbConnection.QueryAsync<Board>("SELECT * FROM boards");
    }
}