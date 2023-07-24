using databaseEditor.Database;
using databaseEditor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace databaseEditor.Logic
{
    public static class Iter0Logic
    {
        public static void FillInCreationTimeDifference(List<Iter0ExpandedArchEmailsAllIssue> listOfExpandedArchEmailsAllIssuesPairs,
                                                               List<Iter0ExpandedArchIssuesAllEmail> listOfExpandedArchIssuesAllEmailsPairs)
        {    
            FillInCreationTimeDifference(listOfExpandedArchEmailsAllIssuesPairs);
            FillInCreationTimeDifference(listOfExpandedArchIssuesAllEmailsPairs);
        }
        
        private static void FillInCreationTimeDifference(List<Iter0ExpandedArchEmailsAllIssue> listOfExpandedArchEmailsAllIssuesPairs)
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

        private static void FillInCreationTimeDifference(List<Iter0ExpandedArchIssuesAllEmail> listOfExpandedArchIssuesAllEmailsPairs)
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
    }
}
