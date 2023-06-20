using databaseEditor.Database;
using databaseEditor.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace databaseEditor.Logic
{
    public static class SimExpandedTableLogic
    {
        public static void RunMultiThreadedFillInAdditionalData(List<DataEmailEmail> listOfEmails,
                                                        List<DataJiraJiraIssue> listOfJiraIssues,
                                                        List<SimExpandedArchEmailsAllIssue> listOfSimExpandedArchEmailsAllIssues,
                                                        List<SimExpandedArchIssuesAllEmail> listOfSimExpandedArchIssuesAllEmails)
        {
            int numThreads = 10;

            // Divide the list sizes to determine the chunk size per thread
            int chunkSizeArchEmailsAllIssues = listOfSimExpandedArchEmailsAllIssues.Count / numThreads;
            int chunkSizeArchIssuesAllEmails = listOfSimExpandedArchIssuesAllEmails.Count / numThreads;

            Parallel.ForEach(Partitioner.Create(0, numThreads), range =>
            {
                // Get the range for the current thread
                int startIndexArchEmailsAllIssues = range.Item1 * chunkSizeArchEmailsAllIssues;
                int startIndexArchIssuesAllEmails = range.Item1 * chunkSizeArchIssuesAllEmails;
                int endIndexArchEmailsAllIssues = range.Item2 * chunkSizeArchEmailsAllIssues;
                int endIndexArchIssuesAllEmails = range.Item2 * chunkSizeArchIssuesAllEmails;

                // Run the function with the subset of pairs
                FillInAdditionalDataInExpandedTable(listOfEmails, listOfJiraIssues,
                                                    listOfSimExpandedArchEmailsAllIssues.GetRange(startIndexArchEmailsAllIssues, endIndexArchEmailsAllIssues - startIndexArchEmailsAllIssues),
                                                    listOfSimExpandedArchIssuesAllEmails.GetRange(startIndexArchIssuesAllEmails, endIndexArchIssuesAllEmails - startIndexArchIssuesAllEmails));
            });
        }

        public static void FillInAdditionalDataInExpandedTable(List<DataEmailEmail> listOfEmails,
                                                               List<DataJiraJiraIssue> listOfJiraIssues,
                                                               List<SimExpandedArchEmailsAllIssue> listOfSimExpandedArchEmailsAllIssues,
                                                               List<SimExpandedArchIssuesAllEmail> listOfSimExpandedArchIssuesAllEmails)
        {
            FillInEmailWordCount(listOfSimExpandedArchEmailsAllIssues, listOfEmails);
            FillInEmailWordCount(listOfSimExpandedArchIssuesAllEmails, listOfEmails);

            FillInIssueWordCount(listOfSimExpandedArchEmailsAllIssues, listOfJiraIssues);
            FillInIssueWordCount(listOfSimExpandedArchIssuesAllEmails, listOfJiraIssues);

            FillInCreationTimeDifference(listOfSimExpandedArchEmailsAllIssues);
            FillInCreationTimeDifference(listOfSimExpandedArchIssuesAllEmails);
        }

        private static void FillInEmailWordCount(List<SimExpandedArchEmailsAllIssue> simExpandedArchEmailsAllIssuesPairs,
                                               List<DataEmailEmail> listOfEmails)
        {
            var totalAmountOfPairs = simExpandedArchEmailsAllIssuesPairs.Count();
            int currentAmountOfPairsDone = 0;
            Console.WriteLine("Starting filling in email word count in sim_expanded_arch_emails_all_issues...");
            foreach (var pair in simExpandedArchEmailsAllIssuesPairs)
            {
                var email = listOfEmails.Where(e => e.Id == pair.EmailId).FirstOrDefault();
                pair.EmailWordCount = email?.WordCount;
                UIFunctions.PrintStatusUpdate(++currentAmountOfPairsDone, totalAmountOfPairs);
            }
            Console.Write("\rFilling in email word count in sim_expanded_arch_emails_all_issues completed.\n");
            Console.WriteLine();
        }

        private static void FillInEmailWordCount(List<SimExpandedArchIssuesAllEmail> simExpandedArchIssuesAllEmailsPairs,
                                               List<DataEmailEmail> listOfEmails)
        {
            var totalAmountOfPairs = simExpandedArchIssuesAllEmailsPairs.Count();
            int currentAmountOfPairsDone = 0;
            Console.WriteLine("Starting filling in email word count in sim_expanded_arch_issues_all_emails...");
            foreach (var pair in simExpandedArchIssuesAllEmailsPairs)
            {
                var email = listOfEmails.Where(e => e.Id == pair.EmailId).FirstOrDefault();
                pair.EmailWordCount = email?.WordCount;
                UIFunctions.PrintStatusUpdate(++currentAmountOfPairsDone, totalAmountOfPairs);
            }
            Console.Write("\rFilling in email word count in sim_expanded_arch_issues_all_emails completed.\n");
            Console.WriteLine();
        }

        private static void FillInIssueWordCount(List<SimExpandedArchEmailsAllIssue> simExpandedArchEmailsAllIssuesPairs,
                                                                    List<DataJiraJiraIssue> listOfJiraIssues)
        {
            var totalAmountOfPairs = simExpandedArchEmailsAllIssuesPairs.Count();
            int currentAmountOfPairsDone = 0;
            Console.WriteLine("Starting filling in issue word count in sim_expanded_arch_emails_all_issues...");
            foreach (var pair in simExpandedArchEmailsAllIssuesPairs)
            {
                var issue = listOfJiraIssues.Where(i => i.Key == pair.IssueKey).FirstOrDefault();
                pair.IssueDescriptionWordCount = issue?.DescriptionWordCount;
                UIFunctions.PrintStatusUpdate(++currentAmountOfPairsDone, totalAmountOfPairs);
            }
            Console.Write("\rFilling in issue word count in sim_expanded_arch_emails_all_issues completed.\n");
            Console.WriteLine();
        }

        private static void FillInIssueWordCount(List<SimExpandedArchIssuesAllEmail> simExpandedArchIssuesAllEmailsPairs,
                                                                    List<DataJiraJiraIssue> listOfJiraIssues)
        {
            var totalAmountOfPairs = simExpandedArchIssuesAllEmailsPairs.Count();
            int currentAmountOfPairsDone = 0;
            Console.WriteLine("Starting filling in issue data in sim_expanded_arch_issues_all_emails...");
            foreach (var pair in simExpandedArchIssuesAllEmailsPairs)
            {
                var issue = listOfJiraIssues.Where(i => i.Key == pair.IssueKey).FirstOrDefault();
                pair.IssueDescriptionWordCount = issue?.DescriptionWordCount;
                UIFunctions.PrintStatusUpdate(++currentAmountOfPairsDone, totalAmountOfPairs);
            }
            Console.Write("\rFilling in issue data in sim_expanded_arch_issues_all_emails completed.\n");
            Console.WriteLine();
        }

        private static void FillInCreationTimeDifference(List<SimExpandedArchEmailsAllIssue> simExpandedArchEmailsAllIssuesPairs)
        {
            var totalAmountOfPairs = simExpandedArchEmailsAllIssuesPairs.Count();
            int currentAmountOfPairsDone = 0;
            Console.WriteLine("Starting filling in creation time differences in sim_expanded_arch_emails_all_issues...");
            foreach (var pair in simExpandedArchEmailsAllIssuesPairs)
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
            Console.Write("\rFilling in creation time differences in sim_expanded_arch_emails_all_issues completed.\n");
            Console.WriteLine();
        }

        private static void FillInCreationTimeDifference(List<SimExpandedArchIssuesAllEmail> simExpandedArchIssuesAllEmailsPairs)
        {
            var totalAmountOfPairs = simExpandedArchIssuesAllEmailsPairs.Count();
            int currentAmountOfPairsDone = 0;
            Console.WriteLine("Starting filling in creation time differences in sim_expanded_arch_issues_all_emails...");
            foreach (var pair in simExpandedArchIssuesAllEmailsPairs)
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
            Console.Write("\rFilling in creation time differences in sim_expanded_arch_issues_all_emails completed.\n");
            Console.WriteLine();
        }
    }
}
