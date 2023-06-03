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
                CommandText = sql
            };
            await command.ExecuteNonQueryAsync();
        }

        public static void SetSmallestWordCount()
        {
            Console.Write("Setting smallest_word_count in tables...");
            ExecuteSQL("UPDATE expanded_arch_emails_all_issues SET smallest_word_count = LEAST(issue_description_word_count, email_word_count)").Wait();
            ExecuteSQL("UPDATE expanded_arch_issues_all_emails SET smallest_word_count = LEAST(issue_description_word_count, email_word_count)").Wait();
            Console.Write("\rsmallest_word_count has been set in tables.      \n");
        }

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

        public static void ApplyWordCountFilterExportAsNewTable()
        {
            Console.Write("Creating new table 'arch_emails_all_issues_word_filtered'...");
            ExecuteSQL("CREATE TABLE IF NOT EXISTS arch_emails_all_issues_word_filtered AS " +
                "SELECT * FROM expanded_arch_emails_all_issues " +
                "WHERE smallest_word_count >= 50").Wait();
            ExecuteSQL("ALTER TABLE arch_emails_all_issues_word_filtered" +
                " ALTER COLUMN id SET NOT NULL," +
                " ALTER COLUMN email_id SET NOT NULL," +
                " ADD PRIMARY KEY (id)," +
                " ALTER COLUMN issue_key SET NOT NULL;").Wait();
            Console.Write("\rCreated table 'arch_emails_all_issues_word_filtered'.      \n");

            Console.Write("Creating new table 'arch_issues_all_emails_word_filtered'...");
            ExecuteSQL("CREATE TABLE IF NOT EXISTS arch_issues_all_emails_word_filtered AS " +
                "SELECT * FROM expanded_arch_issues_all_emails " +
                "WHERE smallest_word_count >= 50").Wait();
            ExecuteSQL("ALTER TABLE arch_issues_all_emails_word_filtered" +
                " ALTER COLUMN id SET NOT NULL," +
                " ALTER COLUMN email_id SET NOT NULL," +
                " ADD PRIMARY KEY (id)," +
                " ALTER COLUMN issue_key SET NOT NULL;").Wait();
            Console.Write("\rCreated table 'arch_issues_all_emails_word_filtered'.      \n");
        }

        public static void ApplCreationTimeDifferenceFilterExportAsNewTable()
        {
            Console.Write("Creating new table 'arch_emails_all_issues_word_and_creation_time_filtered'...");
            ExecuteSQL("CREATE TABLE IF NOT EXISTS arch_emails_all_issues_word_and_creation_time_filtered AS " +
                "SELECT * FROM arch_emails_all_issues_word_filtered " +
                "WHERE creation_time_difference <= 700").Wait();
            ExecuteSQL("ALTER TABLE arch_emails_all_issues_word_and_creation_time_filtered" +
                " ALTER COLUMN id SET NOT NULL," +
                " ALTER COLUMN email_id SET NOT NULL," +
                " ADD PRIMARY KEY (id)," +
                " ALTER COLUMN issue_key SET NOT NULL;").Wait();
            Console.Write("\rCreated table 'arch_emails_all_issues_word_and_creation_time_filtered'.      \n");

            Console.Write("Creating new table 'arch_issues_all_emails_word_and_creation_time_filtered'...");
            ExecuteSQL("CREATE TABLE IF NOT EXISTS arch_issues_all_emails_word_and_creation_time_filtered AS " +
                "SELECT * FROM arch_issues_all_emails_word_filtered " +
                "WHERE creation_time_difference <= 700").Wait();
            ExecuteSQL("ALTER TABLE arch_issues_all_emails_word_and_creation_time_filtered" +
                " ALTER COLUMN id SET NOT NULL," +
                " ALTER COLUMN email_id SET NOT NULL," +
                " ADD PRIMARY KEY (id)," +
                " ALTER COLUMN issue_key SET NOT NULL;").Wait();
            Console.Write("\rCreated table 'arch_issues_all_emails_word_and_creation_time_filtered'.      \n");
        }

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

        #endregion GetTables
    }
}
