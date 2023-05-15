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

        public static List<DataEmailEmail> GetEmails(RelationsDbContext dbContext)
        {
            Console.Write("Loading email data...");
            var listOfEmails = dbContext.DataEmailEmails.ToList();
            Console.Write("\rLoaded email data.      \n");
            return listOfEmails;
        }

        public static List<DataJiraJiraIssue> GetJiraIssues(RelationsDbContext dbContext)
        {
            Console.Write("Loading JIRA issues data...");
            var listOfJiraIssues = dbContext.DataJiraJiraIssues.ToList();
            Console.Write("\rLoaded Jira issues data.      \n");
            return listOfJiraIssues;
        }

        public static List<ModifiedArchEmailsAllIssue> GetModifiedArchEmailsAllIssues(RelationsDbContext dbContext)
        {
            Console.Write("Loading modified ArchEmailsAllIssues table...");
            var listOfModifiedArchEmailsAllIssues = dbContext.ModifiedArchEmailsAllIssues.ToList();
            Console.Write("\rLoaded modified ArchEmailsAllIssues data.      \n");
            return listOfModifiedArchEmailsAllIssues;

        }

        public static List<ModifiedArchIssuesAllEmail> GetModifiedArchIssuesAllEmails(RelationsDbContext dbContext)
        {
            Console.Write("Loading modified ArchEmailsAllIssues table...");
            var listOfModifiedArchIssuesAllEmails = dbContext.ModifiedArchIssuesAllEmails.ToList();
            Console.Write("\rLoaded modified ArchEmailsAllIssues data.      \n");
            return listOfModifiedArchIssuesAllEmails;

        }

        public static async Task ExecuteSQL(string sql)
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
    }
}
