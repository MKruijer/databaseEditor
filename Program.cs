using databaseEditor.Database;
using databaseEditor.jira;
using databaseEditor.Logic;
using databaseEditor.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace databaseEditor;
internal class Program
{
    static void Main(string[] args)
    {
        using (var db = DatabaseFunctions.GetPostgresContext())
        {
            if (UIFunctions.CheckIfUserWantsToTakeAction("edit source tables"))
            {
                EditSourceTableFunctions(db);
            }
            if (UIFunctions.CheckIfUserWantsToTakeAction("prepare iteration 1"))
            {
                PrepareIteration1Functions(db);
            }
            if (UIFunctions.CheckIfUserWantsToTakeAction("prepare iteration 2"))
            {
                PrepareIteration2Functions(db);
            }
            if (UIFunctions.CheckIfUserWantsToTakeAction("Jira issue parent functions"))
            {
                DoJiraIssueParentFunctions(db);
            }
        }
        Console.WriteLine("Shutting down.");
    }

    private static void PrepareIteration2Functions(RelationsDbContext db)
    {
        if (UIFunctions.CheckIfUserWantsToTakeAction("create sim-expanded tables"))
        {
            DatabaseFunctions.CreateExpandedSimilarityTables();
        }
        if (UIFunctions.CheckIfUserWantsToTakeAction("insert old data to expanded table"))
        {
            DatabaseFunctions.InsertInExpandedSimilarityTables();
        }
        if (UIFunctions.CheckIfUserWantsToTakeAction("fill in new data in expanded table"))
        {
            var listOfEmails = DatabaseFunctions.GetEmails(db);
            var listOfJiraIssues = DatabaseFunctions.GetJiraIssues(db);
            var listOfSimExpandedArchEmailsAllIssues = DatabaseFunctions.GetSimExpandedArchEmailsAllIssues(db);
            var listOfSimExpandedArchIssuesAllEmails = DatabaseFunctions.GetSimExpandedArchIssuesAllEmails(db);
            SimExpandedTableLogic.FillInAdditionalDataInExpandedTable(listOfEmails, listOfJiraIssues, listOfSimExpandedArchEmailsAllIssues, listOfSimExpandedArchIssuesAllEmails);
            DatabaseFunctions.SaveDatabase(db);
        }
    }

    private static void DoJiraIssueParentFunctions(RelationsDbContext db)
    {
        if (UIFunctions.CheckIfUserWantsToTakeAction("fill in Jira issue parent key data in max filtered ArchEmailAllIssue tables"))
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
        if (UIFunctions.CheckIfUserWantsToTakeAction("fill in Jira issue parent key data in max filtered ArchIssueAllEmail tables"))
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
    }

    private static void PrepareIteration1Functions(RelationsDbContext db)
    {
        if (UIFunctions.CheckIfUserWantsToTakeAction("create expanded tables"))
        {
            DatabaseFunctions.CreateExpandedTables();
        }
        if (UIFunctions.CheckIfUserWantsToTakeAction("insert old data to expanded table"))
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
        if (UIFunctions.CheckIfUserWantsToTakeAction("apply creation time difference filter (remove entries with a creation time difference greater than 500 days) and export as new table"))
        {
            DatabaseFunctions.ApplCreationTimeDifferenceFilterExportAsNewTable();
        }
    }

    private static void EditSourceTableFunctions(RelationsDbContext db)
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
        if (UIFunctions.CheckIfUserWantsToTakeAction("fill in data_jira_jira_issue and data_email_email tables with new data"))
        {
            var listOfEmails = DatabaseFunctions.GetEmails(db);
            var listOfJiraIssues = DatabaseFunctions.GetJiraIssues(db);
            SourceTableLogic.FillInSourceTables(listOfEmails, listOfJiraIssues);
            DatabaseFunctions.SaveDatabase(db);
        }
    }
}
