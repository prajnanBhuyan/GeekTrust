using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public static class Utility
    {
        private static string[] ones = new string[] { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };
        private static string[] teens = new string[] { "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
        private static string[] tens = new string[] { "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
        private static string[] thousandsGroups = { "", " Thousand", " Million", " Billion" };

        #region Error Messages
        public const string InvalidKingdomMessage = "Our messengers couldn't reach '{0}'. Maybe we should try sending messaged just to our neighbouring kingdoms for now.";
        public const string TooManyKingdomsMessage = "Alas! All the kingdoms wanted the thone for themselves and The High Priest couldn't stop the war.";
        public const string BallotTookTooLongMessage = "Alas! The process took too long the the kings grew weary. The battle could not be avoided.";
        public const string NoCompetingKingdomsMessage = "Atleast two kingdoms need to be competing to use the ballott system to decide a ruler.";
        public const string EmblemNotDefinedMessage = "Looks like the game dev forgot to add an emblem for the {0} kingdom";
        #endregion

        static Utility()
        {

        }

        private static string FriendlyInteger(int n, string leftDigits, int thousands)
        {
            if (n == 0)
            {
                return leftDigits;
            }

            string friendlyInt = leftDigits;

            if (friendlyInt.Length > 0)
            {
                friendlyInt += " ";
            }

            if (n < 10)
            {
                friendlyInt += ones[n];
            }
            else if (n < 20)
            {
                friendlyInt += teens[n - 10];
            }
            else if (n < 100)
            {
                friendlyInt += FriendlyInteger(n % 10, tens[n / 10 - 2], 0);
            }
            else if (n < 1000)
            {
                friendlyInt += FriendlyInteger(n % 100, (ones[n / 100] + " Hundred"), 0);
            }
            else
            {
                friendlyInt += FriendlyInteger(n % 1000, FriendlyInteger(n / 1000, "", thousands + 1), 0);
                if (n % 1000 == 0)
                {
                    return friendlyInt;
                }
            }

            return friendlyInt + thousandsGroups[thousands];
        }

        public static string IntegerToWritten(int n)
        {
            if (n == 0)
            {
                return "Zero";
            }
            else if (n < 0)
            {
                return "Negative " + IntegerToWritten(-n);
            }

            return FriendlyInteger(n, "", 0);
        }
    }
}
