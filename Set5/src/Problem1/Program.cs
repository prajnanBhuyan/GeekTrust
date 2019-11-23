using Engine;
using Engine.Interfaces;
using System;

namespace Problem1
{
    public class Program
    {
        static void Main()
        {
            // Initialize game engine
            var mGameEngine = new GameEngine()
            {
                CustomInputParser = AGoldenCrownParser,
                CustomInputAction = AGoldenCrownAction
            };

            // Give the gorilla king of the Space kingdom a name
            mGameEngine.AllKingdoms[Kingdoms.Space].King = "King Shan";

            // Start program execution
            mGameEngine.Execute();
        }

        // Custom Input Parser for the 'A Golden Crown' problem
        private static bool AGoldenCrownParser(string input)
        {
            var index = input.IndexOf('"');
            var lastIndex = input.LastIndexOf('"');
            // We expect an input that contains:
            //  1. The name of the kingdom it is addressed to
            //  2. The message enclosed in inverted commas
            //  Example:
            //      a. air, "send owl"      Kingdom: air    Message: send owl
            //      b. air"send"~"owl"      Kingdom: air    Message: send"~"owl
            return index != -1 &&
                    lastIndex > index + 1;

        }

        // Custom Input Action for the 'A Golden Crown' problem
        private static string AGoldenCrownAction(string input, ISoutheros southeros)
        {
            // Declare local variables
            var output = string.Empty;
            var competingKingdom = Kingdoms.Space;
            var reqRightToRule = 3;

            // Try parsing the input into target kingdom and message
            var index = input.IndexOf('"');
            var recipient = input.Substring(0, index).Trim(new char[] { ' ', ',' });
            var messageText = input.Substring(index + 1, input.LastIndexOf('"') - index - 1);

            // Try parsing the target kingdom to verify target is valid
            if (Enum.TryParse(recipient, true, out Kingdoms recipientKingdom))
            {
                // Create message object
                var message = new Message(competingKingdom, recipientKingdom, messageText);

                // Send out message
                southeros.AllKingdoms[competingKingdom].SendMessage(southeros.AllKingdoms[recipientKingdom], message);

                // Check if the competeing kingdom now has the required number of allies
                if (southeros.AllKingdoms[competingKingdom].Allies.Count >= reqRightToRule)
                    // If so, set the value for the ruling kingdom
                    southeros.RulingKingdom = southeros.AllKingdoms[competingKingdom];
            }
            // If target is invalid, return appropriate error message
            else
            {
                output = string.Format(Utility.InvalidKingdomMessage, recipient);
            }

            return output;
        }
    }
}
