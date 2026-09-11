using Flashcards.CSharpAcademyLearner;
using Flashcards.CSharpAcademyLearner.Configuration;
using Flashcards.CSharpAcademyLearner.Database;
using Flashcards.CSharpAcademyLearner.Repositories;
using Spectre.Console;

try
{
    string masterConnectionString = ConfigurationManager.GetMasterConnectionString();
    string appConnectionString = ConfigurationManager.GetAppConnectionString();

    DatabaseManager.InitializeDatabase(masterConnectionString, appConnectionString);

    StackRepository stackRepo = new(appConnectionString);
    FlashcardRepository flashRepo = new(appConnectionString);
    StudySessionRepository studySessionRepo = new(appConnectionString);

    var userInterface = new UserInterface(stackRepo, flashRepo, studySessionRepo);
    userInterface.MainMenu();
}
catch (Exception ex)
{
    AnsiConsole.MarkupLine("[bold red]An initialization error occurred:[/]");
    AnsiConsole.WriteException(ex);
    AnsiConsole.MarkupLine("[grey]Press any key to exit...[/]");
    Console.ReadKey();
}