using databaseEditor.Database;
using databaseEditor.jira;
using databaseEditor.Logic;
using databaseEditor.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace databaseEditor;
internal class Program
{
    private const int NumberOfThreads = 11;

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
            if (UIFunctions.CheckIfUserWantsToTakeAction("filter iteration 1"))
            {
                FilterIteration1Functions();
            }
            if (UIFunctions.CheckIfUserWantsToTakeAction("prepare iteration 2"))
            {
                PrepareIteration2Functions(db);
            }
            if (UIFunctions.CheckIfUserWantsToTakeAction("filter iteration 2"))
            {
                FilterIteration2Functions(db);
            }
            if (UIFunctions.CheckIfUserWantsToTakeAction("filter iteration 3"))
            {
                FilterIteration3Functions(db);
            }
            if (UIFunctions.CheckIfUserWantsToTakeAction("prepare iteration 4"))
            {
                PrepareIteration4Functions(db);
            }
            if (UIFunctions.CheckIfUserWantsToTakeAction("filter iteration 4"))
            {
                FilterIteration4Functions(db);
            }
            if (UIFunctions.CheckIfUserWantsToTakeAction("Jira issue parent functions"))
            {
                DoJiraIssueParentFunctions(db);
            }
        }
        Console.WriteLine("Shutting down.");
    }

    private static void FilterIteration3Functions(RelationsDbContext db)
    {
        if (UIFunctions.CheckIfUserWantsToTakeAction("create average_similarity_arch_emails_all_issues"))
        {
            DatabaseFunctions.CreateAverageSimilarityArchEmailsAllIssues(3, "analysis_unique_pairs_arch_emails_all_issues", "unique_filtered_sim_arch_emails_all_issues");
        }
        if (UIFunctions.CheckIfUserWantsToTakeAction("create average_similarity_arch_issues_all_emails"))
        {
            DatabaseFunctions.CreateAverageSimilarityArchIssuesAllEmails(3, "analysis_unique_pairs_arch_issues_all_emails", "unique_filtered_sim_arch_issues_all_emails");
        }
    }

    private static void PrepareIteration4Functions(RelationsDbContext db)
    {
        if (UIFunctions.CheckIfUserWantsToTakeAction("create expanded tables"))
        {
            DatabaseFunctions.CreateExpandedSimilarityTables("iter4_sen_sim_expanded_arch_issues_all_emails");
            DatabaseFunctions.CreateExpandedSimilarityTables("iter4_cos_sim_expanded_arch_issues_all_emails");
        }
        if (UIFunctions.CheckIfUserWantsToTakeAction("insert data to expanded table"))
        {
            DatabaseFunctions.InsertInExpandedSimilarityTables("iter4_sen_sim_expanded_arch_issues_all_emails", "iter4_sen_sim_result_arch_issues_all_emails", 0.35f);
            DatabaseFunctions.InsertInExpandedSimilarityTables("iter4_cos_sim_expanded_arch_issues_all_emails", "iter4_cos_sim_result_arch_issues_all_emails", 0.1f);
        }
        if (UIFunctions.CheckIfUserWantsToTakeAction("fill in creation time difference in expanded table"))
        {
            var listOfIter4SenSimExpandedArchIssuesAllEmails = DatabaseFunctions.GetIter4SenSimExpandedArchIssuesAllEmails(db);
            var listOfIter4CosSimExpandedArchIssuesAllEmails = DatabaseFunctions.GetIter4CosSimExpandedArchIssuesAllEmails(db);
            Iter4Logic.RunMultiThreadedFillInCreationTimeDifference(listOfIter4SenSimExpandedArchIssuesAllEmails, NumberOfThreads);
            Iter4Logic.RunMultiThreadedFillInCreationTimeDifference(listOfIter4CosSimExpandedArchIssuesAllEmails, NumberOfThreads);
            DatabaseFunctions.SaveDatabase(db);
        }
    }

    private static void FilterIteration4Functions(RelationsDbContext db)
    {
        if (UIFunctions.CheckIfUserWantsToTakeAction("create filtered arch_issues_all_emails tables"))
        {
            DatabaseFunctions.CreateFilteredArchIssuesAllEmails("iter4_cos_sim_expanded_arch_issues_all_emails ", "iter4_cos_sim_filtered_arch_issues_all_emails");
            DatabaseFunctions.CreateFilteredArchIssuesAllEmails("iter4_sen_sim_expanded_arch_issues_all_emails ", "iter4_sen_sim_filtered_arch_issues_all_emails");
        }
        if (UIFunctions.CheckIfUserWantsToTakeAction("create average similarity arch_issues_all_emails tables"))
        {
            DatabaseFunctions.CreateAverageSimilarityArchIssuesAllEmails(4, "iter4_cos_sim_filtered_arch_issues_all_emails", "iter4_sen_sim_filtered_arch_issues_all_emails");
        }
    }

    private static void EditSourceTableFunctions(RelationsDbContext db)
    {
        if (UIFunctions.CheckIfUserWantsToTakeAction("all remaining jira issue parent keys in source table"))
        {
            var listOfAllRemainingJiraIssues = DatabaseFunctions.GetJiraIssues(db).Where(issue => issue.ParentKey == null).ToList();
            var batchSize = 1000;
            for (int i = 0; i < listOfAllRemainingJiraIssues.Count(); i+=batchSize)
            {
                var listOfBatchJiraIssues = listOfAllRemainingJiraIssues.Skip(i).Take(batchSize).ToList();
                if (listOfBatchJiraIssues.Count < 1000)
                {
                    Console.WriteLine("Less than 1000 issues found without a filled in parent key.");
                }
                List<string> jiraKeyList = new List<string>();
                listOfBatchJiraIssues.ForEach(jiraIssue => jiraKeyList.Add(jiraIssue.Key));
                Console.Write("Fetching data from API...");
                var dictionary = JiraApiFunctions.GetParentDictionaryFromJiraIssues(jiraKeyList);
                Console.Write("\rFetched data from API.      \n");
                listOfBatchJiraIssues.ForEach(issue =>
                {
                    issue.ParentKey = dictionary[issue.Key].ParentIssueKey ?? issue.Key;
                });
                DatabaseFunctions.SaveDatabase(db);
                Thread.Sleep(60000);
            }
        }
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
        if (UIFunctions.CheckIfUserWantsToTakeAction("update categories / is_design in data_jira_jira_issue with new data json"))
        {
            var listOfJiraIssues = DatabaseFunctions.GetJiraIssues(db);
            SourceTableLogic.UpdateJiraIssueCategories(listOfJiraIssues);
            DatabaseFunctions.SaveDatabase(db);
        }


    }

    private static void PrepareIteration1Functions(RelationsDbContext db)
    {
        if (UIFunctions.CheckIfUserWantsToTakeAction("create expanded tables"))
        {
            DatabaseFunctions.CreateExpandedSimilarityTables("iter1_expanded_arch_issues_all_emails");
            DatabaseFunctions.CreateExpandedSimilarityTables("iter1_expanded_arch_emails_all_issues");
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
    }

    private static void FilterIteration1Functions()
    {
        if (UIFunctions.CheckIfUserWantsToTakeAction("apply word limit filter (remove entries with less than 50 words) and export as new table"))
        {
            DatabaseFunctions.ApplyWordCountFilterExportAsNewTable("arch_emails_all_issues_word_filtered", "expanded_arch_emails_all_issues");
            DatabaseFunctions.ApplyWordCountFilterExportAsNewTable("arch_issues_all_emails_word_filtered", "expanded_arch_issues_all_emails");
        }
        if (UIFunctions.CheckIfUserWantsToTakeAction("apply creation time difference filter (remove entries with a creation time difference greater than 500 days) and export as new table"))
        {
            DatabaseFunctions.ApplyCreationTimeDifferenceFilterExportAsNewTable("arch_emails_all_issues_word_and_creation_time_filtered", "arch_emails_all_issues_word_filtered");
            DatabaseFunctions.ApplyCreationTimeDifferenceFilterExportAsNewTable("arch_issues_all_emails_word_and_creation_time_filtered", "arch_issues_all_emails_word_filtered");
        }
    }

    private static void PrepareIteration2Functions(RelationsDbContext db)
    {
        if (UIFunctions.CheckIfUserWantsToTakeAction("create sim-expanded tables"))
        {
            DatabaseFunctions.CreateExpandedSimilarityTables("iter2_sim_expanded_arch_issues_all_emails");
            DatabaseFunctions.CreateExpandedSimilarityTables("iter2_sim_expanded_arch_emails_all_issues");
        }
        if (UIFunctions.CheckIfUserWantsToTakeAction("insert source data to expanded table"))
        {
            DatabaseFunctions.InsertInExpandedSimilarityTables("iter2_sim_expanded_arch_emails_all_issues", "sim_result_arch_emails_all_issues", 0.35f);
            DatabaseFunctions.InsertInExpandedSimilarityTables("iter2_sim_expanded_arch_issues_all_emails", "sim_result_arch_issues_all_emails", 0.35f);
        }
        if (UIFunctions.CheckIfUserWantsToTakeAction("fill in creation time difference in expanded table"))
        {
            var listOfSimExpandedArchEmailsAllIssues = DatabaseFunctions.GetSimExpandedArchEmailsAllIssues(db);
            var listOfSimExpandedArchIssuesAllEmails = DatabaseFunctions.GetSimExpandedArchIssuesAllEmails(db);
            SimExpandedTableLogic.RunMultiThreadedFillInCreationTimeDifference(listOfSimExpandedArchEmailsAllIssues, listOfSimExpandedArchIssuesAllEmails, NumberOfThreads);
            DatabaseFunctions.SaveDatabase(db);
        }
    }

    private static void FilterIteration2Functions(RelationsDbContext db)
    {
        if (UIFunctions.CheckIfUserWantsToTakeAction("apply word limit filter (remove entries with less than 50 words) and export as new table"))
        {
            DatabaseFunctions.ApplyWordCountFilterExportAsNewTable("iter2_sim_arch_emails_all_issues_word_filtered", "iter2_sim_expanded_arch_emails_all_issues");
            DatabaseFunctions.ApplyWordCountFilterExportAsNewTable("iter2_sim_arch_issues_all_emails_word_filtered", "iter2_sim_expanded_arch_issues_all_emails");
        }
        if (UIFunctions.CheckIfUserWantsToTakeAction("apply creation time difference filter (remove entries with a creation time difference greater than 500 days) and export as new table"))
        {
            DatabaseFunctions.ApplyCreationTimeDifferenceFilterExportAsNewTable("iter2_sim_arch_emails_all_issues_word_and_creation_time_filtered", "iter2_sim_arch_emails_all_issues_word_filtered");
            DatabaseFunctions.ApplyCreationTimeDifferenceFilterExportAsNewTable("iter2_sim_arch_issues_all_emails_word_and_creation_time_filtered", "iter2_sim_arch_issues_all_emails_word_filtered");
        }
        if (UIFunctions.CheckIfUserWantsToTakeAction("apply duplication filter and export as new tables"))
        {
            DatabaseFunctions.ApplyDuplicationFilterExportAsNewTable("iter2_unique_filtered_sim_arch_emails_all_issues", "iter2_sim_arch_emails_all_issues_word_and_creation_time_filtered");
            DatabaseFunctions.ApplyDuplicationFilterExportAsNewTable("iter2_unique_filtered_sim_arch_issues_all_emails", "iter2_sim_arch_issues_all_emails_word_and_creation_time_filtered");
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
}
