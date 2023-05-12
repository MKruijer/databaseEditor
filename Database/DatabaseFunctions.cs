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
        public static PostgresContext GetPostgresContext()
        {
            Console.Write("Setting up DB connection...");
            var db = new PostgresContext();
            Console.Write("\rDB connection succesful.                \n");
            return db;
        }

        public static void SaveDatabase(PostgresContext dbContext)
        {
            Console.Write("Saving DB changes...");
            dbContext.SaveChanges();
            Console.Write("\rDB changes succesfully saved.\n");
        }

        public static List<DataEmailEmail> GetEmails(PostgresContext dbContext)
        {
            Console.Write("Loading email data...");
            var listOfEmails = dbContext.DataEmailEmails.ToList();
            Console.Write("\rLoaded email data.      \n");
            return listOfEmails;
        }

        public static async Task ExecuteSQL(string sql, string? tableName = null)
        {
            var con = new NpgsqlConnection(
                        connectionString: "Host=localhost;Database=postgres;Username=postgres;Password=UnsavePassword");
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
