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
                CustomInputParser = GoldenCrownParser,
                CustomInputAction = GoldenCrownAction
            };

            // Give the gorilla king of the Space kingdom a name
            mGameEngine.AllKingdoms[Kingdoms.Space].King = "King Shan";

            // Start program execution
            mGameEngine.Execute();
        }

        // Custom Input Parser for the 'A Golden Crown' problem
        private static bool GoldenCrownParser(string input)
        {
            return input.IndexOf('"') != -1;
        }

        // Custom Input Action for the 'A Golden Crown' problem
        private static string GoldenCrownAction(string input, ISoutheros southeros)
        {
            // Declare local variables
            var output = string.Empty;
            var competingKingdom = Kingdoms.Space;
            var reqRightToRule = 3;

            // Try parsing the input into target kingdom and message
            var index = input.IndexOf('"');
            var recipient = input.Substring(0, index).Trim(new char[] { ' ', ',' });
            // TODO: Handle and add test cases for scenario where user enter corrupt message like:
            //          Air, "User dozes of while wri...
            //          Ice, User forgot to enter starting inverted comma"
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
