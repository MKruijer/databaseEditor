using databaseEditor.Database;
using databaseEditor.jira;
using databaseEditor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace databaseEditor.Logic
{
    public static class ModifiedTableLogic
    {
        public static void FillInArchIssuesAllEmailJiraParent(List<Models.ModifiedArchIssuesAllEmail> listOfModifiedResultsArchIssuesAllEmailPairs)
        {
            List<string> jiraKeyList = new List<string>();
            listOfModifiedResultsArchIssuesAllEmailPairs.ForEach(pair => jiraKeyList.Add(pair.IssueKey));
            var dictionary = JiraApiFunctions.GetParentDictionaryFromJiraIssues(jiraKeyList);
            listOfModifiedResultsArchIssuesAllEmailPairs.ForEach(pair =>
            {
                pair.IssueParentKey = dictionary[pair.IssueKey].ParentIssueKey ?? pair.IssueKey;
            });
        }

        public static void FillInArchEmailsAllIssueJiraParent(List<Models.ModifiedArchEmailsAllIssue> listOfModifiedResultsArchEmailsAllIssuePairs)
        {
            List<string> jiraKeyList = new List<string>();
            listOfModifiedResultsArchEmailsAllIssuePairs.ForEach(pair => jiraKeyList.Add(pair.IssueKey));
            var dictionary = JiraApiFunctions.GetParentDictionaryFromJiraIssues(jiraKeyList);
            listOfModifiedResultsArchEmailsAllIssuePairs.ForEach(pair =>
            {
                pair.IssueParentKey = dictionary[pair.IssueKey].ParentIssueKey ?? pair.IssueKey;
            });
        }

    }
}
