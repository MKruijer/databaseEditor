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
        public static void RunMultiThreadedFillInCreationTimeDifference(List<SimExpandedArchEmailsAllIssue> listOfSimExpandedArchEmailsAllIssues,
                                                        List<SimExpandedArchIssuesAllEmail> listOfSimExpandedArchIssuesAllEmails,
                                                        int threadCount)
        {
            // Divide the list sizes to determine the chunk size per thread
            int chunkSizeArchEmailsAllIssues = listOfSimExpandedArchEmailsAllIssues.Count / threadCount;
            int chunkSizeArchIssuesAllEmails = listOfSimExpandedArchIssuesAllEmails.Count / threadCount;

            Parallel.ForEach(Partitioner.Create(0, threadCount), range =>
            {
                // Get the range for the current thread
                int startIndexArchEmailsAllIssues = range.Item1 * chunkSizeArchEmailsAllIssues;
                int startIndexArchIssuesAllEmails = range.Item1 * chunkSizeArchIssuesAllEmails;
                int endIndexArchEmailsAllIssues = range.Item2 * chunkSizeArchEmailsAllIssues;
                int endIndexArchIssuesAllEmails = range.Item2 * chunkSizeArchIssuesAllEmails;

                // Run the function with the subset of pairs
                FillInCreationTimeDifference(listOfSimExpandedArchEmailsAllIssues.GetRange(startIndexArchEmailsAllIssues, endIndexArchEmailsAllIssues - startIndexArchEmailsAllIssues));
                FillInCreationTimeDifference(listOfSimExpandedArchIssuesAllEmails.GetRange(startIndexArchIssuesAllEmails, endIndexArchIssuesAllEmails - startIndexArchIssuesAllEmails));
            });
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
