using Engine;
using Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Problem2
{
    public class Program
    {
        static void Main()
        {
            // Create new ConsoleMethods object
            var consoleMethods = new ConsoleMethods();

            // Create BreakerOfChains object
            var breakerOfChains = new BreakerOfChains(consoleMethods);

            // Initialize game engine
            var mGameEngine = new GameEngine(consoleMethods)
            {
                CustomInputValidator = breakerOfChains.BreakerOfChainsValidator,
                CustomInputAction = breakerOfChains.BreakerOfChainsAction
            };

            // Start program execution
            mGameEngine.Execute();
        }
    }

    public class BreakerOfChains
    {
        // Using consoleMethos instead of directly using System.Console
        public IConsoleMethods console;

        public BreakerOfChains(IConsoleMethods consoleMethods)
        {
            this.console = consoleMethods;
        }

        /// <summary>
        /// Custom Input Parser for the 'Breaker of Chains' problem
        /// </summary>
        /// <param name="input">Takes the user input</param>
        /// <returns>Returns a bool value stating whether the user input can be handled</returns>
        public bool BreakerOfChainsValidator(string input)
        {
            return input.Contains("enter") && input.Contains("kingdoms competing");
        }

        /// <summary>
        /// Custom Input Action for the 'Breaker of Chains' problem
        /// </summary>
        /// <param name="input">The user input which triggered the custom action</param>
        /// <param name="southeros"></param>
        /// <returns></returns>
        /// <remarks>We don't particularly require the input data in this function but we keep it anyway to match the function signature</remarks>
        public string BreakerOfChainsAction(string input, ISoutheros southeros)
        {
            // Once a ruler has been found, we no longer need to hold a ballot
            if (southeros.RulingKingdom != null)
                return string.Format(Utility.RulerCrownedMessage, southeros.RulingKingdom.Name);

            // Declare local variables
            var output = string.Empty;
            var noOfCompetitors = 0;
            var maxBallotRounds = 100;
            var messagesToChoose = 6;

            // Read the list of competitors from the user
            var possibleCandidates = console.ReadLine().Trim().Replace(',', ' ').Split(" ", StringSplitOptions.RemoveEmptyEntries);

            // Try parsing the user input into valid kingdoms
            foreach (var candidate in possibleCandidates)
            {
                if (!string.IsNullOrWhiteSpace(candidate) &&
                    Enum.TryParse(candidate, true, out Kingdoms competingKingdom))
                {
                    southeros.AllKingdoms[competingKingdom].IsCompeting = true;
                    noOfCompetitors++;
                }
                else
                    output += string.Format(Utility.InvalidKingdomMessage, candidate);
            }

            // We need atleast two kingdoms competing for the crown
            if (noOfCompetitors < 2)
                output += Utility.NoCompetingKingdomsMessage;
            // and atmost one less than all of them
            else if (noOfCompetitors < Enum.GetValues(typeof(Kingdoms)).Length)
            {
                // FindRulerByBallot return false if we don't have a ruler after maxBallotRounds
                if (!FindRulerByBallot(southeros, maxBallotRounds, messagesToChoose))
                    output += Utility.BallotTookTooLongMessage;
            }
            // If all the kingdoms compete, we can't proceed
            else
                output += Utility.TooManyKingdomsMessage;

            return output;
        }

        /// <summary>
        /// Finds the ruler of Southeros using a ballot system
        /// </summary>
        /// <param name="southeros"></param>
        /// <param name="maxRounds">Maximum number of rounds the ballot can go upto in case of ties</param>
        /// <param name="messagesToChoose">The number of messages the high priest can choose to send out</param>
        /// <returns>Returns a boolean denoting whether a king was crowned or not</returns>
        protected bool FindRulerByBallot(ISoutheros southeros, int maxRounds, int messagesToChoose)
        {
            // Verify arguments passed
            if (messagesToChoose < 1) throw new ArgumentException(Utility.InvalidMessagesToChoose, nameof(messagesToChoose));

            //Ballot round
            int round = 0;

            // The drawing from the ballot goes on until a winner is decided or we reach maxRounds
            while (southeros.RulingKingdom == null &&
                    ++round <= maxRounds)
            {
                // Randomizer to help The High Priest of Southeros choose the messages to send out
                var rnd = new Random();

                // Round up all kingdoms competing for the throne
                var competingKingdoms = southeros.AllKingdoms.Where(kv => kv.Value.IsCompeting).Select(kv => kv.Key).ToList();

                // A list to hold alliance requests from all competing kingdoms
                var allianceRequests = new List<Message>();

                // Each competing kingdom composes messages...
                foreach (var competitor in competingKingdoms)
                {
                    // ...for all the other kingdoms
                    foreach (var reciver in southeros.AllKingdoms.Keys)
                    {
                        // ...except itself ofcourse ¯\_(ツ)_/¯
                        if (reciver == competitor) continue;
                        var newRequest = new Message(competitor, reciver, Utility.listOfMessages[rnd.Next(Utility.listOfMessages.Count)]);
                        allianceRequests.Add(newRequest);
                    }
                }

                // In some cases the number of alliance requests might be less 
                // than the number of messages the high priest intended to select
                messagesToChoose = Math.Min(allianceRequests.Count, messagesToChoose);

                // The High Priest of Southeros chooses the messages to send out
                for (int i = 0; i < messagesToChoose; i++)
                {
                    // Randomly choose message index
                    var messageIndex = rnd.Next(allianceRequests.Count);
                    // Read message
                    var message = allianceRequests.ElementAtOrDefault(messageIndex);
                    // Send message out
                    southeros.AllKingdoms[message.Sender].SendMessage(southeros.AllKingdoms[message.Recipient], message);
                    // Remove message from list so that it is not sent out a second time
                    allianceRequests.RemoveAt(messageIndex);
                }

                // Declare results
                console.WriteLine($"Results after round {Utility.IntegerToWritten(round)} ballot count");

                // Variable to store max allies by any kingdom
                var maxAllies = 0;

                foreach (var competitor in competingKingdoms)
                {
                    var allyCount = southeros.AllKingdoms[competitor].Allies.Count;

                    console.WriteLine($"Allies for {competitor} : {allyCount}");

                    maxAllies = allyCount > maxAllies ? allyCount : maxAllies;
                }

                // Find the kingdom(s) with the most allies
                var leadingKingdoms = southeros.AllKingdoms.Where(kv => kv.Value.IsCompeting && kv.Value.Allies.Count == maxAllies);

#if DEBUG
                // Verbose output to help in Debug mode
                console.WriteLine($"MaxAllies: {maxAllies}");
                console.WriteLine($"Tied Kingdoms: {string.Join(", ", leadingKingdoms.Select(k => $"{k.Key} ({k.Value.Allies.Count})"))}");
                foreach (var kv in leadingKingdoms)
                {
                    console.WriteLine($"Allies of {kv.Key}: {(string.Join(", ", kv.Value.Allies))}");
                }
#endif

                // If there is a tie
                if (leadingKingdoms.Count() > 1)
                {
                    // Clear the list of old competitors
                    competingKingdoms.Clear();

                    // Create a new list comprising of the kingdoms there was a tie between
                    competingKingdoms.AddRange(leadingKingdoms.Select<KeyValuePair<Kingdoms, Kingdom>, Kingdoms>(kv => kv.Key));

                    // Break all alliances
                    foreach (var kingdom in southeros.AllKingdoms.Values) kingdom.BreakAlliances();
                }
                // If not...
                else
                    // ...we declare the winner
                    southeros.RulingKingdom = leadingKingdoms.FirstOrDefault().Value;
            }

            return southeros.RulingKingdom != null;
        }
    }
}
