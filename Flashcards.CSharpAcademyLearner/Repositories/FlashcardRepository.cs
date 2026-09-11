using Dapper;
using Flashcards.CSharpAcademyLearner.Models;
using Microsoft.Data.SqlClient;

namespace Flashcards.CSharpAcademyLearner.Repositories
{
    internal class FlashcardRepository
    {
        private readonly string _connectionString;

        internal FlashcardRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        internal List<Flashcard> GetByStack(int stackId)
        {
            using var connection = new SqlConnection(_connectionString);
            return connection.Query<Flashcard>(
                "SELECT Id, Front, Back, StackId FROM Flashcards WHERE StackId = @StackId;",
                new { StackId = stackId }).ToList();
        }

        internal void Insert(string front, string back, int stackId)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Execute(
                "INSERT INTO Flashcards (Front, Back, StackId) VALUES (@Front, @Back, @StackId);",
                new { Front = front, Back = back, StackId = stackId });
        }

        internal void Update(int id, string front, string back)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Execute(
                "UPDATE Flashcards SET Front = @Front, Back = @Back WHERE Id = @Id;",
                new { Front = front, Back = back, Id = id });
        }

        internal void Delete(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Execute("DELETE FROM Flashcards WHERE Id = @Id;", new { Id = id });
        }
    }
}
