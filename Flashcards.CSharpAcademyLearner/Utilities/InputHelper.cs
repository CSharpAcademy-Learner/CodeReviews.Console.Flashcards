using Spectre.Console;

namespace Flashcards.CSharpAcademyLearner.Utilities
{
    internal static class InputHelper
    {
        internal static string GetStringInput(string prompt, bool optional = false)
        {
            var textPrompt = new TextPrompt<string>(prompt);
            if (optional)
            {
                textPrompt.AllowEmpty();
            }
            else
            {
                textPrompt.ValidationErrorMessage("[red]Input cannot be empty. Please try again.[/]");
            }
            return AnsiConsole.Prompt(textPrompt) ?? string.Empty;
        }

        internal static int GetIntInput(string prompt, int min, int max)
        {
            return AnsiConsole.Prompt(
                new TextPrompt<int>(prompt)
                    .ValidationErrorMessage($"[red]Please enter a valid integer between {min} and {max}.[/]")
                    .Validate(value => value >= min && value <= max));
        }

        internal static void WaitForKey()
        {
            AnsiConsole.MarkupLine("\n[grey]Press any key to continue...[/]");
            Console.ReadKey(true);
        }
    }
}
