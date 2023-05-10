// See https://aka.ms/new-console-template for more information

using databaseEditor.Models;

Console.WriteLine("Hello, World!");
using var db = new PostgresContext();
Console.WriteLine("Reading email...");
var email = db.DataEmailEmails.Where(email => email.Id == 23618).FirstOrDefault();
if(email == null)
{
    Console.WriteLine("Error, no email found!\nShutting down.");
    return;
}
Console.WriteLine("Email found!\nAdding threadId.");
if (email.ParentId == null)
{
    Console.WriteLine("Email has no parent. Setting threadId as emailId...");
    email.ThreadId = (int)email.Id;
    return;
}
Console.WriteLine("Email has parent. Setting threadId as parentId...");
email.ThreadId = (int)email.ParentId;
Console.WriteLine("Added threadId.\nSaving db...");
db.SaveChanges();
Console.WriteLine("Db saved.\nShutting down.");
