using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Caching;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace databaseEditor.jira
{
    public static class JiraApiObject
    {
        private static void Throttle()
        {
            var maxPerPeriod = 250;
            //If you utilize multiple accounts, you can throttle per account. If not, don't use this:
            var keyPrefix = "a_unique_id_for_the_basis_of_throttling";
            var intervalPeriod = 300000;//5 minutes
            var sleepInterval = 5000;//period to "sleep" before trying again (if the limits have been reached)
            var recentTransactions = MemoryCache.Default.Count(x => x.Key.StartsWith(keyPrefix));
            while (recentTransactions >= maxPerPeriod)
            {
                System.Threading.Thread.Sleep(sleepInterval);
                recentTransactions = MemoryCache.Default.Count(x => x.Key.StartsWith(keyPrefix));
            }
            var key = keyPrefix + "_" + DateTime.Now.ToUniversalTime().ToString("yyyyMMddHHmm");
            var existing = MemoryCache.Default.Where(x => x.Key.StartsWith(key));
            if (existing != null && existing.Any())
            {
                var counter = 2;
                var last = existing.OrderBy(x => x.Key).Last();
                var pieces = last.Key.Split('_');
                if (pieces.Count() > 2)
                {
                    var lastCount = 0;
                    if (int.TryParse(pieces[2], out lastCount))
                    {
                        counter = lastCount + 1;
                    }
                }
                key = key + "_" + counter;
            }
            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddMilliseconds(intervalPeriod)
            };
            MemoryCache.Default.Set(key, 1, policy);
        }

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

        private static string RestCall(string issueKey, string? filter = null)
        {
            Throttle();
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
                if(response.StatusCode == HttpStatusCode.TooManyRequests) {
                    Thread.Sleep(60000);
                }
                result = response.Content;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private static string? GetParentFromJson(string json)
        {
            try
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
            catch (JsonException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private static string? GetIssueKeyFromIssueJson(string json)
        {
            try
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
            catch (JsonException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
