using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace databaseEditor.jira
{
    public static class JiraApiObject
    {
        public static string GetTopParentJiraIssueKeyFromJiraIssueKey(string issueKey)
        {
            var currentIterIssueKey = issueKey;
            var parentIssueKey = issueKey;
            do
            {
                parentIssueKey = currentIterIssueKey;
                currentIterIssueKey = GetParentKeyFromIssue(parentIssueKey);
            } while (currentIterIssueKey != parentIssueKey);
            return currentIterIssueKey;
        }

        private static string GetParentKeyFromIssue(string issueKey)
        {
            var issueJson = RestCall(issueKey, "fields=parent");
            string? parentKey = GetParentFromJson(issueJson);
            if(string.Compare(issueKey,GetIssueKeyFromIssueJson(issueJson)) != 0)
            {
                throw new Exception("issueKey and jsonIssueKey are not equal");
            }
            if(parentKey == null)
            {
                return issueKey;
            } else
            {
                return parentKey;
            }
        }

        private static string? GetParentFromJson(string json)
        {
            using (JsonDocument document = JsonDocument.Parse(json))
            {
                // Access the root element
                JsonElement root = document.RootElement;

                // Check if the "fields" property exists
                if (root.TryGetProperty("fields", out JsonElement fields))
                {
                    // Check if the "parent" property exists within "fields"
                    if (fields.TryGetProperty("parent", out JsonElement parent))
                    {
                        // Check if the "key" property exists within "fields"
                        if (parent.TryGetProperty("key", out JsonElement key))
                        {
                            return key.GetString();
                        }
                    }
                }
                return null;
            }
        }

        private static string? GetIssueKeyFromIssueJson(string json)
        {
            using (JsonDocument document = JsonDocument.Parse(json))
            {
                // Access the root element
                JsonElement root = document.RootElement;

                // Check if the "fields" property exists
                if (root.TryGetProperty("key", out JsonElement key))
                {
                    return key.GetString();
                }
                return null;
            }
        }

        private static string RestCall(string issueKey, string? filter = null)
        {
            var result = string.Empty;
            try
            {
                var client = new RestClient($"https://issues.apache.org/jira/rest/api/2/issue/{issueKey}?{filter}");
                var request = new RestRequest
                {
                    Method = Method.GET,
                    RequestFormat = DataFormat.Json
                };
                //request.AddHeader("Authorization", "Basic " + api_token);
                var response = client.Execute(request);
                result = response.Content;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        

    }
}
