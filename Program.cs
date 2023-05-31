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
        //string url = "https://issues.apache.org/jira/browse/HADOOP-249";

        //using (HttpClient client = new HttpClient())
        //{
        //    HttpResponseMessage response = client.GetAsync(url).Result;

        //    if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
        //    {
        //        Console.WriteLine("API rate limit exceeded!");
        //    }
        //    if (response.Headers.Contains("RetryAfter") || response.Headers.Contains("Warning"))
        //    {
        //        string retryAfter = response.Headers.GetValues("RetryAfter").FirstOrDefault();
        //        string warning = response.Headers.GetValues("Warning").FirstOrDefault();

        //        Console.WriteLine($"retryAfter: {retryAfter}, Warning: {warning}");
        //    }
        //}



        using (var db = DatabaseFunctions.GetPostgresContext())
        {
            if (UIFunctions.CheckIfUserWantsToTakeAction("update changes"))
            {
                var listOfJiraIssues = DatabaseFunctions.GetJiraIssues(db);
                SourceTableLogic.FillInJiraParentKeys(listOfJiraIssues);
            }
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
            if (UIFunctions.CheckIfUserWantsToTakeAction("fill in jira parentKey"))
            {
                var listOfModifiedResultsArchEmailsAllIssuePairs = DatabaseFunctions.GetModifiedArchEmailsAllIssues(db).Where(pair => pair.IssueParentKey == null).OrderByDescending(pair => pair.Similarity).Take(1000).ToList(); ;
                var listOfModifiedResultsArchIssuesAllEmailPairs = DatabaseFunctions.GetModifiedArchIssuesAllEmails(db).Where(pair => pair.IssueParentKey == null).OrderByDescending(pair => pair.Similarity).Take(1000).ToList(); ;
                ModifiedTableLogic.FillInArchEmailsAllIssueJiraParent(listOfModifiedResultsArchEmailsAllIssuePairs);
                ModifiedTableLogic.FillInArchIssuesAllEmailJiraParent(listOfModifiedResultsArchIssuesAllEmailPairs);
                DatabaseFunctions.SaveDatabase(db);
            }
        }
        Console.WriteLine("Shutting down.");
    }
}
