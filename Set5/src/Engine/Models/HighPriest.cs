using Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Models
{
    public class HighPriest
    {
        int messagesToChoose;
        readonly int maxRounds;
        // Using consoleMethos instead of directly using System.Console
        // Can easily be update to read from a text file instead
        public IConsoleMethods console;

        /// <summary>
        /// The high priest of southeros
        /// </summary>
        /// <param name="messagesToChoose">The number of messages to be picked randomly from the ballot.</param>
        /// <param name="maxRounds">The number of rounds the ballot process can go on for.</param>
        /// <param name="consoleMethods">Console methods to be used for user interaction</param>
        /// <remarks>Special Powers: Hold Ballot to avoid war</remarks>
        public HighPriest(int messagesToChoose, int maxRounds, IConsoleMethods consoleMethods)
        {
            // messagesToChoose has to be greater than or equal to 1
            if (messagesToChoose < 1)
                throw new ArgumentException($"{nameof(messagesToChoose)} should be greater than 0", nameof(messagesToChoose));
            // maxRounds has to be greater than or equal to 1
            if (maxRounds < 1)
                throw new ArgumentException($"{nameof(maxRounds)} should be greater than 0", nameof(maxRounds));

            this.messagesToChoose = messagesToChoose;
            this.maxRounds = maxRounds;
            console = consoleMethods;
        }

        /// <summary>
        /// Picks a specified number of message from the ballot box to be handed out to the kingdoms
        /// </summary>
        /// <param name="ballotBox"></param>
        /// <returns></returns>
        private List<Message> PickRandomMessages(BallotBox ballotBox)
        {
            // In some cases the number of alliance requests might be less 
            // than the number of messages the high priest intended to select
            messagesToChoose = Math.Min(ballotBox.Count, messagesToChoose);

            var choosenMessages = new List<Message>();

            // The High Priest of Southeros chooses the messages to send out
            for (int i = 0; i < messagesToChoose; i++)
                choosenMessages.Add(ballotBox.PickMessage());

            return choosenMessages;
        }

        /// <summary>
        /// Finds the ruler of Southeros using a ballot system
        /// </summary>
        /// <param name="southeros">Reference to a ISoutheros implemented class object</param>
        /// <returns>Returns a boolean denoting whether a king was crowned or not</returns>
        public bool HoldBallot(ISoutheros southeros)
        {
            // Randomizer to choose message to write
            var rnd = new Random();

            //Ballot round
            int round = 0;

            // The drawing from the ballot goes on until a winner is decided or we reach maxRounds
            while (southeros.RulingKingdom == null &&
                    ++round <= maxRounds)
            {

                // Get a new ballot box
                var ballotBox = new BallotBox();

                // Round up all kingdoms competing for the throne
                var competingKingdoms = southeros.AllKingdoms.Where(kv => kv.Value.IsCompeting).Select(kv => kv.Key).ToList();

                // Each competing kingdom composes messages...
                foreach (var competitor in competingKingdoms)
                {
                    // ...for all the other kingdoms
                    foreach (var reciver in southeros.AllKingdoms.Keys)
                    {
                        // ...except itself ofcourse ¯\_(ツ)_/¯
                        if (reciver == competitor) continue;
                        var newRequest = new Message(competitor, reciver, Utility.listOfMessages[rnd.Next(Utility.listOfMessages.Count)]);
                        ballotBox.DropMessage(newRequest);
                    }
                }

                // The High Priest of Southeros chooses the messages to send out
                var choosenMessages = PickRandomMessages(ballotBox);

                // Hand over the messages
                foreach (var message in choosenMessages)
                {
                    southeros.AllKingdoms[message.Sender].SendMessage(southeros.AllKingdoms[message.Recipient], message);
                }

                // Declare results
                console.WriteLine($"{Environment.NewLine}Results after round {Utility.IntegerToWritten(round)} ballot count");

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

                // If there is a tie
                if (leadingKingdoms.Count() > 1)
                {
                    // Clear the list of old competitors
                    competingKingdoms.Clear();

                    // Create a new list comprising of the kingdoms there was a tie between
                    competingKingdoms.AddRange(leadingKingdoms.Select(kv => kv.Key));

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
