using Dapper;
using DataAccess.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace DataAccess.Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly IConfiguration _config;
    private readonly string _connectionString;
    public PersonRepository(IConfiguration config)
    {
        _config = config;
        _connectionString = _config.GetConnectionString("default");
    }

    private IDbConnection GetConnection()
    {
        using IDbConnection connection = new MySqlConnection(_connectionString);
        return connection;
    }
    public async Task<IEnumerable<PersonModel>> GetPeopleAsync()
    {
        var connection = GetConnection();
        string sql = "select Id,Name,Email from Person";
        var people = await connection.QueryAsync<PersonModel>(sql);
        return people;
    }

    public async Task<PersonModel> GetPeopleByIdAsync(int id)
    {
        var connection = GetConnection();
        string sql = "select Id,Name,Email from Person where Id=@id";
        var people = await connection.QueryFirstOrDefaultAsync<PersonModel>(sql, new { id });
        return people;
    }

    public async Task<PersonModel> CreatePersonAsync(PersonModel person)
    {
        var connection = GetConnection();
        string query = @"insert into Person(Name,Email) values(@Name,@Email); select Last_Insert_Id();";
        int createdId = await connection.ExecuteScalarAsync<int>(query, new { person.Name, person.Email });
        person.Id = createdId;
        return person;

    }

    public async Task UpdatePersonAsync(PersonModel person)
    {
        var connection = GetConnection();
        string query = @"update Person set Name=@Name,Email=@Email where Id=@Id";
        await connection.ExecuteAsync(query, person);
    }

    public async Task DeletePersonAsync(int id)
    {
        var connection = GetConnection();
        string query = @"delete from person where Id=@id";
        await connection.ExecuteAsync(query, new { id });
    }

}
