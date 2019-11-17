using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Engine
{

    // Consider to be the High Priest
    public class Engine
    {
        private Kingdom rulingKingdom;
        private readonly List<string> listOfMessages;

        public readonly Dictionary<Kingdoms, Kingdom> AllKingdoms;
        public const int MessagesDuringBallot = 6;
        public const int MaxBallotRounds = 100;
        public static readonly string[] Emblems =
        {
            "gorilla",
            "panda",
            "octopus",
            "mammoth",
            "owl",
            "dragon"
        };
        public const string InvalidKingdomErrorMessage = "Our messengers couldn't reach '{0}'. Maybe we should try sending messaged just to our neighbouring kingdoms for now.";
        public const string TooManyCompetingKingdoms = "Alas! All the kingdoms wanted the thone for themselves and The High Priest couldn't stop the war.";
        public const string BallotTookTooLong = "Alas! The process took too long the the kings grew weary. The battle could not be avoided.";

        public Engine()
        {
            // Initialize dictionary of all kingdoms
            AllKingdoms = new Dictionary<Kingdoms, Kingdom>();
            foreach (var kingdomObj in Enum.GetValues(typeof(Kingdoms)))
            {
                var kingdom = Enum.Parse<Kingdoms>(kingdomObj.ToString());
                if (kingdom == Kingdoms.None) continue;
                AllKingdoms.Add(kingdom, new Kingdom(kingdom));
            }

            // Read the list of messages
            listOfMessages = new List<string>();
            var resourceName = "Engine.Engine.Resources.messages.txt";
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listOfMessages.Add(line);
                }
            }
        }

        public string ProcessInput(string input)
        {
            string output = string.Empty;

            // If the user asks who the ruler is
            if (input == "who is the ruler of southeros?")
            {
                if (rulingKingdom != null)
                    output = string.IsNullOrWhiteSpace(rulingKingdom.King) ? rulingKingdom.Name : rulingKingdom.King;
                else
                    output = "None";
            }

            // Or if they ask how are the allies of the ruler
            else if (input.Contains("allies of"))
            {
                if (rulingKingdom != null)
                    output = string.Join(", ", rulingKingdom.Allies);
                else
                    output = "None";
            }

            return output;
        }


        /// <summary>
        /// Sends a message from one kingdom to another.
        /// </summary>
        /// <param name="message">A messsage object contains infromation about sender, reciever and message itself.</param>
        /// <remarks>If prestated conditions are met, an alliance is created between the two kingdoms.</remarks>
        public void SendMessage(Message message)
        {
            // Only send the message if:
            //  1. If the message contains the emblem animal of the recieving kingdom...
            //  2. The recieving kingdom itself is not competing
            //  3. It is not already allied with another kingdom
            if (SecretMessageAccepted(message.Recipient, message.SecretMessage) &&
                !AllKingdoms[message.Recipient].IsCompeting &&
                AllKingdoms[message.Recipient].Allies.Count == 0)
            {
                // Create an alliance
                AllKingdoms[message.Sender].Allies.Add(message.Recipient);
                AllKingdoms[message.Recipient].Allies.Add(message.Sender);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void FindRulerByBallot()
        {
            // Ballot round
            int round = 1;

            // The drawing from the ballot goes on until a winner is decided or we reach MaxBallotRounds
            while (rulingKingdom == null &&
                    round <= MaxBallotRounds)
            {
                // Randomizer to help The High Priest of Southeros choose the messages to send out
                var rnd = new Random();

                // Round up all kingdoms competing for the throne
                var competingKingdoms = AllKingdoms.Where(kv => kv.Value.IsCompeting).Select(kv => kv.Key).ToList();

                // A list to hold alliance requests from all competing kingdoms
                var allianceRequests = new List<Message>();

                // Each competing kingdom composes messages...
                foreach (var competitor in competingKingdoms)
                {
                    // ...for all the other kingdoms
                    foreach (var reciver in AllKingdoms.Keys)
                    {
                        // ...except itself ofcourse ¯\_(ツ)_/¯
                        if (reciver == competitor) continue;
                        var newRequest = new Message(competitor, reciver, listOfMessages[rnd.Next(listOfMessages.Count)]);
                        allianceRequests.Add(newRequest);
                    }
                }

                // The High Priest of Southeros chooses the messages to send out
                for (int i = 0; i < MessagesDuringBallot; i++)
                {
                    SendMessage(allianceRequests[rnd.Next(allianceRequests.Count)]);
                }

                // Declare results
                Console.WriteLine($"Results after round {HumanFriendlyInteger.IntegerToWritten(round)} ballot count");

                // Variable to store max allies by any kingdom
                var maxAllies = 0;

                foreach (var competitor in competingKingdoms)
                {
                    var allyCount = AllKingdoms[competitor].Allies.Count;

                    Console.WriteLine($"Allies for {competitor.ToString()} : {allyCount}");

                    maxAllies = allyCount > maxAllies ? allyCount : maxAllies;
                }

                // Find the kingdom(s) with the most allies
                var leadingKingdoms = AllKingdoms.Where(kv => kv.Value.Allies.Count == maxAllies);

                // If there is a tie
                if (leadingKingdoms.Count() > 1)
                {
                    // Clear the list of old competitors
                    competingKingdoms.Clear();

                    // Create a new list comprising of the kingdoms there was a tie between
                    competingKingdoms.AddRange(leadingKingdoms.Select<KeyValuePair<Kingdoms, Kingdom>, Kingdoms>(kv => kv.Key));
                }
                // If not...
                else
                    // ...we declare the winner
                    rulingKingdom = leadingKingdoms.FirstOrDefault().Value;
            }

            if (rulingKingdom == null)
                Console.WriteLine(BallotTookTooLong);
        }

        /// <summary>
        /// Checks if the recieving kingdoms animal has been sneakily added into the message.
        /// </summary>
        /// <param name="kingdom">Kingdom sending the message.</param>
        /// <param name="message">The message being sent to create an alliance.</param>
        /// <returns></returns>
        public static bool SecretMessageAccepted(Kingdoms kingdom, string message)
        {

            var emblem = Emblems[(int)kingdom];

            var isValid = message.Length >= emblem.Length;

            for (int i = 0; isValid && i < emblem.Length; i++)
            {
                int index;

                if ((index = message.IndexOf(emblem[i])) != -1)
                    message = message.Remove(index, 1);
                else
                    isValid = false;
            }

            return isValid;
        }
    }

    public static class HumanFriendlyInteger
    {
        static string[] ones = new string[] { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };
        static string[] teens = new string[] { "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
        static string[] tens = new string[] { "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
        static string[] thousandsGroups = { "", " Thousand", " Million", " Billion" };

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
