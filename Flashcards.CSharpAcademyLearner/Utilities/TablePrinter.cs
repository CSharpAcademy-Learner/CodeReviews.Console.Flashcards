using Spectre.Console;

namespace Flashcards.CSharpAcademyLearner.Utilities
{
    internal static class TablePrinter
    {
        internal static void PrintTable<T>(IEnumerable<T> items, string[] headers, params Func<T, object>[] selectors)
        {
            var list = items.ToList();
            if (list.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No records found.[/]");
                return;
            }

            var table = new Table().Border(TableBorder.Rounded);

            foreach (var header in headers)
            {
                table.AddColumn(new TableColumn($"[bold green]{header}[/]").Centered());
            }

            foreach (var item in list)
            {
                var rowValues = new string[headers.Length];
                for (int i = 0; i < headers.Length; i++)
                {
                    rowValues[i] = selectors[i](item)?.ToString() ?? string.Empty;
                }
                table.AddRow(rowValues);
            }

            AnsiConsole.Write(table);
        }
    }
}
