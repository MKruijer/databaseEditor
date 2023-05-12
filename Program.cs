using databaseEditor.Database;
using databaseEditor.Logic;

namespace databaseEditor;
internal class Program
{
    static void Main(string[] args)
    {
        using (var db = DatabaseFunctions.GetPostgresContext())
        {
                var listOfEmails = DatabaseFunctions.GetEmails(db);

                DataEmailEmailLogic.FillEmailThreadIds(listOfEmails);

                DataEmailEmailLogic.FillInWordCount(listOfEmails);

                DatabaseFunctions.SaveDatabase(db);
        }
        Console.WriteLine("Shutting down.");
    }
}
