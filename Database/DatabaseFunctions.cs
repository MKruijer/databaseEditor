using databaseEditor.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace databaseEditor.Database
{
    public static class DatabaseFunctions
    {
        public static RelationsDbContext GetPostgresContext()
        {
            Console.Write("Setting up DB connection...");
            var db = new RelationsDbContext();
            Console.Write("\rDB connection succesful.                \n");
            return db;
        }

        public static void SaveDatabase(RelationsDbContext dbContext)
        {
            Console.Write("Saving DB changes...");
            dbContext.SaveChanges();
            Console.Write("\rDB changes succesfully saved.\n");
        }

        private static async Task ExecuteSQL(string sql)
        {
            var con = new NpgsqlConnection(
                        connectionString: "Host=localhost;Database=relationsDB;Username=postgres;Password=UnsavePassword");
            con.Open();
            var command = new NpgsqlCommand()
            {
                Connection = con,
                CommandText = sql,
                CommandTimeout = 300
            };
            await command.ExecuteNonQueryAsync();
            con.Close();
        }

        #region UpdateFunctions

        public static NpgsqlConnection GetPostgresConnection()
        {
            var con = new NpgsqlConnection(
                        connectionString: "Host=localhost;Database=relationsDB;Username=postgres;Password=UnsavePassword");
            con.Open();
            return con;
        }

        public static async Task<int> UpdateJiraCategory(NpgsqlConnection con, string sql)
        {
            var command = new NpgsqlCommand()
            {
                Connection = con,
                CommandText = sql,
                CommandTimeout = 300
            };
            var result = await command.ExecuteNonQueryAsync();
            return result;
        }

        #endregion UpdateFunctions

        #region SharedFunctions

        public static void ApplyWordCountFilterByRemoval(int wordCountLowerBound, string tableName)
        {
            Console.Write($"Applying word count filter in table '{tableName}'...");
            ExecuteSQL($"""
                DELETE FROM {tableName}
                WHERE smallest_word_count < {wordCountLowerBound};
                """).Wait();
            Console.Write($"\rApplyied word count filter in table '{tableName}'.   \n");
        }

        public static void ApplyCreationTimeFilterByRemoval(int creationTimeUpperBound, string tableName)
        {
            Console.Write($"Applying creation time filter in table '{tableName}'...");
            ExecuteSQL($"""
                DELETE FROM {tableName}
                WHERE creation_time_difference > {creationTimeUpperBound};
                """).Wait();
            Console.Write($"\rApplyied creation time filter in table '{tableName}'.   \n");
        }

        public static void ApplyDuplicationFilterExportAsNewTable(string newTableName, string sourceTableName)
        {
            Console.Write($"Creating new table '{newTableName}'...");
            ExecuteSQL($"""
                CREATE TABLE {newTableName} AS
                SELECT t.id, t.email_id, t.issue_key, t.similarity, t.smallest_word_count, t.creation_time_difference, t.email_thread_id, t.issue_parent_key
                FROM (
                  SELECT email_thread_id, issue_parent_key, MAX(similarity) AS max_similarity
                  FROM {sourceTableName}
                  GROUP BY email_thread_id, issue_parent_key
                ) AS subq
                JOIN {sourceTableName} AS t
                  ON t.email_thread_id = subq.email_thread_id
                  AND t.issue_parent_key = subq.issue_parent_key
                  AND t.similarity = subq.max_similarity
                ORDER BY t.similarity DESC;
                """).Wait();
            ExecuteSQL($"""
                ALTER TABLE {newTableName}
                ALTER COLUMN id SET NOT NULL,
                ALTER COLUMN email_id SET NOT NULL,
                ALTER COLUMN issue_key SET NOT NULL,
                ADD PRIMARY KEY (id);
                """).Wait();
            Console.Write($"\rCreated table '{newTableName}'.      \n");
        }

        /// <summary>
        /// Creates a new table with extra columns which are required for filtering
        /// </summary>
        /// <param name="newTableName">Name of the new table</param>
        public static void CreateExpandedSimilarityTables(string newTableName)
        {
            Console.Write("Creating tables...");
            List<String> createExpandedTablesSQL = new List<string>()
                {
                    $"CREATE SEQUENCE IF NOT EXISTS {newTableName}_id_seq AS integer;",

                    $"CREATE TABLE IF NOT EXISTS {newTableName} " +
                        $"(id integer default nextval('{newTableName}_id_seq'::regclass) not null primary key," +
                        " email_id integer not null," +
                        " issue_key text not null," +
                        " similarity numeric," +
                        " email_date timestamp," +
                        " issue_created timestamp," +
                        " email_word_count integer," +
                        " issue_description_word_count integer," +
                        " smallest_word_count integer," +
                        " email_thread_id integer," +
                        " issue_parent_key text," +
                        " creation_time_difference integer);",
                };
            createExpandedTablesSQL.ForEach(queryString => ExecuteSQL(queryString).Wait());
            Console.Write("\rCreated tables.      \n");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceTableName">Name of the table which contains the data</param>
        /// <param name="targetTableName">Name of the table in which the data gets insterted</param>
        /// <param name="similarityThreshold">Get pairs with a similarity score above this threshold</param>
        public static void InsertInExpandedSimilarityTables(string targetTableName, string sourceTableName, float similarityThreshold)
        {
            Console.Write("Inserting into tables...");
            var fillInExpandedTablesSQL =
                $"""
                    INSERT INTO {targetTableName} (email_id, issue_key, similarity, email_date, issue_created, email_word_count, issue_description_word_count, smallest_word_count, email_thread_id, issue_parent_key)
                    SELECT
                        s.email_id,
                        s.issue_key,
                        s.similarity,
                        e.date,
                        j.created,
                        e.word_count,
                        j.description_word_count,
                        LEAST(j.issue_description_word_count, e.email_word_count)
                        e.thread_id,
                        j.parent_key
                    FROM
                        {sourceTableName} AS s
                        JOIN data_email_email AS e ON s.email_id = e.id
                        JOIN data_jira_jira_issue AS j ON s.issue_key = j.key
                    WHERE
                        s.similarity > {similarityThreshold.ToString(CultureInfo.InvariantCulture)};
                    
                    """;
            ExecuteSQL(fillInExpandedTablesSQL).Wait(TimeSpan.FromSeconds(300));
            Console.Write("\rInserted into tables.      \n");
        }


        #endregion SharedFunctions

        #region Iteration3Functions

        public static void CreateAverageSimilarityArchEmailsAllIssues(int iteration, string sourceTableCosineSimilarity, string sourceTableSentenceSimilarity)
        {
            var sql = $"""
                CREATE TABLE iter{iteration}_average_similarity_arch_emails_all_issues AS
                SELECT
                    t1.id,
                    t1.email_id,
                    t1.issue_key,
                    t1.smallest_word_count,
                    t1.creation_time_difference,
                    t1.email_thread_id,
                    t1.issue_parent_key,
                    t1.similarity as sentence_similarity,
                    t2.similarity AS cosine_similarity,
                    (t1.similarity + t2.similarity) / 2 AS average_similarity
                FROM
                    {sourceTableSentenceSimilarity} AS t1
                    JOIN {sourceTableCosineSimilarity} AS t2 ON t1.email_id = t2.email_id AND t1.issue_key = t2.issue_key;
                
                """;
            ExecuteSQL(sql).Wait();
        }
        public static void CreateAverageSimilarityArchIssuesAllEmails(int iteration, string sourceTableCosineSimilarity, string sourceTableSentenceSimilarity)
        {
            var sql = $"""
                CREATE TABLE iter{iteration}_average_similarity_arch_issues_all_emails AS
                SELECT
                    t1.id,
                    t1.email_id,
                    t1.issue_key,
                    t1.smallest_word_count,
                    t1.creation_time_difference,
                    t1.email_thread_id,
                    t1.issue_parent_key,
                    t1.similarity as sentence_similarity,
                    t2.similarity AS cosine_similarity,
                    (t1.similarity + t2.similarity) / 2 AS average_similarity
                FROM
                    {sourceTableSentenceSimilarity} AS t1
                    JOIN {sourceTableCosineSimilarity} AS t2 ON t1.email_id = t2.email_id AND t1.issue_key = t2.issue_key;
                
                """;
            ExecuteSQL(sql).Wait();
        }

        #endregion Iteration3Functions

        #region Iteration4Functions

        public static void CreateFilteredArchIssuesAllEmails(string sourceTableName, string newTableName)
        {
            var sql = $"""
                BEGIN TRANSACTION;

                CREATE TABLE IF NOT EXISTS {newTableName} AS
                    SELECT t.id, t.email_id, t.issue_key, t.similarity, t.smallest_word_count, t.creation_time_difference, t.email_thread_id, t.issue_parent_key
                    FROM (
                        SELECT email_thread_id, issue_parent_key, MAX(similarity) AS max_similarity
                        FROM {sourceTableName}
                        WHERE smallest_word_count >= 50
                        GROUP BY email_thread_id, issue_parent_key
                    ) AS subq
                    JOIN {sourceTableName} AS t
                    ON t.email_thread_id = subq.email_thread_id
                    AND t.issue_parent_key = subq.issue_parent_key
                    AND t.similarity = subq.max_similarity
                    WHERE t.creation_time_difference <= 500
                    ORDER BY t.similarity DESC;

                ALTER TABLE {newTableName}
                    ALTER COLUMN id SET NOT NULL,
                    ALTER COLUMN email_id SET NOT NULL,
                    ALTER COLUMN issue_key SET NOT NULL,
                    ADD PRIMARY KEY (id);

                COMMIT;
                """;
            ExecuteSQL(sql).Wait(TimeSpan.FromSeconds(600));
        }

        #endregion Iteration4Functions

        #region GetTables
        public static List<DataEmailEmail> GetEmails(RelationsDbContext dbContext)
        {
            Console.Write("Loading email table...");
            var listOfEmails = dbContext.DataEmailEmails.ToList();
            Console.Write("\rLoaded email table.      \n");
            return listOfEmails;
        }

        public static List<DataJiraJiraIssue> GetJiraIssues(RelationsDbContext dbContext)
        {
            Console.Write("Loading JIRA issues table...");
            var listOfJiraIssues = dbContext.DataJiraJiraIssues.ToList();
            Console.Write("\rLoaded Jira issues table.      \n");
            return listOfJiraIssues;
        }

        public static List<Iter1ExpandedArchEmailsAllIssue> GetExpandedArchEmailsAllIssues(RelationsDbContext dbContext)
        {
            Console.Write("Loading expanded ArchEmailsAllIssues table...");
            var listOfExpandedArchEmailsAllIssuesPairs = dbContext.Iter1ExpandedArchEmailsAllIssues.ToList();
            Console.Write("\rLoaded expanded ArchEmailsAllIssues table.      \n");
            return listOfExpandedArchEmailsAllIssuesPairs;
        }

        public static List<Iter1ExpandedArchIssuesAllEmail> GetExpandedArchIssuesAllEmails(RelationsDbContext dbContext)
        {
            Console.Write("Loading expanded ArchIssuesAllEmails table...");
            var listOfExpandedArchEmailsAllIssuesPairs = dbContext.Iter1ExpandedArchIssuesAllEmails.ToList();
            Console.Write("\rLoaded expanded ArchIssuesAllEmails table.      \n");
            return listOfExpandedArchEmailsAllIssuesPairs;
        }

        public static List<Iter2SimExpandedArchEmailsAllIssue> GetSimExpandedArchEmailsAllIssues(RelationsDbContext dbContext)
        {
            Console.Write("Loading sim_expanded_arch_emails_all_issues table...");
            var listToReturn = dbContext.Iter2SimExpandedArchEmailsAllIssues.ToList();
            Console.Write("\rLoaded sim_expanded_arch_emails_all_issues table.      \n");
            return listToReturn;
        }

        public static List<Iter2SimExpandedArchIssuesAllEmail> GetSimExpandedArchIssuesAllEmails(RelationsDbContext dbContext)
        {
            Console.Write("Loading sim_expanded_arch_issues_all_emails table...");
            var listToReturn = dbContext.Iter2SimExpandedArchIssuesAllEmails.ToList();
            Console.Write("\rLoaded sim_expanded_arch_issues_all_emails table.      \n");
            return listToReturn;
        }

        public static List<Iter4SenSimExpandedArchIssuesAllEmail> GetIter4SenSimExpandedArchIssuesAllEmails(RelationsDbContext dbContext)
        {
            Console.Write("Loading iter4_sen_sim_expanded_arch_issues_all_emails table...");
            var listToReturn = dbContext.Iter4SenSimExpandedArchIssuesAllEmails.ToList();
            Console.Write("\rLoaded iter4_sen_sim_expanded_arch_issues_all_emails table.      \n");
            return listToReturn;
        }

        public static List<Iter4CosSimExpandedArchIssuesAllEmail> GetIter4CosSimExpandedArchIssuesAllEmails(RelationsDbContext dbContext)
        {
            Console.Write("Loading iter4_cos_sim_expanded_arch_issues_all_emails table...");
            var listToReturn = dbContext.Iter4CosSimExpandedArchIssuesAllEmails.ToList();
            Console.Write("\rLoaded iter4_cos_sim_expanded_arch_issues_all_emails table.      \n");
            return listToReturn;
        }

        #endregion GetTables
    }
}
