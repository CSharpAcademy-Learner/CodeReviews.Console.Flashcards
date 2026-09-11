using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Flashcards.CSharpAcademyLearner.Utilities
{
    internal static class EnumExtensions
    {
        internal static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault()?.GetCustomAttribute<DisplayAttribute>()?.GetName() ?? enumValue.ToString();
        }
    }
}
