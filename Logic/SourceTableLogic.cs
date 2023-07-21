using databaseEditor.Database;
using databaseEditor.jira;
using databaseEditor.Models;
using System.Text.Json;

namespace databaseEditor.Logic
{
    /// <summary>
    /// 
    /// </summary>
    public static class SourceTableLogic
    {
        public static void UpdateJiraIssueCategories(List<DataJiraJiraIssue> listOfJiraIssues)
        {
            // Get the base directory of the application
            string currentDirectory = Directory.GetCurrentDirectory();
            string? projectDirectory = Directory.GetParent(currentDirectory)?.Parent?.Parent?.FullName;
            if(projectDirectory == null)
            {
                Console.WriteLine("Error getting directory.");
                return;
            }
            string jsonPath;
            Dictionary<string, Entry>? entries = new Dictionary<string, Entry>();
            // Construct the file path
            try
            {
                jsonPath = Path.Combine(projectDirectory, "textFiles", "updatedIssueCategories.json");
                if (!File.Exists(jsonPath))
                {
                    Console.WriteLine("File does not exist or the path is incorrect.");
                }
                else
                {
                    string jsonContent = File.ReadAllText(jsonPath);
                    entries = JsonSerializer.Deserialize<Dictionary<string, Entry>>(jsonContent);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            if (entries == null)
            {
                Console.WriteLine("Deserialized json is empty");
            }
            else
            {
                var con = DatabaseFunctions.GetPostgresConnection();
                Console.Write("Creating tasks...");
                var totalAmountOfPairs = entries.Count();
                int currentAmountOfPairsDone = 0;
                Console.WriteLine("Starting updating jira issue categories...");
                foreach (var entry in entries)
                {
                    var key = entry.Key;
                    var isExistence = entry.Value.label.existence;
                    var isProperty = entry.Value.label.property;
                    var isExecutive = entry.Value.label.executive;
                    var sql = $"UPDATE data_jira_jira_issue SET is_design = {isExistence || isExecutive || isProperty}::boolean, is_cat_existence = {isExistence}::boolean, is_cat_executive = {isExecutive}::boolean, is_cat_property = {isProperty}::boolean WHERE key = '{key}'";
                    var result = DatabaseFunctions.UpdateJiraCategory(con, sql).GetAwaiter().GetResult();
                    if (result != 1)
                    {
                        Console.WriteLine($"ERROR WITH ISSUE WITH KEY {key}");
                    }
                    UIFunctions.PrintStatusUpdate(++currentAmountOfPairsDone, totalAmountOfPairs);
                }
                Console.Write("\rUpdating jira issue categories completed.     \n");
                Console.WriteLine();
                con.Close();
            }
        }


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
