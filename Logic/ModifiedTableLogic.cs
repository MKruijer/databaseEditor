using databaseEditor.Database;
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
                                                            "email_body",
                                                            "email_date",
                                                            "issue_summary",
                                                            "issue_description",
                                                            "issue_created",
                                                            "issue_modified"
                                                        };
            InsertDataFromOldTableIntoNewTable("analysis_arch_emails_all_issues", "modified_arch_emails_all_issues", columnNames);
            InsertDataFromOldTableIntoNewTable("analysis_arch_issues_all_emails", "modified_arch_issues_all_emails", columnNames);
        }

        public static void FillInModifyTables(List<DataEmailEmail> listOfEmails, List<DataJiraJiraIssue> listOfJiraIssues, List<ModifiedArchEmailsAllIssue> listOfModifiedArchEmailsAllIssuePairs, List<ModifiedArchIssuesAllEmail> listOfModifiedArchIssuesAllEmailPairs)
        {
            FillInModifiedArchEmailsAllIssueNewEmailData(listOfModifiedArchEmailsAllIssuePairs, listOfEmails);
            FillInModifiedArchEmailsAllIssueIssueDescription(listOfModifiedArchEmailsAllIssuePairs, listOfJiraIssues);
            FillInModifiedArchEmailsAllIssueCreationTimeDifference(listOfModifiedArchEmailsAllIssuePairs);

            FillInModifiedArchIssuesAllEmailNewEmailData(listOfModifiedArchIssuesAllEmailPairs, listOfEmails);
            FillInModifiedArchIssuesAllEmailIssueDescription(listOfModifiedArchIssuesAllEmailPairs,listOfJiraIssues);
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
            Console.WriteLine("Starting filling in email thread and word count...");
            foreach (var pair in listOfModifiedCosinePairs)
            {
                pair.EmailWordCount = listOfEmails.Where(e => e.Id == pair.EmailId).FirstOrDefault()?.WordCount;
                pair.EmailThreadId = listOfEmails.Where(e => e.Id == pair.EmailId).FirstOrDefault()?.ThreadId;
                UIFunctions.PrintStatusUpdate(++currentAmountOfPairsDone, totalAmountOfPairs);
            }
            Console.Write("\rFilling in email thread and word count completed.\n");
            Console.WriteLine();
        }

        private static void FillInModifiedArchEmailsAllIssueIssueDescription(List<ModifiedArchEmailsAllIssue> listOfModifiedCosinePairs, List<DataJiraJiraIssue> listOfJiraIssues)
        {
            var totalAmountOfPairs = listOfModifiedCosinePairs.Count();
            int currentAmountOfPairsDone = 0;
            Console.WriteLine("Starting filling in issue description word count...");
            foreach (var pair in listOfModifiedCosinePairs)
            {
                //pair.IssueDescriptionWordCount = listOfJiraIssues.Where(i => i.Key == pair.IssueKey).FirstOrDefault()?.DescriptionWordCount;
                UIFunctions.PrintStatusUpdate(++currentAmountOfPairsDone, totalAmountOfPairs);
            }
            Console.Write("\rFilling in issue description word count completed.\n");
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
            Console.WriteLine("Starting filling in email thread and word count...");
            foreach (var pair in listOfModifiedCosinePairs)
            {
                pair.EmailWordCount = listOfEmails.Where(e => e.Id == pair.EmailId).FirstOrDefault()?.WordCount;
                pair.EmailThreadId = listOfEmails.Where(e => e.Id == pair.EmailId).FirstOrDefault()?.ThreadId;
                UIFunctions.PrintStatusUpdate(++currentAmountOfPairsDone, totalAmountOfPairs);
            }
            Console.Write("\rFilling in email thread and word count completed.\n");
            Console.WriteLine();
        }

        private static void FillInModifiedArchIssuesAllEmailIssueDescription(List<ModifiedArchIssuesAllEmail> listOfModifiedCosinePairs, List<DataJiraJiraIssue> listOfJiraIssues)
        {
            var totalAmountOfPairs = listOfModifiedCosinePairs.Count();
            int currentAmountOfPairsDone = 0;
            Console.WriteLine("Starting filling in issue description word count...");
            foreach (var pair in listOfModifiedCosinePairs)
            {
                pair.IssueDescriptionWordCount = listOfJiraIssues.Where(i => i.Key == pair.IssueKey).FirstOrDefault()?.DescriptionWordCount;
                UIFunctions.PrintStatusUpdate(++currentAmountOfPairsDone, totalAmountOfPairs);
            }
            Console.Write("\rFilling in issue description word count completed.\n");
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

    }
}
