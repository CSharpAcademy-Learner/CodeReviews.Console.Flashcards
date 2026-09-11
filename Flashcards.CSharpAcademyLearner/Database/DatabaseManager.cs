using Dapper;
using Microsoft.Data.SqlClient;

namespace Flashcards.CSharpAcademyLearner.Database
{
    internal static class DatabaseManager
    {
        internal static void InitializeDatabase(string masterConnectionString, string appConnectionString)
        {
            var createDatabaseSQL = @"IF DB_ID('FlashcardsDB') IS NULL
                            CREATE DATABASE FlashcardsDB";

            using (var connection = new SqlConnection(masterConnectionString))
            {
                connection.Execute(createDatabaseSQL);
            }

            var createTablesSQL = @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Stacks')
                    BEGIN
                        CREATE TABLE Stacks (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            Name NVARCHAR(100) UNIQUE NOT NULL
                        );
                    END;

                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Flashcards')
                    BEGIN
                        CREATE TABLE Flashcards (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            Front NVARCHAR(255) NOT NULL,
                            Back NVARCHAR(255) NOT NULL,
                            StackId INT NOT NULL,
                            CONSTRAINT FK_Flashcards_Stacks FOREIGN KEY (StackId) REFERENCES Stacks(Id) ON DELETE CASCADE
                        );
                    END;

                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StudySessions')
                    BEGIN
                        CREATE TABLE StudySessions (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            Date DATETIME NOT NULL,
                            Score INT NOT NULL,
                            StackId INT NOT NULL,
                            CONSTRAINT FK_StudySessions_Stacks FOREIGN KEY (StackId) REFERENCES Stacks(Id) ON DELETE CASCADE
                        );
                    END;";

            using (var connection = new SqlConnection(appConnectionString))
            {
                connection.Execute(createTablesSQL);
            }
        }
    }
}
