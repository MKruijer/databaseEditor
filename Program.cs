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
        using (var db = DatabaseFunctions.GetPostgresContext())
        {
            if (UIFunctions.CheckIfUserWantsToTakeAction("update 1000 jira issue parent keys in source table"))
            {
                var listOfJiraIssues = DatabaseFunctions.GetJiraIssues(db).Where(issue => issue.ParentKey == null).Take(1000).ToList();
                if (listOfJiraIssues.Count < 1000)
                {
                    Console.WriteLine("Less than 1000 issues found without a filled in parent key.");
                }
                List<string> jiraKeyList = new List<string>();
                listOfJiraIssues.ForEach(jiraIssue => jiraKeyList.Add(jiraIssue.Key));
                Console.Write("Fetching data from API...");
                var dictionary = JiraApiFunctions.GetParentDictionaryFromJiraIssues(jiraKeyList);
                Console.Write("\rFetched data from API.      \n");
                listOfJiraIssues.ForEach(issue =>
                {
                    issue.ParentKey = dictionary[issue.Key].ParentIssueKey ?? issue.Key;
                });
                DatabaseFunctions.SaveDatabase(db);
            }
            if (UIFunctions.CheckIfUserWantsToTakeAction("edit source tables"))
            {
                var listOfEmails = DatabaseFunctions.GetEmails(db);
                var listOfJiraIssues = DatabaseFunctions.GetJiraIssues(db);
                SourceTableLogic.FillInSourceTables(listOfEmails, listOfJiraIssues);
                DatabaseFunctions.SaveDatabase(db);
            }
            if (UIFunctions.CheckIfUserWantsToTakeAction("create expanded tables"))
            {
                DatabaseFunctions.CreateExpandedTables();
            }
            if (UIFunctions.CheckIfUserWantsToTakeAction("insert data to expanded table"))
            {
                DatabaseFunctions.InsertInExpandedTables();
            }
            if (UIFunctions.CheckIfUserWantsToTakeAction("fill in new data in expanded table"))
            {
                var listOfEmails = DatabaseFunctions.GetEmails(db);
                var listOfJiraIssues = DatabaseFunctions.GetJiraIssues(db);
                var listOfExpandedArchEmailsAllIssuesPairs = DatabaseFunctions.GetExpandedArchEmailsAllIssues(db);
                var listOfExpandedArchIssuesAllEmailsPairs = DatabaseFunctions.GetExpandedArchIssuesAllEmails(db);
                ExpandedTableLogic.FillInAdditionalDataInExpandedTable(listOfEmails, listOfJiraIssues, listOfExpandedArchEmailsAllIssuesPairs, listOfExpandedArchIssuesAllEmailsPairs);
                DatabaseFunctions.SaveDatabase(db);
            }
            if (UIFunctions.CheckIfUserWantsToTakeAction("apply word limit filter (remove entries with less than 50 words) and export as new table"))
            {
                DatabaseFunctions.ApplyWordCountFilterExportAsNewTable();
            }
            if (UIFunctions.CheckIfUserWantsToTakeAction("apply creation time difference filter (remove entries with a creation time difference greater than 700 days) and export as new table"))
            {
                DatabaseFunctions.ApplCreationTimeDifferenceFilterExportAsNewTable();
            }
            if (UIFunctions.CheckIfUserWantsToTakeAction("fill in Jira issue parent key data in max filtered tables"))
            {
                var listOfJiraIssues = DatabaseFunctions.GetJiraIssues(db);
                var listOfFilteredPairs = DatabaseFunctions.GetMaxFilteredArchEmailAllIssue(db).OrderByDescending(pair => pair.Similarity).ToList().Take(1000).ToList();
                List<string> jiraKeyList = new List<string>();
                listOfFilteredPairs.ForEach(pair => jiraKeyList.Add(pair.IssueKey));
                var dictionary = JiraApiFunctions.GetParentDictionaryFromJiraIssues(jiraKeyList);
                listOfFilteredPairs.ForEach(pair =>
                {
                    pair.IssueParentKey = dictionary[pair.IssueKey].ParentIssueKey ?? pair.IssueKey;
                });
                DatabaseFunctions.SaveDatabase(db);

            }
            if (UIFunctions.CheckIfUserWantsToTakeAction("fill in Jira issue parent key data in max filtered tables"))
            {
                var listOfJiraIssues = DatabaseFunctions.GetJiraIssues(db);
                var listOfFilteredPairs = DatabaseFunctions.GetMaxFilteredArchIssueAllEmail(db).OrderByDescending(pair => pair.Similarity).ToList().Take(1000).ToList();
                List<string> jiraKeyList = new List<string>();
                listOfFilteredPairs.ForEach(pair => jiraKeyList.Add(pair.IssueKey));
                var dictionary = JiraApiFunctions.GetParentDictionaryFromJiraIssues(jiraKeyList);
                listOfFilteredPairs.ForEach(pair =>
                {
                    pair.IssueParentKey = dictionary[pair.IssueKey].ParentIssueKey ?? pair.IssueKey;
                });
                DatabaseFunctions.SaveDatabase(db);

            }
            if (UIFunctions.CheckIfUserWantsToTakeAction("fill in Jira issue parent key data in expanded tables"))
            {
                var listOfJiraIssues = DatabaseFunctions.GetJiraIssues(db);
                var listOfExpandedArchEmailsAllIssuesPairs = DatabaseFunctions.GetExpandedArchEmailsAllIssues(db);
                var listOfExpandedArchIssuesAllEmailsPairs = DatabaseFunctions.GetExpandedArchIssuesAllEmails(db);
                ExpandedTableLogic.FillInJiraIssueParentKeyInArchIssuesAllEmail(listOfExpandedArchIssuesAllEmailsPairs, listOfJiraIssues);
                ExpandedTableLogic.FillInJiraIssueParentKeyInArchEmailsAllIssues(listOfExpandedArchEmailsAllIssuesPairs, listOfJiraIssues);
                DatabaseFunctions.SaveDatabase(db);
            }
            Console.WriteLine("Shutting down.");
        }
    }
}
