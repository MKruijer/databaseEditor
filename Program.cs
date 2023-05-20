using databaseEditor.Database;
using databaseEditor.jira;
using databaseEditor.Logic;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace databaseEditor;
internal class Program
{
    static void Main(string[] args)
    {
        //var jsonString = JiraApiObject.RestCall("CASSANDRA-13475", "fields=parent");
        Console.WriteLine(JiraApiObject.GetTopParentJiraIssueKeyFromJiraIssueKey("CASSANDRA-13475"));

        using (var db = DatabaseFunctions.GetPostgresContext())
        {
            if (UIFunctions.CheckIfUserWantsToTakeAction("edit source tables"))
            {
                var listOfEmails = DatabaseFunctions.GetEmails(db);
                var listOfJiraIssues = DatabaseFunctions.GetJiraIssues(db);
                SourceTableLogic.FillInSourceTables(listOfEmails, listOfJiraIssues);
                DatabaseFunctions.SaveDatabase(db);
            }
            if (UIFunctions.CheckIfUserWantsToTakeAction("edit modify table"))
            {
                var listOfEmails = DatabaseFunctions.GetEmails(db);
                var listOfJiraIssues = DatabaseFunctions.GetJiraIssues(db);
                var listOfModifiedResultsArchEmailsAllIssuePairs = DatabaseFunctions.GetModifiedArchEmailsAllIssues(db);
                var listOfModifiedResultsArchIssuesAllEmailPairs = DatabaseFunctions.GetModifiedArchIssuesAllEmails(db);
                ModifiedTableLogic.FillInModifyTables(listOfEmails, listOfJiraIssues, listOfModifiedResultsArchEmailsAllIssuePairs, listOfModifiedResultsArchIssuesAllEmailPairs);
                DatabaseFunctions.SaveDatabase(db);
            }
        }
        Console.WriteLine("Shutting down.");
    }
}
