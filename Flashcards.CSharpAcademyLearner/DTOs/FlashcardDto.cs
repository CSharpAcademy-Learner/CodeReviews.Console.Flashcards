namespace Flashcards.CSharpAcademyLearner.DTOs
{
    internal class FlashcardDto
    {
        internal int DisplayId { get; set; }
        internal int RealId { get; set; }
        internal string Front { get; set; } = string.Empty;
        internal string Back { get; set; } = string.Empty;
    }
}
