using databaseEditor.Models;
using Npgsql;
using System;
using System.Collections.Generic;
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
        }

        #region SharedFunctions

        public static void SetSmallestWordCount(string tableName)
        {
            Console.Write($"Setting smallest_word_count in {tableName}...");
            ExecuteSQL($"UPDATE {tableName} SET smallest_word_count = LEAST(issue_description_word_count, email_word_count)").Wait();
            Console.Write($"\rsmallest_word_count has been set in {tableName}.      \n");
        }

        public static void ApplyWordCountFilterExportAsNewTable(string newTableName, string sourceTableName)
        {
            Console.Write($"Creating new table '{newTableName}'...");
            ExecuteSQL($"CREATE TABLE IF NOT EXISTS {newTableName} AS " +
                $"SELECT * FROM {sourceTableName} " +
                $"WHERE smallest_word_count >= 50").Wait();
            ExecuteSQL($"ALTER TABLE {newTableName}" +
                $" ALTER COLUMN id SET NOT NULL," +
                $" ALTER COLUMN email_id SET NOT NULL," +
                $" ADD PRIMARY KEY (id)," +
                $" ALTER COLUMN issue_key SET NOT NULL;").Wait();
            Console.Write($"\rCreated table '{newTableName}'.      \n");
        }

        public static void ApplyCreationTimeDifferenceFilterExportAsNewTable(string newTableName, string sourceTableName)
        {
            Console.Write($"Creating new table '{newTableName}'...");
            ExecuteSQL($"CREATE TABLE IF NOT EXISTS {newTableName} AS " +
                $"SELECT * FROM {sourceTableName} " +
                $"WHERE creation_time_difference <= 500").Wait();
            ExecuteSQL($"ALTER TABLE {newTableName}" +
                $" ALTER COLUMN id SET NOT NULL," +
                $" ALTER COLUMN email_id SET NOT NULL," +
                $" ADD PRIMARY KEY (id)," +
                $" ALTER COLUMN issue_key SET NOT NULL;").Wait();
            Console.Write($"\rCreated table 'arch_emails_all_issues_word_and_creation_time_filtered'.      \n");
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

        #endregion SharedFunctions

        #region Iteration1Functions

        public static void CreateExpandedTables()
        {
            Console.Write("Creating tables...");
            List<String> createExpandedTablesSQL = new List<string>()
                {
                    "CREATE SEQUENCE IF NOT EXISTS expanded_arch_issues_all_emails_id_seq AS integer;",

                    "CREATE SEQUENCE IF NOT EXISTS expanded_arch_emails_all_issues_id_seq AS integer;",

                    "CREATE TABLE IF NOT EXISTS expanded_arch_issues_all_emails " +
                        "(id integer default nextval('expanded_arch_issues_all_emails_id_seq'::regclass) not null primary key," +
                        " email_id integer not null," +
                        " issue_key text not null," +
                        " similarity numeric," +
                        " email_date timestamp," +
                        " issue_created timestamp," +
                        " issue_modified timestamp," +
                        " email_word_count integer," +
                        " issue_description_word_count integer," +
                        " smallest_word_count integer," +
                        " email_thread_id integer," +
                        " issue_parent_key text," +
                        " creation_time_difference integer);",

                    "CREATE TABLE IF NOT EXISTS expanded_arch_emails_all_issues" +
                    "(id integer default nextval('expanded_arch_emails_all_issues_id_seq'::regclass) not null primary key," +
                        " email_id integer not null," +
                        " issue_key text not null," +
                        " similarity numeric," +
                        " email_date timestamp," +
                        " issue_created timestamp," +
                        " issue_modified timestamp," +
                        " email_word_count integer," +
                        " issue_description_word_count integer," +
                        " smallest_word_count integer," +
                        " email_thread_id integer," +
                        " issue_parent_key text," +
                        " creation_time_difference integer);"
                };
            createExpandedTablesSQL.ForEach(queryString => ExecuteSQL(queryString).Wait());
            Console.Write("\rCreated tables.      \n");
        }

        public static void InsertInExpandedTables()
        {
            Console.Write("Inserting into tables...");
            List<String> fillInExpandedTablesSQL = new List<string>()
                {
                    "INSERT INTO expanded_arch_emails_all_issues (email_id, issue_key, similarity) " +
                    "SELECT email_id, issue_key, similarity FROM result_arch_emails_all_issues WHERE similarity > 0.1;",

                    "INSERT INTO expanded_arch_issues_all_emails (email_id, issue_key, similarity)" +
                    "SELECT email_id, issue_key, similarity FROM result_arch_issues_all_emails WHERE similarity > 0.1;"
                };
            fillInExpandedTablesSQL.ForEach(queryString => ExecuteSQL(queryString).Wait());
            Console.Write("\rInserted into tables.      \n");
        }

        #endregion Iteration1Functions

        #region Iteration2Functions

        public static void CreateExpandedSimilarityTables()
        {
            Console.Write("Creating tables...");
            List<String> createExpandedTablesSQL = new List<string>()
                {
                    "CREATE SEQUENCE IF NOT EXISTS sim_expanded_arch_issues_all_emails_id_seq AS integer;",

                    "CREATE SEQUENCE IF NOT EXISTS sim_expanded_arch_emails_all_issues_id_seq AS integer;",

                    "CREATE TABLE IF NOT EXISTS sim_expanded_arch_issues_all_emails " +
                        "(id integer default nextval('sim_expanded_arch_issues_all_emails_id_seq'::regclass) not null primary key," +
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

                    "CREATE TABLE IF NOT EXISTS sim_expanded_arch_emails_all_issues" +
                    "(id integer default nextval('sim_expanded_arch_emails_all_issues_id_seq'::regclass) not null primary key," +
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
                        " creation_time_difference integer);"
                };
            createExpandedTablesSQL.ForEach(queryString => ExecuteSQL(queryString).Wait());
            Console.Write("\rCreated tables.      \n");
        }

        public static void InsertInExpandedSimilarityTables()
        {
            Console.Write("Inserting into tables...");
            List<String> fillInExpandedTablesSQL = new List<string>()
                {
                    """
                    INSERT INTO sim_expanded_arch_emails_all_issues (email_id, issue_key, similarity, email_date, email_thread_id, issue_created, issue_parent_key)
                    SELECT
                        s.email_id,
                        s.issue_key,
                        s.similarity,
                        e.date,
                        e.thread_id,
                        j.created,
                        j.parent_key
                    FROM
                        sim_result_arch_emails_all_issues AS s
                        JOIN data_email_email AS e ON s.email_id = e.id
                        JOIN data_jira_jira_issue AS j ON s.issue_key = j.key
                    WHERE
                        s.similarity > 0.35;
                    
                    """,
                    """
                    INSERT INTO sim_expanded_arch_issues_all_emails (email_id, issue_key, similarity, email_date, email_thread_id, issue_created, issue_parent_key)
                    SELECT
                        s.email_id,
                        s.issue_key,
                        s.similarity,
                        e.date,
                        e.thread_id,
                        j.created,
                        j.parent_key
                    FROM
                        sim_result_arch_issues_all_emails AS s
                        JOIN data_email_email AS e ON s.email_id = e.id
                        JOIN data_jira_jira_issue AS j ON s.issue_key = j.key
                    WHERE
                        s.similarity > 0.35;

                    """
                };
            fillInExpandedTablesSQL.ForEach(queryString => ExecuteSQL(queryString).Wait(TimeSpan.FromSeconds(300)));
            Console.Write("\rInserted into tables.      \n");
        }

        #endregion Iteration2Functions

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

        public static List<ExpandedArchEmailsAllIssue> GetExpandedArchEmailsAllIssues(RelationsDbContext dbContext)
        {
            Console.Write("Loading expanded ArchEmailsAllIssues table...");
            var listOfExpandedArchEmailsAllIssuesPairs = dbContext.ExpandedArchEmailsAllIssues.ToList();
            Console.Write("\rLoaded expanded ArchEmailsAllIssues table.      \n");
            return listOfExpandedArchEmailsAllIssuesPairs;
        }

        public static List<ExpandedArchIssuesAllEmail> GetExpandedArchIssuesAllEmails(RelationsDbContext dbContext)
        {
            Console.Write("Loading expanded ArchIssuesAllEmails table...");
            var listOfExpandedArchEmailsAllIssuesPairs = dbContext.ExpandedArchIssuesAllEmails.ToList();
            Console.Write("\rLoaded expanded ArchIssuesAllEmails table.      \n");
            return listOfExpandedArchEmailsAllIssuesPairs;
        }

        public static List<ArchEmailsAllIssuesWordAndCreationTimeFiltered> GetMaxFilteredArchEmailAllIssue(RelationsDbContext dbContext)
        {
            Console.Write("Loading max filtered ArchIssuesAllEmails table...");
            var listOfMaxFilteredArchEmailsAllIssuesPairs = dbContext.ArchEmailsAllIssuesWordAndCreationTimeFiltereds.ToList();
            Console.Write("\rLoaded max filtered ArchIssuesAllEmails table.      \n");
            return listOfMaxFilteredArchEmailsAllIssuesPairs;
        }

        public static List<ArchIssuesAllEmailsWordAndCreationTimeFiltered> GetMaxFilteredArchIssueAllEmail(RelationsDbContext dbContext)
        {
            Console.Write("Loading max filtered ArchIssuesAllEmails table...");
            var listOfMaxFilteredArchEmailsAllIssuesPairs = dbContext.ArchIssuesAllEmailsWordAndCreationTimeFiltereds.ToList();
            Console.Write("\rLoaded max filtered ArchIssuesAllEmails table.      \n");
            return listOfMaxFilteredArchEmailsAllIssuesPairs;
        }

        public static List<SimArchEmailsAllIssuesWordAndCreationTimeFiltered> GetSimMaxFilteredArchEmailAllIssue(RelationsDbContext dbContext)
        {
            Console.Write("Loading sim max filtered ArchIssuesAllEmails table...");
            var SimMaxFilteredSimArchEmailsAllIssuesPairs = dbContext.SimArchEmailsAllIssuesWordAndCreationTimeFiltereds.ToList();
            Console.Write("\rLoaded sim max filtered ArchIssuesAllEmails table.      \n");
            return SimMaxFilteredSimArchEmailsAllIssuesPairs;
        }

        public static List<SimArchIssuesAllEmailsWordAndCreationTimeFiltered> GetSimMaxFilteredArchIssueAllEmail(RelationsDbContext dbContext)
        {
            Console.Write("Loading sim max filtered ArchIssuesAllEmails table...");
            var SimMaxFilteredSimArchIssuesAllEmailsPairs = dbContext.SimArchIssuesAllEmailsWordAndCreationTimeFiltereds.ToList();
            Console.Write("\rLoaded sim max filtered ArchIssuesAllEmails table.      \n");
            return SimMaxFilteredSimArchIssuesAllEmailsPairs;
        }

        public static List<SimExpandedArchEmailsAllIssue> GetSimExpandedArchEmailsAllIssues(RelationsDbContext dbContext)
        {
            Console.Write("Loading sim_expanded_arch_emails_all_issues table...");
            var listToReturn = dbContext.SimExpandedArchEmailsAllIssues.ToList();
            Console.Write("\rLoaded sim_expanded_arch_emails_all_issues table.      \n");
            return listToReturn;
        }

        public static List<SimExpandedArchIssuesAllEmail> GetSimExpandedArchIssuesAllEmails(RelationsDbContext dbContext)
        {
            Console.Write("Loading sim_expanded_arch_issues_all_emails table...");
            var listToReturn = dbContext.SimExpandedArchIssuesAllEmails.ToList();
            Console.Write("\rLoaded sim_expanded_arch_issues_all_emails table.      \n");
            return listToReturn;
        }

        #endregion GetTables
    }
}
