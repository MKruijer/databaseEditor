using databaseEditor.Database;
using databaseEditor.jira;
using databaseEditor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace databaseEditor.Logic
{
    public static class ModifiedTableLogic
    {
        public static void InsertDataFromOldTableToNewTable()
        {
            List<string> columnNames = new List<string>{
                                                            "email_id",
                                                            "issue_key",
                                                            "similarity",
                                                        };
            InsertDataFromOldTableIntoNewTable("result_arch_emails_all_issues", "modified_results_arch_emails_all_issues", columnNames);
            InsertDataFromOldTableIntoNewTable("result_arch_issues_all_emails", "modified_results_arch_issues_all_emails", columnNames);
        }

        public static void FillInModifyTables(List<DataEmailEmail> listOfEmails, List<DataJiraJiraIssue> listOfJiraIssues, List<ModifiedArchEmailsAllIssue> listOfModifiedArchEmailsAllIssuePairs, List<ModifiedArchIssuesAllEmail> listOfModifiedArchIssuesAllEmailPairs)
        {
            FillInModifiedArchEmailsAllIssueNewEmailData(listOfModifiedArchEmailsAllIssuePairs, listOfEmails);
            FillInModifiedArchEmailsAllIssueNewIssueData(listOfModifiedArchEmailsAllIssuePairs, listOfJiraIssues);
            FillInModifiedArchEmailsAllIssueCreationTimeDifference(listOfModifiedArchEmailsAllIssuePairs);

            FillInModifiedArchIssuesAllEmailNewEmailData(listOfModifiedArchIssuesAllEmailPairs, listOfEmails);
            FillInModifiedArchIssuesAllEmailIssueDescription(listOfModifiedArchIssuesAllEmailPairs, listOfJiraIssues);
            FillInModifiedArchIssuesAllEmailCreationTimeDifference(listOfModifiedArchIssuesAllEmailPairs);
        }

        private static void InsertDataFromOldTableIntoNewTable(string oldTable, string newTable, List<string> columnNames)
        {
            Console.Write($"Inserting columns: ");
            columnNames.ForEach(columnName => Console.WriteLine($"{columnName} "));
            Console.Write($"from {oldTable} into {newTable}...");
            if (columnNames.Count == 0) return;
            var sqlString = $"INSERT INTO {newTable} ";
            if (columnNames.Count == 1)
            {
                sqlString += $"({columnNames[0]})\nSELECT {columnNames[0]}\nFROM {oldTable}";
            }
            else
            {
                sqlString += $"({columnNames[0]}";
                for (int i = 1; i < columnNames.Count; i++)
                {
                    sqlString += $", {columnNames[i]}";
                }
                sqlString += $")\nSELECT {columnNames[0]}";
                for (int i = 1; i < columnNames.Count; i++)
                {
                    sqlString += $", {columnNames[i]}";
                }
                sqlString += $"\nFROM {oldTable}";
            }
            DatabaseFunctions.ExecuteSQL(sqlString).Wait();
            Console.Write($"\rInserted columns {columnNames}");
            columnNames.ForEach(columnName => Console.WriteLine($"{columnName} "));
            Console.Write($"from {oldTable} into {newTable}.   ");
        }

        private static void FillInModifiedArchEmailsAllIssueNewEmailData(List<ModifiedArchEmailsAllIssue> listOfModifiedCosinePairs, List<DataEmailEmail> listOfEmails)
        {
            var totalAmountOfPairs = listOfModifiedCosinePairs.Count();
            int currentAmountOfPairsDone = 0;
            Console.WriteLine("Starting filling in email data...");
            foreach (var pair in listOfModifiedCosinePairs)
            {
                var email = listOfEmails.Where(e => e.Id == pair.EmailId).FirstOrDefault();
                pair.EmailWordCount = email?.WordCount;
                pair.EmailThreadId = email?.ThreadId;
                pair.EmailDate = email?.Date;
                UIFunctions.PrintStatusUpdate(++currentAmountOfPairsDone, totalAmountOfPairs);
            }
            Console.Write("\rFilling in email data completed.\n");
            Console.WriteLine();
        }

        private static void FillInModifiedArchEmailsAllIssueNewIssueData(List<ModifiedArchEmailsAllIssue> listOfModifiedCosinePairs, List<DataJiraJiraIssue> listOfJiraIssues)
        {
            var totalAmountOfPairs = listOfModifiedCosinePairs.Count();
            int currentAmountOfPairsDone = 0;
            Console.WriteLine("Starting filling in issue data...");
            foreach (var pair in listOfModifiedCosinePairs)
            {
                var issue = listOfJiraIssues.Where(i => i.Key == pair.IssueKey).FirstOrDefault();
                pair.IssueDescriptionWordCount = issue?.DescriptionWordCount;
                pair.IssueCreated = issue?.Created;
                pair.IssueModified = issue?.Modified;
                pair.IssueParentKey = issue?.ParentKey;
                UIFunctions.PrintStatusUpdate(++currentAmountOfPairsDone, totalAmountOfPairs);
            }
            Console.Write("\rFilling in issue data completed.\n");
            Console.WriteLine();
        }

        private static void FillInModifiedArchEmailsAllIssueCreationTimeDifference(List<ModifiedArchEmailsAllIssue> listOfModifiedCosinePairs)
        {
            var totalAmountOfPairs = listOfModifiedCosinePairs.Count();
            int currentAmountOfPairsDone = 0;
            Console.WriteLine("Starting filling in creation time difference...");
            foreach (var pair in listOfModifiedCosinePairs)
            {
                if (pair.IssueCreated != null && pair.EmailDate != null)
                {
                    if (pair.EmailDate > pair.IssueCreated)
                    {
                        pair.CreationTimeDifference = (pair.EmailDate - pair.IssueCreated).Value.Days;
                    }
                    else
                    {
                        pair.CreationTimeDifference = (pair.IssueCreated - pair.EmailDate).Value.Days;
                    }
                }
                UIFunctions.PrintStatusUpdate(++currentAmountOfPairsDone, totalAmountOfPairs);
            }
            Console.Write("\rFilling in creation time difference completed.\n");
            Console.WriteLine();
        }

        private static void FillInModifiedArchIssuesAllEmailNewEmailData(List<ModifiedArchIssuesAllEmail> listOfModifiedCosinePairs, List<DataEmailEmail> listOfEmails)
        {
            var totalAmountOfPairs = listOfModifiedCosinePairs.Count();
            int currentAmountOfPairsDone = 0;
            Console.WriteLine("Starting filling in email data...");
            foreach (var pair in listOfModifiedCosinePairs)
            {
                var email = listOfEmails.Where(e => e.Id == pair.EmailId).FirstOrDefault();
                pair.EmailWordCount = email?.WordCount;
                pair.EmailThreadId = email?.ThreadId;
                pair.EmailDate = email?.Date;
                UIFunctions.PrintStatusUpdate(++currentAmountOfPairsDone, totalAmountOfPairs);
            }
            Console.Write("\rFilling in email data completed.\n");
            Console.WriteLine();
        }

        private static void FillInModifiedArchIssuesAllEmailIssueDescription(List<ModifiedArchIssuesAllEmail> listOfModifiedCosinePairs, List<DataJiraJiraIssue> listOfJiraIssues)
        {
            var totalAmountOfPairs = listOfModifiedCosinePairs.Count();
            int currentAmountOfPairsDone = 0;
            Console.WriteLine("Starting filling in issue data...");
            foreach (var pair in listOfModifiedCosinePairs)
            {
                var issue = listOfJiraIssues.Where(i => i.Key == pair.IssueKey).FirstOrDefault();
                pair.IssueDescriptionWordCount = issue?.DescriptionWordCount;
                pair.IssueCreated = issue?.Created;
                pair.IssueModified = issue?.Modified;
                pair.IssueParentKey = issue?.ParentKey;
                UIFunctions.PrintStatusUpdate(++currentAmountOfPairsDone, totalAmountOfPairs);
            }
            Console.Write("\rFilling in issue data completed.\n");
            Console.WriteLine();
        }

        private static void FillInModifiedArchIssuesAllEmailCreationTimeDifference(List<ModifiedArchIssuesAllEmail> listOfModifiedCosinePairs)
        {
            var totalAmountOfPairs = listOfModifiedCosinePairs.Count();
            int currentAmountOfPairsDone = 0;
            Console.WriteLine("Starting filling in creation time difference...");
            foreach (var pair in listOfModifiedCosinePairs)
            {
                if (pair.IssueCreated != null && pair.EmailDate != null)
                {
                    if (pair.EmailDate > pair.IssueCreated)
                    {
                        pair.CreationTimeDifference = (pair.EmailDate - pair.IssueCreated).Value.Days;
                    }
                    else
                    {
                        pair.CreationTimeDifference = (pair.IssueCreated - pair.EmailDate).Value.Days;
                    }
                }
                UIFunctions.PrintStatusUpdate(++currentAmountOfPairsDone, totalAmountOfPairs);
            }
            Console.Write("\rFilling in creation time difference completed.\n");
            Console.WriteLine();
        }

        public static void FillInArchIssuesAllEmailJiraParent(List<Models.ModifiedArchIssuesAllEmail> listOfModifiedResultsArchIssuesAllEmailPairs)
        {
            List<string> jiraKeyList = new List<string>();
            listOfModifiedResultsArchIssuesAllEmailPairs.ForEach(pair => jiraKeyList.Add(pair.IssueKey));
            var dictionary = JiraApiFunctions.GetParentDictionaryFromJiraIssues(jiraKeyList);
            listOfModifiedResultsArchIssuesAllEmailPairs.ForEach(pair =>
            {
                pair.IssueParentKey = dictionary[pair.IssueKey].ParentIssueKey ?? pair.IssueKey;
            });
        }

        public static void FillInArchEmailsAllIssueJiraParent(List<Models.ModifiedArchEmailsAllIssue> listOfModifiedResultsArchEmailsAllIssuePairs)
        {
            List<string> jiraKeyList = new List<string>();
            listOfModifiedResultsArchEmailsAllIssuePairs.ForEach(pair => jiraKeyList.Add(pair.IssueKey));
            var dictionary = JiraApiFunctions.GetParentDictionaryFromJiraIssues(jiraKeyList);
            listOfModifiedResultsArchEmailsAllIssuePairs.ForEach(pair =>
            {
                pair.IssueParentKey = dictionary[pair.IssueKey].ParentIssueKey ?? pair.IssueKey;
            });
        }

    }
}
