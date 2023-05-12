using databaseEditor.Models;
namespace databaseEditor.Logic
{
    /// <summary>
    /// 
    /// </summary>
    public static class DataEmailEmailLogic
    {
        public static void FillEmailThreadIds(List<DataEmailEmail> listOfEmails)
        {
            var totalAmountOfEmails = listOfEmails.Count();
            int currentAmountOfEmailsDone = 0;
            Console.WriteLine("Starting filling in thread id...");
            foreach (var email in listOfEmails)
            {
                if (email.ParentId == null)
                {
                    email.ThreadId = (int)email.Id;
                }
                else
                {
                    if (email.InReplyTo == null)
                    {
                        throw new Exception("Email has parent but no reply to. Invalid dataset.");
                    }
                    var parentEmail = listOfEmails
                        .Where(e => e.MessageId == email.InReplyTo)
                        .FirstOrDefault();
                    if (parentEmail == null)
                    {
                        throw new Exception("Email has parent but can't find parent. Probably an invalid dataset.");
                    }
                    email.ThreadId = (int)parentEmail.Id;
                }
                PrintStatusUpdate(currentAmountOfEmailsDone++, totalAmountOfEmails);
            }
            Console.WriteLine("\nFilling in thread id completed.");
        }

        public static void FillInWordCount(List<DataEmailEmail> listOfEmails)
        {
            var totalAmountOfEmails = listOfEmails.Count();
            int currentAmountOfEmailsDone = 0;
            Console.WriteLine("Starting filling in word count...");
            foreach (var email in listOfEmails)
            {
                int emailWordCount;
                if (email.Body == null)
                {
                    emailWordCount = 0;
                    return;
                }
                char[] delimiters = new char[] { ' ', '\r', '\n' };
                emailWordCount = email.Body.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length;
                PrintStatusUpdate(currentAmountOfEmailsDone++, totalAmountOfEmails);
            }
            Console.WriteLine("\nFilling in word count completed.");
        }

        /// <summary>
        /// Returns the percentage that is currently done, with 2 digit precision.
        /// </summary>
        /// <param name="totalAmountToBeDone"></param>
        /// <param name="currentlyDone"></param>
        /// <returns></returns>
        private static float PrintStatusUpdate(int currentlyDone, int totalAmountToBeDone)
        {
            double percentage = Math.Round((((double)currentlyDone / (double)totalAmountToBeDone) * 100), 2);
            Console.Write($"\rCurrently {percentage}% completed.  ");
            return (float)percentage;
        }
    }
}
