using Dapper;
using Flashcards.CSharpAcademyLearner.Models;
using Microsoft.Data.SqlClient;

namespace Flashcards.CSharpAcademyLearner.Repositories
{
    internal class StackRepository
    {
        private readonly string _connectionString;

        internal StackRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        internal List<Stack> GetAll()
        {
            using var connection = new SqlConnection(_connectionString);
            return connection.Query<Stack>("SELECT Id, Name FROM Stacks ORDER BY Name;").ToList();
        }

        internal Stack? GetByName(string name)
        {
            using var connection = new SqlConnection(_connectionString);
            return connection.QuerySingleOrDefault<Stack>(
                "SELECT Id, Name FROM Stacks WHERE Name = @Name;",
                new { Name = name });
        }

        internal bool Insert(string name)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Execute("INSERT INTO Stacks (Name) VALUES (@Name);", new { Name = name });
                return true;
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                return false;
            }
        }

        internal void Delete(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Execute("DELETE FROM Stacks WHERE Id = @Id;", new { Id = id });
        }
    }
}
