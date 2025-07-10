using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlClient;


namespace DataAccess.DbAccess;

public class SqlDataAccess : ISqlDataAccess
{
    private readonly IConfiguration _config;

    public SqlDataAccess(IConfiguration config)
    {
        _config = config;
    }

    public async Task<IEnumerable<T>> LoadData<T, U>(string storedProcedure, U parameters)
    {
        using IDbConnection connection = GetConnection();

        return await connection.QueryAsync<T>(storedProcedure, parameters,
                     commandType: CommandType.StoredProcedure);
    }

    public async Task SaveData<T>(string storedProcedure, T parameters)
    {
        using IDbConnection connection = GetConnection();

        await connection.ExecuteAsync(storedProcedure, parameters,
              commandType: CommandType.StoredProcedure);
    }

    public IDbConnection GetConnection(string? connectionId = "MySQL")
    {
        IDbConnection connection;
        switch (connectionId)
        {
            case "SqlServer":
                connection = new SqlConnection(_config.GetConnectionString(connectionId));
                break;
            case "MySQL":
                connection = new MySqlConnection(_config.GetConnectionString(connectionId));
                break;
            default:
                connection = new MySqlConnection(_config.GetConnectionString(connectionId));
                break;
        }
        return connection;
    }
}
