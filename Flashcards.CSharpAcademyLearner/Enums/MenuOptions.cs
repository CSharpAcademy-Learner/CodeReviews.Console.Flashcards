using System.ComponentModel.DataAnnotations;

namespace Flashcards.CSharpAcademyLearner.Enums
{
    internal class MenuOptions
    {
        internal enum MainMenuOption
        {
            [Display(Name = "Manage Stacks")]
            ManageStacks,
            [Display(Name = "Manage Flashcards")]
            ManageFlashcards,
            [Display(Name = "Start Study Session")]
            StartStudy,
            [Display(Name = "View Study Session Data")]
            ViewStudySessions,
            [Display(Name = "Exit Application")]
            Exit
        }

        internal enum StackMenuOption
        {
            [Display(Name = "View All Stacks")]
            ViewAll,
            [Display(Name = "Create Stack")]
            Create,
            [Display(Name = "Delete Stack")]
            Delete,
            [Display(Name = "Back to Main Menu")]
            Back
        }

        internal enum FlashcardMenuOption
        {
            [Display(Name = "View all Flashcards in Stack")]
            ViewAll,
            [Display(Name = "Create a Flashcard")]
            Create,
            [Display(Name = "Edit a Flashcard")]
            Edit,
            [Display(Name = "Delete a Flashcard")]
            Delete,
            [Display(Name = "Go back")]
            Back
        }
    }
}
