namespace Flashcards.CSharpAcademyLearner.Models
{
    internal class StudySession
    {
        internal int Id { get; set; }
        internal DateTime Date { get; set; }
        internal int Score { get; set; }
        internal int StackId { get; set; }
        internal string StackName { get; set; } = string.Empty;
    }
}
