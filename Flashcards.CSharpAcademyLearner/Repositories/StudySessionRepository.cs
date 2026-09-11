using Dapper;
using Flashcards.CSharpAcademyLearner.Models;
using Microsoft.Data.SqlClient;

namespace Flashcards.CSharpAcademyLearner.Repositories
{
    internal class StudySessionRepository
    {
        private readonly string _connectionString;

        internal StudySessionRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        internal void Insert(DateTime date, int score, int stackId)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Execute(
                "INSERT INTO StudySessions (Date, Score, StackId) VALUES (@Date, @Score, @StackId);",
                new { Date = date, Score = score, StackId = stackId });
        }

        internal List<StudySession> GetAllWithStackNames()
        {
            string sql = @"
                SELECT ss.Id, ss.Date, ss.Score, ss.StackId, s.Name AS StackName
                FROM StudySessions ss
                INNER JOIN Stacks s ON ss.StackId = s.Id
                ORDER BY ss.Date DESC;";

            using var connection = new SqlConnection(_connectionString);
            return connection.Query<StudySession>(sql).ToList();
        }
    }
}
