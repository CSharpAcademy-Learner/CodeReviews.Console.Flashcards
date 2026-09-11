using Flashcards.CSharpAcademyLearner.DTOs;
using Flashcards.CSharpAcademyLearner.Repositories;
using Flashcards.CSharpAcademyLearner.Utilities;
using Spectre.Console;
using static Flashcards.CSharpAcademyLearner.Enums.MenuOptions;

namespace Flashcards.CSharpAcademyLearner
{
    internal class UserInterface
    {

        private readonly StackRepository _stackRepo;
        private readonly FlashcardRepository _flashcardRepo;
        private readonly StudySessionRepository _studySessionRepo;

        internal UserInterface(StackRepository stack, FlashcardRepository flashcard, StudySessionRepository study)
        {
            _stackRepo = stack;
            _flashcardRepo = flashcard;
            _studySessionRepo = study;
        }

        internal void MainMenu()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<MainMenuOption>()
                        .Title("[bold green]FLASHCARDS MAIN MENU[/]")
                        .UseConverter(e => e.GetDisplayName())
                        .PageSize(10)
                        .AddChoices(Enum.GetValues<MainMenuOption>()));

                switch (choice)
                {
                    case MainMenuOption.ManageStacks:
                        ManageStacksLoop();
                        break;
                    case MainMenuOption.ManageFlashcards:
                        ManageFlashcardsLoop();
                        break;
                    case MainMenuOption.StartStudy:
                        StartStudySession();
                        break;
                    case MainMenuOption.ViewStudySessions:
                        ViewStudySessions();
                        break;
                    case MainMenuOption.Exit:
                        running = false;
                        break;
                }
            }
        }

        private void ManageStacksLoop()
        {
            bool inLoop = true;
            while (inLoop)
            {
                Console.Clear();

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<StackMenuOption>()
                        .Title("[bold blue]Manage Stacks[/]")
                        .UseConverter(e => e.GetDisplayName())
                        .AddChoices(Enum.GetValues<StackMenuOption>()));

                switch (choice)
                {
                    case StackMenuOption.ViewAll:
                        DisplayAllStacks();
                        InputHelper.WaitForKey();
                        break;
                    case StackMenuOption.Create:
                        CreateStack();
                        break;
                    case StackMenuOption.Delete:
                        DeleteStack();
                        break;
                    case StackMenuOption.Back:
                        inLoop = false;
                        break;
                }
            }
        }

        private void DisplayAllStacks()
        {
            var stacks = _stackRepo.GetAll();
            AnsiConsole.MarkupLine("\n[cyan]--- Stacks ---[/]");
            TablePrinter.PrintTable(stacks, new[] { "Stack Name" }, s => s.Name);
        }

        private void CreateStack()
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[bold blue]--- Create New Stack ---[/]");
            string name = InputHelper.GetStringInput("Enter Stack Name (or type 'back' to cancel): ");
            if (name.ToLower() == "back") return;

            bool success = _stackRepo.Insert(name);
            if (success)
            {
                AnsiConsole.MarkupLine($"[green]Stack '{name}' created successfully.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[red]Error: A stack with the name '{name}' already exists.[/]");
            }
            InputHelper.WaitForKey();
        }

        private void DeleteStack()
        {
            Console.Clear();
            var stacks = _stackRepo.GetAll();
            if (stacks.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No stacks available to delete.[/]");
                InputHelper.WaitForKey();
                return;
            }

            var stackNames = stacks.Select(s => s.Name).Concat(new[] { "Cancel" }).ToArray();
            var selectedStackName = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose a stack to [red]delete[/]:")
                    .AddChoices(stackNames));

            if (selectedStackName == "Cancel") return;

            var target = _stackRepo.GetByName(selectedStackName);
            if (target != null)
            {
                _stackRepo.Delete(target.Id);
                AnsiConsole.MarkupLine($"[green]Stack '{selectedStackName}' and all associated flashcards/sessions have been deleted.[/]");
            }
            InputHelper.WaitForKey();
        }

        private void ManageFlashcardsLoop()
        {
            var stacks = _stackRepo.GetAll();
            if (stacks.Count == 0)
            {
                Console.Clear();
                AnsiConsole.MarkupLine("[yellow]Please create at least one stack before managing flashcards.[/]");
                InputHelper.WaitForKey();
                return;
            }

            Console.Clear();
            var stackNames = stacks.Select(s => s.Name).Concat(new[] { "Cancel" }).ToArray();
            var selectedStackName = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose a stack to manage:")
                    .AddChoices(stackNames));

            if (selectedStackName == "Cancel") return;

            var selectedStack = _stackRepo.GetByName(selectedStackName);
            if (selectedStack == null) return;

            bool inLoop = true;
            while (inLoop)
            {
                Console.Clear();
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<FlashcardMenuOption>()
                        .Title($"[bold blue]Managing Flashcards in Stack: {selectedStack.Name}[/]")
                        .UseConverter(e => e.GetDisplayName())
                        .AddChoices(Enum.GetValues<FlashcardMenuOption>()));

                switch (choice)
                {
                    case FlashcardMenuOption.ViewAll:
                        ViewFlashcardsInStack(selectedStack.Id);
                        InputHelper.WaitForKey();
                        break;
                    case FlashcardMenuOption.Create:
                        CreateFlashcard(selectedStack.Id);
                        break;
                    case FlashcardMenuOption.Edit:
                        EditFlashcard(selectedStack.Id);
                        break;
                    case FlashcardMenuOption.Delete:
                        DeleteFlashcard(selectedStack.Id);
                        break;
                    case FlashcardMenuOption.Back:
                        inLoop = false;
                        break;
                }
            }
        }

        private List<FlashcardDto> GetFlashcardDtosForStack(int stackId)
        {
            var rawCards = _flashcardRepo.GetByStack(stackId);
            var dtos = new List<FlashcardDto>();
            int index = 1;
            foreach (var card in rawCards)
            {
                dtos.Add(new FlashcardDto
                {
                    DisplayId = index++,
                    RealId = card.Id,
                    Front = card.Front,
                    Back = card.Back
                });
            }
            return dtos;
        }

        private void ViewFlashcardsInStack(int stackId)
        {
            var dtos = GetFlashcardDtosForStack(stackId);
            AnsiConsole.MarkupLine("\n[cyan]--- Flashcards ---[/]");
            TablePrinter.PrintTable(dtos, new[] { "ID", "Front", "Back" }, d => d.DisplayId, d => d.Front, d => d.Back);
        }

        private void CreateFlashcard(int stackId)
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[bold blue]--- Create New Flashcard ---[/]");
            string front = InputHelper.GetStringInput("Enter Front description: ");
            string back = InputHelper.GetStringInput("Enter Back explanation: ");

            _flashcardRepo.Insert(front, back, stackId);
            AnsiConsole.MarkupLine("[green]Flashcard created successfully.[/]");
            InputHelper.WaitForKey();
        }

        private void EditFlashcard(int stackId)
        {
            Console.Clear();
            var dtos = GetFlashcardDtosForStack(stackId);
            if (dtos.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No flashcards found in this stack to edit.[/]");
                InputHelper.WaitForKey();
                return;
            }

            TablePrinter.PrintTable(dtos, new[] { "ID", "Front", "Back" }, d => d.DisplayId, d => d.Front, d => d.Back);
            int displayId = InputHelper.GetIntInput("Enter the ID of the card you want to edit: ", 1, dtos.Count);

            var cardToEdit = dtos.First(d => d.DisplayId == displayId);

            string newFront = InputHelper.GetStringInput("Enter new Front value (or press Enter to keep current): ", optional: true);
            string newBack = InputHelper.GetStringInput("Enter new Back value (or press Enter to keep current): ", optional: true);

            string finalFront = string.IsNullOrWhiteSpace(newFront) ? cardToEdit.Front : newFront;
            string finalBack = string.IsNullOrWhiteSpace(newBack) ? cardToEdit.Back : newBack;

            _flashcardRepo.Update(cardToEdit.RealId, finalFront, finalBack);
            AnsiConsole.MarkupLine("[green]Flashcard updated successfully.[/]");
            InputHelper.WaitForKey();
        }

        private void DeleteFlashcard(int stackId)
        {
            Console.Clear();
            var dtos = GetFlashcardDtosForStack(stackId);
            if (dtos.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No flashcards found in this stack to delete.[/]");
                InputHelper.WaitForKey();
                return;
            }

            TablePrinter.PrintTable(dtos, new[] { "ID", "Front", "Back" }, d => d.DisplayId, d => d.Front, d => d.Back);
            int displayId = InputHelper.GetIntInput("Enter the ID of the card you want to delete: ", 1, dtos.Count);

            var cardToDelete = dtos.First(d => d.DisplayId == displayId);
            _flashcardRepo.Delete(cardToDelete.RealId);

            AnsiConsole.MarkupLine("[green]Flashcard deleted successfully.[/]");
            InputHelper.WaitForKey();
        }

        private void StartStudySession()
        {
            Console.Clear();
            var stacks = _stackRepo.GetAll();
            if (stacks.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No stacks available. Create a stack first.[/]");
                InputHelper.WaitForKey();
                return;
            }

            var stackNames = stacks.Select(s => s.Name).Concat(new[] { "Cancel" }).ToArray();
            var selectedStackName = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose a stack for study:")
                    .AddChoices(stackNames));

            if (selectedStackName == "Cancel") return;

            var selectedStack = _stackRepo.GetByName(selectedStackName);
            if (selectedStack == null) return;

            var cards = _flashcardRepo.GetByStack(selectedStack.Id);
            if (cards.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]This stack has no flashcards. Please add cards before starting a session.[/]");
                InputHelper.WaitForKey();
                return;
            }

            Console.Clear();
            AnsiConsole.MarkupLine($"[bold green]Starting Study Session for stack '{selectedStack.Name}'...[/]");
            AnsiConsole.MarkupLine($"There are {cards.Count} flashcards. Press any key to begin.");
            Console.ReadKey(true);

            int score = 0;
            foreach (var card in cards)
            {
                Console.Clear();
                AnsiConsole.MarkupLine($"[cyan]Stack:[/] {selectedStack.Name}");
                AnsiConsole.MarkupLine("-------------------------");
                AnsiConsole.MarkupLine($"[bold]Front:[/] {card.Front}");
                AnsiConsole.MarkupLine("-------------------------");

                string answer = InputHelper.GetStringInput("Your Answer: ");
                if (answer.Trim().Equals(card.Back.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    AnsiConsole.MarkupLine("[green]CORRECT![/]");
                    score++;
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]INCORRECT. The correct answer was:[/] [bold]{card.Back}[/]");
                }
                InputHelper.WaitForKey();
            }

            Console.Clear();
            AnsiConsole.MarkupLine("[bold green]--- Session Finished! ---[/]");
            AnsiConsole.MarkupLine($"You scored [bold]{score}[/] out of [bold]{cards.Count}[/].");

            _studySessionRepo.Insert(DateTime.Now, score, selectedStack.Id);
            AnsiConsole.MarkupLine("[green]Your study session has been logged.[/]");
            InputHelper.WaitForKey();
        }

        private void ViewStudySessions()
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[cyan]--- Study Session History ---[/]");
            var sessions = _studySessionRepo.GetAllWithStackNames();
            TablePrinter.PrintTable(sessions,
                new[] { "Date/Time", "Score", "Stack Studied" },
                s => s.Date.ToString("yyyy-MM-dd HH:mm:ss"),
                s => s.Score,
                s => s.StackName);
            InputHelper.WaitForKey();
        }
    }
}
