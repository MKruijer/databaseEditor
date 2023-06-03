using databaseEditor.jira;
using databaseEditor.Models;
namespace databaseEditor.Logic
{
    /// <summary>
    /// 
    /// </summary>
    public static class SourceTableLogic
    {
        public static void FillInSourceTables(List<DataEmailEmail> listOfEmails, List<DataJiraJiraIssue> listOfJiraIssues)
        {
            FillEmailThreadIds(listOfEmails);
            FillInEmailWordCount(listOfEmails);
            FillInJiraWordCount(listOfJiraIssues);
        }

        private static void FillEmailThreadIds(List<DataEmailEmail> listOfEmails)
        {
            var totalAmountOfEmails = listOfEmails.Count();
            int currentAmountOfEmailsDone = 0;
            Console.WriteLine("Starting filling in thread id...");
            foreach (var email in listOfEmails)
            {
                try
                {
                    email.ThreadId = GetEmailTopParentIdRecursively(listOfEmails, email);
                } catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    email.ThreadId = null;
                }
                UIFunctions.PrintStatusUpdate(++currentAmountOfEmailsDone, totalAmountOfEmails);
            }
            Console.Write("\rFilling in thread id completed.\n");
        }

        private static int GetEmailTopParentIdRecursively(List<DataEmailEmail> listOfEmails, DataEmailEmail email)
        {
            if (email.ParentId == null)
            {
                return email.Id;
            }
            else
            {
                var parentEmail = listOfEmails
                        .Where(e => e.Id == email.ParentId)
                        .FirstOrDefault();
                if (parentEmail == null)
                {
                    throw new Exception($"Email with id {email.Id} has parent but can't find parent. Probably an invalid dataset.");
                }
                return GetEmailTopParentIdRecursively(listOfEmails, parentEmail);
            }
        }

        private static void FillInEmailWordCount(List<DataEmailEmail> listOfEmails)
        {
            var totalAmountOfEmails = listOfEmails.Count();
            int currentAmountOfEmailsDone = 0;
            Console.WriteLine("Starting filling in word count...");
            foreach (var email in listOfEmails)
            {
                if (email.Body == null)
                {
                    email.WordCount = 0;
                    return;
                }
                char[] delimiters = new char[] { ' ', '\r', '\n' };
                email.WordCount = email.Body.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length;
                UIFunctions.PrintStatusUpdate(++currentAmountOfEmailsDone, totalAmountOfEmails);
            }
            Console.WriteLine("\nFilling in word count completed.");
        }

        private static void FillInJiraWordCount(List<DataJiraJiraIssue> listOfJiraJiraIssues)
        {
            var totalAmountOfJiraJiraIssues = listOfJiraJiraIssues.Count();
            int currentAmountOfJiraJiraIssuesDone = 0;
            Console.WriteLine("Starting filling in word count...");
            foreach (var JiraJiraIssue in listOfJiraJiraIssues)
            {
                if (JiraJiraIssue.Description == null)
                {
                    JiraJiraIssue.DescriptionWordCount = 0;
                    return;
                }
                char[] delimiters = new char[] { ' ', '\r', '\n' };
                JiraJiraIssue.DescriptionWordCount = JiraJiraIssue.Description.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length;
                UIFunctions.PrintStatusUpdate(++currentAmountOfJiraJiraIssuesDone, totalAmountOfJiraJiraIssues);
            }
            Console.Write("\rFilling in word count completed.\n");
        }
    }
}
