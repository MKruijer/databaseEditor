using databaseEditor.Database;
using databaseEditor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace databaseEditor.Logic
{
    public static class ExpandedTableLogic
    {
        public static void FillInAdditionalDataInExpandedTable(List<DataEmailEmail> listOfEmails,
                                                               List<DataJiraJiraIssue> listOfJiraIssues,
                                                               List<ExpandedArchEmailsAllIssue> listOfExpandedArchEmailsAllIssuesPairs,
                                                               List<ExpandedArchIssuesAllEmail> listOfExpandedArchIssuesAllEmailsPairs)
        {
            FillInNewEmailData(listOfExpandedArchEmailsAllIssuesPairs, listOfEmails);
            FillInNewEmailData(listOfExpandedArchIssuesAllEmailsPairs, listOfEmails);

            FillInNewIssueData(listOfExpandedArchEmailsAllIssuesPairs, listOfJiraIssues);
            FillInNewIssueData(listOfExpandedArchIssuesAllEmailsPairs, listOfJiraIssues);         

            FillInCreationTimeDifference(listOfExpandedArchEmailsAllIssuesPairs);
            FillInCreationTimeDifference(listOfExpandedArchIssuesAllEmailsPairs);

            DatabaseFunctions.SetSmallestWordCount();
        }

        private static void FillInNewIssueData(List<ExpandedArchEmailsAllIssue> listOfExpandedArchEmailsAllIssuesPairs,
                                                                    List<DataJiraJiraIssue> listOfJiraIssues)
        {
            var totalAmountOfPairs = listOfExpandedArchEmailsAllIssuesPairs.Count();
            int currentAmountOfPairsDone = 0;
            Console.WriteLine("Starting filling in issue data in ArchEmailsAllIssues...");
            foreach (var pair in listOfExpandedArchEmailsAllIssuesPairs)
            {
                var issue = listOfJiraIssues.Where(i => i.Key == pair.IssueKey).FirstOrDefault();
                pair.IssueDescriptionWordCount = issue?.DescriptionWordCount;
                pair.IssueCreated = issue?.Created;
                pair.IssueModified = issue?.Modified;
                pair.IssueParentKey = issue?.ParentKey;
                UIFunctions.PrintStatusUpdate(++currentAmountOfPairsDone, totalAmountOfPairs);
            }
            Console.Write("\rFilling in issue data in ArchEmailsAllIssues completed.\n");
            Console.WriteLine();
        }
        
        private static void FillInNewIssueData(List<ExpandedArchIssuesAllEmail> listOfExpandedArchIssuesAllEmailsPairs,
                                                                    List<DataJiraJiraIssue> listOfJiraIssues)
        {
            var totalAmountOfPairs = listOfExpandedArchIssuesAllEmailsPairs.Count();
            int currentAmountOfPairsDone = 0;
            Console.WriteLine("Starting filling in issue data in ArchIssuesAllEmails...");
            foreach (var pair in listOfExpandedArchIssuesAllEmailsPairs)
            {
                var issue = listOfJiraIssues.Where(i => i.Key == pair.IssueKey).FirstOrDefault();
                pair.IssueDescriptionWordCount = issue?.DescriptionWordCount;
                pair.IssueCreated = issue?.Created;
                pair.IssueModified = issue?.Modified;
                pair.IssueParentKey = issue?.ParentKey;
                UIFunctions.PrintStatusUpdate(++currentAmountOfPairsDone, totalAmountOfPairs);
            }
            Console.Write("\rFilling in issue data in ArchIssuesAllEmails completed.\n");
            Console.WriteLine();
        }

        private static void FillInNewEmailData(List<ExpandedArchEmailsAllIssue> listOfExpandedArchEmailsAllIssuesPairs,
                                                                    List<DataEmailEmail> listOfEmails)
        {
            var totalAmountOfPairs = listOfExpandedArchEmailsAllIssuesPairs.Count();
            int currentAmountOfPairsDone = 0;
            Console.WriteLine("Starting filling in email data in ArchEmailsAllIssues...");
            foreach (var pair in listOfExpandedArchEmailsAllIssuesPairs)
            {
                var email = listOfEmails.Where(e => e.Id == pair.EmailId).FirstOrDefault();
                pair.EmailWordCount = email?.WordCount;
                pair.EmailThreadId = email?.ThreadId;
                pair.EmailDate = email?.Date;
                UIFunctions.PrintStatusUpdate(++currentAmountOfPairsDone, totalAmountOfPairs);
            }
            Console.Write("\rFilling in email data in ArchEmailsAllIssues completed.\n");
            Console.WriteLine();
        }

        private static void FillInNewEmailData(List<ExpandedArchIssuesAllEmail> listOfExpandedArchIssuesAllEmailsPairs,
                                                                    List<DataEmailEmail> listOfEmails)
        {
            var totalAmountOfPairs = listOfExpandedArchIssuesAllEmailsPairs.Count();
            int currentAmountOfPairsDone = 0;
            Console.WriteLine("Starting filling in email data in ArchIssuesAllEmails...");
            foreach (var pair in listOfExpandedArchIssuesAllEmailsPairs)
            {
                var email = listOfEmails.Where(e => e.Id == pair.EmailId).FirstOrDefault();
                pair.EmailWordCount = email?.WordCount;
                pair.EmailThreadId = email?.ThreadId;
                pair.EmailDate = email?.Date;
                UIFunctions.PrintStatusUpdate(++currentAmountOfPairsDone, totalAmountOfPairs);
            }
            Console.Write("\rFilling in email data in ArchIssuesAllEmails completed.\n");
            Console.WriteLine();
        }

        private static void FillInCreationTimeDifference(List<ExpandedArchEmailsAllIssue> listOfExpandedArchEmailsAllIssuesPairs)
        {
            var totalAmountOfPairs = listOfExpandedArchEmailsAllIssuesPairs.Count();
            int currentAmountOfPairsDone = 0;
            Console.WriteLine("Starting filling in creation time differences in ArchEmailsAllIssues...");
            foreach (var pair in listOfExpandedArchEmailsAllIssuesPairs)
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
            Console.Write("\rFilling in creation time differences in ArchEmailsAllIssues completed.\n");
            Console.WriteLine();
        }

        private static void FillInCreationTimeDifference(List<ExpandedArchIssuesAllEmail> listOfExpandedArchIssuesAllEmailsPairs)
        {
            var totalAmountOfPairs = listOfExpandedArchIssuesAllEmailsPairs.Count();
            int currentAmountOfPairsDone = 0;
            Console.WriteLine("Starting filling in creation time differences in ArchIssuesAllEmails...");
            foreach (var pair in listOfExpandedArchIssuesAllEmailsPairs)
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
            Console.Write("\rFilling in creation time differences in ArchIssuesAllEmails completed.\n");
            Console.WriteLine();
        }

        public static void FillInJiraIssueParentKeyInArchEmailsAllIssues(List<ExpandedArchEmailsAllIssue> listOfExpandedArchEmailsAllIssuesPairs,
                                                                          List<DataJiraJiraIssue> listOfJiraIssues)
        {
            var totalAmountOfPairs = listOfJiraIssues.Where(issue => issue.ParentKey != null).Count();
            int currentAmountOfPairsDone = 0;
            Console.WriteLine("Starting filling in issue parent keys in ArchEmailsAllIssues...");
            listOfJiraIssues
                .Where(issue => issue.ParentKey != null)
                .ToList()
                .ForEach(issue =>
                {
                    listOfExpandedArchEmailsAllIssuesPairs
                    .Where(pair => pair.IssueParentKey == null && pair.IssueKey == issue.Key)
                    .ToList()
                    .ForEach(pair =>
                    {
                        pair.IssueParentKey = issue.ParentKey;
                    });
                    UIFunctions.PrintStatusUpdate(++currentAmountOfPairsDone, totalAmountOfPairs);
                });
            Console.Write("\rFilling in issue parent keys in ArchEmailsAllIssues completed.\n");
            Console.WriteLine();
        }

        public static void FillInJiraIssueParentKeyInArchIssuesAllEmail(List<ExpandedArchIssuesAllEmail> listOfExpandedArchIssuesAllEmailsPairs,
                                                                         List<DataJiraJiraIssue> listOfJiraIssues)
        {
            var totalAmountOfPairs = listOfJiraIssues.Where(issue => issue.ParentKey != null).Count();
            int currentAmountOfPairsDone = 0;
            Console.WriteLine("Starting filling in issue parent keys in ArchIssuesAllEmails...");
            listOfJiraIssues
                .Where(issue => issue.ParentKey != null)
                .ToList()
                .ForEach(issue =>
                {
                    listOfExpandedArchIssuesAllEmailsPairs
                    .Where(pair => pair.IssueParentKey == null && pair.IssueKey == issue.Key)
                    .ToList()
                    .ForEach(pair =>
                    {
                        pair.IssueParentKey = issue.ParentKey;
                    });
                    UIFunctions.PrintStatusUpdate(++currentAmountOfPairsDone, totalAmountOfPairs);
                });
            Console.Write("\rFilling in issue parent keys in ArchIssuesAllEmails completed.\n");
            Console.WriteLine();
        }
    }
}
