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

    public const string ConnectionString = "Host=localhost;Database=relationsDB;Username=postgres;Password=UnsavePassword";

    static void Main(string[] args)
    {
        using (var db = DatabaseFunctions.GetPostgresContext())
        {
            if (UIFunctions.CheckIfUserWantsToTakeAction("edit source tables"))
            {
                EditSourceTableFunctions(db);
            }
            if (UIFunctions.CheckIfUserWantsToTakeAction("prepare iteration 0"))
            {
                PrepareIteration0Functions(db);
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
                FilterIteration2Functions();
            }
            if (UIFunctions.CheckIfUserWantsToTakeAction("filter iteration 3"))
            {
                FilterIteration3Functions();
            }
            if (UIFunctions.CheckIfUserWantsToTakeAction("prepare iteration 4"))
            {
                PrepareIteration4Functions(db);
            }
            if (UIFunctions.CheckIfUserWantsToTakeAction("filter iteration 4"))
            {
                FilterIteration4Functions();
            }
        }
        Console.WriteLine("Shutting down.");
    }

    private static void EditSourceTableFunctions(RelationsDbContext db)
    {
        if (UIFunctions.CheckIfUserWantsToTakeAction("all remaining jira issue parent keys in source table"))
        {
            JiraApiFunctions.UpdateJiraParentKeys(db, false);
        }
        if (UIFunctions.CheckIfUserWantsToTakeAction("update 1000 jira issue parent keys in source table"))
        {
            JiraApiFunctions.UpdateJiraParentKeys(db, true);
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

    private static void PrepareIteration0Functions(RelationsDbContext db)
    {
        if (UIFunctions.CheckIfUserWantsToTakeAction("create expanded tables"))
        {
            DatabaseFunctions.CreateExpandedSimilarityTable("iter0_expanded_arch_issues_all_emails");
            DatabaseFunctions.CreateExpandedSimilarityTable("iter0_expanded_arch_emails_all_issues");
        }
        if (UIFunctions.CheckIfUserWantsToTakeAction("fill in expanded table"))
        {
            DatabaseFunctions.InsertInExpandedSimilarityTables("iter0_expanded_arch_emails_all_issues", "result_arch_emails_all_issues", 0.1f);
            DatabaseFunctions.InsertInExpandedSimilarityTables("iter0_expanded_arch_issues_all_emails", "result_arch_issues_all_emails", 0.1f);
            var listOfExpandedArchEmailsAllIssuesPairs = DatabaseFunctions.GetIter0ExpandedArchEmailsAllIssues(db);
            var listOfExpandedArchIssuesAllEmailsPairs = DatabaseFunctions.GetIter0ExpandedArchIssuesAllEmails(db);
            Iter0Logic.FillInCreationTimeDifference(listOfExpandedArchEmailsAllIssuesPairs, listOfExpandedArchIssuesAllEmailsPairs);
            DatabaseFunctions.SaveDatabase(db);
        }
    }

    private static void FilterIteration1Functions()
    {
        if (UIFunctions.CheckIfUserWantsToTakeAction("create new filtered table based on expanded table"))
        {
            DatabaseFunctions.CopyTable("iter1_filtered_arch_emails_all_issues", "iter0_expanded_arch_emails_all_issues");
            DatabaseFunctions.CopyTable("iter1_filtered_arch_issues_all_emails", "iter0_expanded_arch_issues_all_emails");
        }
        if (UIFunctions.CheckIfUserWantsToTakeAction("apply word limit filter (remove entries with less than 50 words) to filtered table"))
        {
            DatabaseFunctions.ApplyWordCountFilterByRemoval(50, "iter1_filtered_arch_emails_all_issues");
            DatabaseFunctions.ApplyWordCountFilterByRemoval(50, "iter1_filtered_arch_issues_all_emails");
        }
        if (UIFunctions.CheckIfUserWantsToTakeAction("apply creation time difference filter (remove entries with a creation time difference greater than 500 days) to filtered table"))
        {
            DatabaseFunctions.ApplyCreationTimeFilterByRemoval(500, "iter1_filtered_arch_emails_all_issues");
            DatabaseFunctions.ApplyCreationTimeFilterByRemoval(500, "iter1_filtered_arch_issues_all_emails");
        }
        if (UIFunctions.CheckIfUserWantsToTakeAction("apply duplication filter (remove entries with the same email_thread_id and issue_parent_key and a smaller similarity score) to filtered table"))
        {
            DatabaseFunctions.ApplyDuplicationFilterByRemoval("iter1_filtered_arch_emails_all_issues");
            DatabaseFunctions.ApplyDuplicationFilterByRemoval("iter1_filtered_arch_issues_all_emails");
        }
    }

    private static void PrepareIteration2Functions(RelationsDbContext db)
    {
        if (UIFunctions.CheckIfUserWantsToTakeAction("create sim-expanded tables"))
        {
            DatabaseFunctions.CreateExpandedSimilarityTable("iter2_expanded_arch_emails_all_issues");
            DatabaseFunctions.CreateExpandedSimilarityTable("iter2_expanded_arch_issues_all_emails");
        }
        if (UIFunctions.CheckIfUserWantsToTakeAction("fill in expanded table"))
        {
            DatabaseFunctions.InsertInExpandedSimilarityTables("iter2_expanded_arch_emails_all_issues", "sim_result_arch_emails_all_issues", 0.35f);
            DatabaseFunctions.InsertInExpandedSimilarityTables("iter2_expanded_arch_issues_all_emails", "sim_result_arch_issues_all_emails", 0.35f);
            var listOfExpandedArchEmailsAllIssues = DatabaseFunctions.GetIter2ExpandedArchEmailsAllIssues(db);
            var listOfExpandedArchIssuesAllEmails = DatabaseFunctions.GetIter2ExpandedArchIssuesAllEmails(db);
            Iter2Logic.RunMultiThreadedFillInCreationTimeDifference(listOfExpandedArchEmailsAllIssues, listOfExpandedArchIssuesAllEmails, NumberOfThreads);
            DatabaseFunctions.SaveDatabase(db);
        }
    }

    private static void FilterIteration2Functions()
    {
        if (UIFunctions.CheckIfUserWantsToTakeAction("create new filtered table based on expanded table"))
        {
            DatabaseFunctions.CopyTable("iter2_filtered_arch_emails_all_issues", "iter2_expanded_arch_emails_all_issues");
            DatabaseFunctions.CopyTable("iter2_filtered_arch_issues_all_emails", "iter2_expanded_arch_issues_all_emails");
        }
        if (UIFunctions.CheckIfUserWantsToTakeAction("apply word limit filter (remove entries with less than 50 words) to filtered table"))
        {
            DatabaseFunctions.ApplyWordCountFilterByRemoval(50, "iter2_filtered_arch_emails_all_issues");
            DatabaseFunctions.ApplyWordCountFilterByRemoval(50, "iter2_filtered_arch_issues_all_emails");
        }
        if (UIFunctions.CheckIfUserWantsToTakeAction("apply creation time difference filter (remove entries with a creation time difference greater than 500 days) to filtered table"))
        {
            DatabaseFunctions.ApplyCreationTimeFilterByRemoval(500, "iter2_filtered_arch_emails_all_issues");
            DatabaseFunctions.ApplyCreationTimeFilterByRemoval(500, "iter2_filtered_arch_issues_all_emails");
        }
        if (UIFunctions.CheckIfUserWantsToTakeAction("apply duplication filter (remove entries with the same email_thread_id and issue_parent_key and a smaller similarity score) to filtered table"))
        {
            DatabaseFunctions.ApplyDuplicationFilterByRemoval("iter2_filtered_arch_emails_all_issues");
            DatabaseFunctions.ApplyDuplicationFilterByRemoval("iter2_filtered_arch_issues_all_emails");
        }
    }

    private static void FilterIteration3Functions()
    {
        if (UIFunctions.CheckIfUserWantsToTakeAction("create iter3_average_similarity_arch_emails_all_issues"))
        {
            DatabaseFunctions.CreateAverageSimilarityArchEmailsAllIssues(3, "iter1_filtered_arch_emails_all_issues", "iter2_filtered_arch_emails_all_issues");
        }
        if (UIFunctions.CheckIfUserWantsToTakeAction("create iter3_average_similarity_arch_issues_all_emails"))
        {
            DatabaseFunctions.CreateAverageSimilarityArchIssuesAllEmails(3, "iter1_filtered_arch_issues_all_emails", "iter2_filtered_arch_issues_all_emails");
        }
    }

    private static void PrepareIteration4Functions(RelationsDbContext db)
    {
        if (UIFunctions.CheckIfUserWantsToTakeAction("create expanded tables"))
        {
            DatabaseFunctions.CreateExpandedSimilarityTable("iter4_sen_sim_expanded_arch_issues_all_emails");
            DatabaseFunctions.CreateExpandedSimilarityTable("iter4_cos_sim_expanded_arch_issues_all_emails");
        }
        if (UIFunctions.CheckIfUserWantsToTakeAction("fill in expanded table"))
        {
            var listOfIter4SenSimExpandedArchIssuesAllEmails = DatabaseFunctions.GetIter4SenSimExpandedArchIssuesAllEmails(db);
            var listOfIter4CosSimExpandedArchIssuesAllEmails = DatabaseFunctions.GetIter4CosSimExpandedArchIssuesAllEmails(db);
            DatabaseFunctions.InsertInExpandedSimilarityTables("iter4_sen_sim_expanded_arch_issues_all_emails", "iter4_sen_sim_result_arch_issues_all_emails", 0.35f);
            DatabaseFunctions.InsertInExpandedSimilarityTables("iter4_cos_sim_expanded_arch_issues_all_emails", "iter4_cos_sim_result_arch_issues_all_emails", 0.1f);
            Iter4Logic.RunMultiThreadedFillInCreationTimeDifference(listOfIter4SenSimExpandedArchIssuesAllEmails, NumberOfThreads);
            Iter4Logic.RunMultiThreadedFillInCreationTimeDifference(listOfIter4CosSimExpandedArchIssuesAllEmails, NumberOfThreads);
            DatabaseFunctions.SaveDatabase(db);
        }
    }

    private static void FilterIteration4Functions()
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
}
