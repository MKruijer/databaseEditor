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
    public static class Iter4Logic
    {
        public static void RunMultiThreadedFillInCreationTimeDifference(List<Iter4SenSimExpandedArchIssuesAllEmail> listOfIter4SimExpandedArchIssuesAllEmails,
                                                        int threadCount)
        { 

            // Divide the list sizes to determine the chunk size per thread
            int chunkSizeArchIssuesAllEmails = listOfIter4SimExpandedArchIssuesAllEmails.Count / threadCount;

            Parallel.ForEach(Partitioner.Create(0, threadCount), range =>
            {
                // Get the range for the current thread
                int startIndex = range.Item1 * chunkSizeArchIssuesAllEmails;
                int endIndex = range.Item2 * chunkSizeArchIssuesAllEmails;

                // Run the function with the subset of pairs
                FillInCreationTimeDifference(listOfIter4SimExpandedArchIssuesAllEmails.GetRange(startIndex, endIndex - startIndex));
            });
        }

        public static void RunMultiThreadedFillInCreationTimeDifference(List<Iter4CosSimExpandedArchIssuesAllEmail> listOfIter4SimExpandedArchIssuesAllEmails,
                                                        int threadCount)
        {

            // Divide the list sizes to determine the chunk size per thread
            int chunkSizeArchIssuesAllEmails = listOfIter4SimExpandedArchIssuesAllEmails.Count / threadCount;

            Parallel.ForEach(Partitioner.Create(0, threadCount), range =>
            {
                // Get the range for the current thread
                int startIndex = range.Item1 * chunkSizeArchIssuesAllEmails;
                int endIndex = range.Item2 * chunkSizeArchIssuesAllEmails;

                // Run the function with the subset of pairs
                FillInCreationTimeDifference(listOfIter4SimExpandedArchIssuesAllEmails.GetRange(startIndex, endIndex - startIndex));
            });
        }

        private static void FillInCreationTimeDifference(List<Iter4SenSimExpandedArchIssuesAllEmail> listOfPairs)
        {
            var totalAmountOfPairs = listOfPairs.Count();
            int currentAmountOfPairsDone = 0;
            Console.WriteLine("Starting filling in creation time differences in iter4_sen_sim_expanded_arch_issues_all_emails...");
            foreach (var pair in listOfPairs)
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
            Console.Write("\rFilling in creation time differences in iter4_sen_sim_expanded_arch_issues_all_emails completed.\n");
            Console.WriteLine();
        }

        private static void FillInCreationTimeDifference(List<Iter4CosSimExpandedArchIssuesAllEmail> listOfPairs)
        {
            var totalAmountOfPairs = listOfPairs.Count();
            int currentAmountOfPairsDone = 0;
            Console.WriteLine("Starting filling in creation time differences in iter4_cos_sim_expanded_arch_issues_all_emails...");
            foreach (var pair in listOfPairs)
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
            Console.Write("\rFilling in creation time differences in iter4_cos_sim_expanded_arch_issues_all_emails completed.\n");
            Console.WriteLine();
        }
    }
}
