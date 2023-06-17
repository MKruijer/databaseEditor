using databaseEditor.Database;
using databaseEditor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace databaseEditor.Logic
{
    public static class SimExpandedTableLogic
    {
        public static void FillInAdditionalDataInExpandedTable(List<DataEmailEmail> listOfEmails,
                                                               List<DataJiraJiraIssue> listOfJiraIssues,
                                                               List<SimExpandedArchEmailsAllIssue> listOfSimExpandedArchEmailsAllIssues,
                                                               List<SimExpandedArchIssuesAllEmail> listOfSimExpandedArchIssuesAllEmails)
        {
            FillInNewEmailData(listOfSimExpandedArchEmailsAllIssues, listOfEmails);
            FillInNewEmailData(listOfSimExpandedArchIssuesAllEmails, listOfEmails);

            FillInNewIssueData(listOfSimExpandedArchEmailsAllIssues, listOfJiraIssues);
            FillInNewIssueData(listOfSimExpandedArchIssuesAllEmails, listOfJiraIssues);

            FillInCreationTimeDifference(listOfSimExpandedArchEmailsAllIssues);
            FillInCreationTimeDifference(listOfSimExpandedArchIssuesAllEmails);

            DatabaseFunctions.SetSmallestWordCount();
        }

        private static void FillInNewEmailData(List<SimExpandedArchEmailsAllIssue> simExpandedArchEmailsAllIssuesPairs,
                                               List<DataEmailEmail> listOfEmails)
        {
            var totalAmountOfPairs = simExpandedArchEmailsAllIssuesPairs.Count();
            int currentAmountOfPairsDone = 0;
            Console.WriteLine("Starting filling in email data in sim_expanded_arch_emails_all_issues...");
            foreach (var pair in simExpandedArchEmailsAllIssuesPairs)
            {
                var email = listOfEmails.Where(e => e.Id == pair.EmailId).FirstOrDefault();
                pair.EmailWordCount = email?.WordCount;
                pair.EmailThreadId = email?.ThreadId;
                pair.EmailDate = email?.Date;
                UIFunctions.PrintStatusUpdate(++currentAmountOfPairsDone, totalAmountOfPairs);
            }
            Console.Write("\rFilling in email data in sim_expanded_arch_emails_all_issues completed.\n");
            Console.WriteLine();
        }

        private static void FillInNewEmailData(List<SimExpandedArchIssuesAllEmail> simExpandedArchIssuesAllEmailsPairs,
                                               List<DataEmailEmail> listOfEmails)
        {
            var totalAmountOfPairs = simExpandedArchIssuesAllEmailsPairs.Count();
            int currentAmountOfPairsDone = 0;
            Console.WriteLine("Starting filling in email data in sim_expanded_arch_issues_all_emails...");
            foreach (var pair in simExpandedArchIssuesAllEmailsPairs)
            {
                var email = listOfEmails.Where(e => e.Id == pair.EmailId).FirstOrDefault();
                pair.EmailWordCount = email?.WordCount;
                pair.EmailThreadId = email?.ThreadId;
                pair.EmailDate = email?.Date;
                UIFunctions.PrintStatusUpdate(++currentAmountOfPairsDone, totalAmountOfPairs);
            }
            Console.Write("\rFilling in email data in sim_expanded_arch_issues_all_emails completed.\n");
            Console.WriteLine();
        }

        private static void FillInNewIssueData(List<SimExpandedArchEmailsAllIssue> simExpandedArchEmailsAllIssuesPairs,
                                                                    List<DataJiraJiraIssue> listOfJiraIssues)
        {
            var totalAmountOfPairs = simExpandedArchEmailsAllIssuesPairs.Count();
            int currentAmountOfPairsDone = 0;
            Console.WriteLine("Starting filling in issue data in sim_expanded_arch_emails_all_issues...");
            foreach (var pair in simExpandedArchEmailsAllIssuesPairs)
            {
                var issue = listOfJiraIssues.Where(i => i.Key == pair.IssueKey).FirstOrDefault();
                pair.IssueDescriptionWordCount = issue?.DescriptionWordCount;
                pair.IssueCreated = issue?.Created;
                pair.IssueParentKey = issue?.ParentKey;
                UIFunctions.PrintStatusUpdate(++currentAmountOfPairsDone, totalAmountOfPairs);
            }
            Console.Write("\rFilling in issue data in sim_expanded_arch_emails_all_issues completed.\n");
            Console.WriteLine();
        }

        private static void FillInNewIssueData(List<SimExpandedArchIssuesAllEmail> simExpandedArchIssuesAllEmailsPairs,
                                                                    List<DataJiraJiraIssue> listOfJiraIssues)
        {
            var totalAmountOfPairs = simExpandedArchIssuesAllEmailsPairs.Count();
            int currentAmountOfPairsDone = 0;
            Console.WriteLine("Starting filling in issue data in sim_expanded_arch_issues_all_emails...");
            foreach (var pair in simExpandedArchIssuesAllEmailsPairs)
            {
                var issue = listOfJiraIssues.Where(i => i.Key == pair.IssueKey).FirstOrDefault();
                pair.IssueDescriptionWordCount = issue?.DescriptionWordCount;
                pair.IssueCreated = issue?.Created;
                pair.IssueParentKey = issue?.ParentKey;
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
