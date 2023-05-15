using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace databaseEditor
{
    public static class UIFunctions
    {
        /// <summary>
        /// Returns the percentage that is currently done, with 2 digit precision.
        /// </summary>
        /// <param name="totalAmountToBeDone"></param>
        /// <param name="currentlyDone"></param>
        /// <returns></returns>
        public static float PrintStatusUpdate(int currentlyDone, int totalAmountToBeDone)
        {
            double percentage = Math.Round((((double)currentlyDone / (double)totalAmountToBeDone) * 100), 2);
            Console.Write($"\rCurrently {percentage}% completed.  ");
            return (float)percentage;
        }

        public static bool CheckIfUserWantsToTakeAction(string requestedAction)
        {
            Console.WriteLine($"Do you want to {requestedAction}?\nYes / No");
            var userInput = Console.ReadLine();
            if (userInput != null && userInput == "Yes")
            {
                return true;
            }
            return false;
        }
    }
}
