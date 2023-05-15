using databaseEditor.Database;
using databaseEditor.Logic;
using Microsoft.EntityFrameworkCore;

namespace databaseEditor;
internal class Program
{
    static void Main(string[] args)
    {
        using (var db = DatabaseFunctions.GetPostgresContext())
        {
            if (UIFunctions.CheckIfUserWantsToTakeAction("edit source tables"))
            {
                var listOfEmails = DatabaseFunctions.GetEmails(db);
                var listOfJiraIssues = DatabaseFunctions.GetJiraIssues(db);
                SourceTableLogic.FillInSourceTables(listOfEmails, listOfJiraIssues);
                DatabaseFunctions.SaveDatabase(db);
            }
            if (UIFunctions.CheckIfUserWantsToTakeAction("insert source table data into modify tables"))
            {
                ModifiedTableLogic.InsertDataFromOldTableToNewTable();
                DatabaseFunctions.SaveDatabase(db);
            }
            if (UIFunctions.CheckIfUserWantsToTakeAction("edit modify table"))
            {
                var listOfEmails = DatabaseFunctions.GetEmails(db);
                var listOfJiraIssues = DatabaseFunctions.GetJiraIssues(db);
                var listOfModifiedArchEmailsAllIssuePairs = DatabaseFunctions.GetModifiedArchEmailsAllIssues(db);
                var listOfModifiedArchIssuesAllEmailPairs = DatabaseFunctions.GetModifiedArchIssuesAllEmails(db);
                ModifiedTableLogic.FillInModifyTables(listOfEmails, listOfJiraIssues, listOfModifiedArchEmailsAllIssuePairs, listOfModifiedArchIssuesAllEmailPairs);
                DatabaseFunctions.SaveDatabase(db);
            }
        }
        Console.WriteLine("Shutting down.");
    }
}
