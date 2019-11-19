using Engine;
using System;

namespace Problem1
{
    public class Program
    {
        static void Main()
        {
            // Initialising engine
            var engine = new Engine.Engine();

            // Fixed competitor for Problem1
            var competingKingdom = Kingdoms.Space;

            // Declaring contestant for the throne
            engine.AllKingdoms[competingKingdom].IsCompeting = true;
            // Assign the kings name
            engine.AllKingdoms[competingKingdom].King = "King Shan";

            // Variables to store input and output from the user
            string input = string.Empty;

            // Keep reading input from the user unit they enter "exit"
            while (!(input = Console.ReadLine().Trim().ToLower()).Contains("exit"))
            {
                string output;

                int index;
                // In case the user wishes to send a message
                if ((index = input.IndexOf('"')) != -1)
                {
                    var recipient = input.Substring(0, index).Trim(new char[] { ' ', ',' });
                    var messageText = input.Substring(index + 1, input.LastIndexOf('"') - index - 1);

                    if (Enum.TryParse(recipient, true, out Kingdoms recipientKingdom))
                    {
                        output = string.Empty;
                        engine.SendMessage(new Message(competingKingdom, recipientKingdom, messageText));
                        engine.FindRulerByRightToRule(competingKingdom);
                    }
                    else
                    {
                        output = string.Format(Engine.Engine.InvalidKingdomErrorMessage, recipient);
                    }
                }

                // Handle all other input
                else
                    output = engine.ProcessInput(input);

                if (!string.IsNullOrWhiteSpace(output)) Console.WriteLine(output);

            }
        }
    }
}
