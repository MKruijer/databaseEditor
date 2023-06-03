using Atlassian.Jira;
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
        private const string APACHE_JIRA_PASSWORD = "Eju9uhY2";
        public static IDictionary<string, Issue> GetParentDictionaryFromJiraIssues(List<string> issueKeys)
        {
            var jira = Jira.CreateRestClient(JIRA_SERVER_URL, APACHE_JIRA_USERNAME, APACHE_JIRA_PASSWORD);
            jira.Issues.MaxIssuesPerRequest = MAX_ISSUES_PER_REQUEST;
            return jira.Issues.GetIssuesAsync(issueKeys).GetAwaiter().GetResult();
        }
    }
}
