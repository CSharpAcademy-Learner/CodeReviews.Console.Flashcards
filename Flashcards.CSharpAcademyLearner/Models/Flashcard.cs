namespace Flashcards.CSharpAcademyLearner.Models
{
    internal class Flashcard
    {
        internal int Id { get; set; }
        internal string Front { get; set; } = string.Empty;
        internal string Back { get; set; } = string.Empty;
        internal int StackId { get; set; }
    }
}
