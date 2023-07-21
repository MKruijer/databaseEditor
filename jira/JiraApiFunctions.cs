using Atlassian.Jira;
using databaseEditor.Database;
using databaseEditor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace databaseEditor.jira
{
    public static class JiraApiFunctions
    {
        private const string JIRA_SERVER_URL = "https://issues.apache.org/jira/";
        private const int MAX_ISSUES_PER_REQUEST = 1000;
        private const string APACHE_JIRA_USERNAME = "MKruijerStudent";
        private const string APACHE_JIRA_PASSWORD = "UnSavePassword";

        public static void UpdateJiraParentKeys(RelationsDbContext db, bool doMax1000)
        {
            var batchSize = 1000;
            var listOfAllRemainingJiraIssues = 
                !doMax1000 
                ? DatabaseFunctions.GetJiraIssues(db)
                .Where(issue => issue.ParentKey == null)
                .ToList()
                : DatabaseFunctions.GetJiraIssues(db)
                .Where(issue => issue.ParentKey == null)
                .Take(1000)
                .ToList();

            for (int i = 0; i < listOfAllRemainingJiraIssues.Count; i += batchSize)
            {
                var listOfBatchJiraIssues = listOfAllRemainingJiraIssues.Skip(i).Take(batchSize).ToList();
                List<string> jiraKeyList = new List<string>();
                listOfBatchJiraIssues.ForEach(jiraIssue => jiraKeyList.Add(jiraIssue.Key));
                Console.Write("Fetching data from API...");
                var dictionary = GetParentDictionaryFromJiraIssues(jiraKeyList);
                Console.Write("\rFetched data from API.      \n");
                listOfBatchJiraIssues.ForEach(issue =>
                {
                    issue.ParentKey = dictionary[issue.Key].ParentIssueKey ?? issue.Key;
                });
                DatabaseFunctions.SaveDatabase(db);
                Thread.Sleep(60000);
            }
        }

        private static IDictionary<string, Issue> GetParentDictionaryFromJiraIssues(List<string> issueKeys)
        {
            var jira = Jira.CreateRestClient(JIRA_SERVER_URL, APACHE_JIRA_USERNAME, APACHE_JIRA_PASSWORD);
            jira.Issues.MaxIssuesPerRequest = MAX_ISSUES_PER_REQUEST;
            return jira.Issues.GetIssuesAsync(issueKeys).GetAwaiter().GetResult();
        }


    }
}
