using Engine.Interfaces;
using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Factory
{
    public class BreakerOfChains : ICustomGameObject
    {
        // Using consoleMethos instead of directly using System.Console
        // Can easily be update to read from a text file instead
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
        public bool CustomInputValidator(string input)
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
        public string CustomInputAction(string input, ISoutheros southeros)
        {
            // Once a ruler has been found, we no longer need to hold a ballot
            if (southeros.RulingKingdom != null)
                return string.Format(Utility.RulerCrownedMessage, southeros.RulingKingdom.Name);

            // Declare local variables
            var output = string.Empty;
            var noOfCompetitors = 0;
            var maxBallotRounds = 1000;
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
                //
                var highPriest = new HighPriest(messagesToChoose, maxBallotRounds, console);
                // FindRulerByBallot return false if we don't have a ruler after maxBallotRounds

                if (!highPriest.HoldBallot(southeros))
                    output += Utility.BallotTookTooLongMessage;
            }
            // If all the kingdoms compete, we can't proceed
            else
                output += Utility.TooManyKingdomsMessage;

            return output;
        }
    }
}
