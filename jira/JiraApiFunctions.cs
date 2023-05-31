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
        private const string JIRASERVER = "https://issues.apache.org/jira/";
        private const int MAXISSUESPERREQUEST = 1000;
        public static IDictionary<string, Issue> GetParentDictionaryFromJiraIssues(List<string> issueKeys)
        {
            var jira = Jira.CreateRestClient(JIRASERVER, "MKruijerStudent", "Eju9uhY2");
            jira.Issues.MaxIssuesPerRequest = MAXISSUESPERREQUEST;
            return jira.Issues.GetIssuesAsync(issueKeys).GetAwaiter().GetResult();            
        }
    }
}
