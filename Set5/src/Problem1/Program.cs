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

            // Declaring contestant for the throne
            engine.AllKingdoms[Kingdoms.Space].IsCompeting = true;
            // Assign the kings name
            engine.AllKingdoms[Kingdoms.Space].King = "King Shan";

            // Variables to store input and output from the user
            string input = string.Empty;
            string output;

            // Keep reading input from the user unit they enter "exit"
            while (!(input = Console.ReadLine().Trim().ToLower()).Contains("exit"))
            {
                int index;
                // In case the user wishes to send a message
                if ((index = input.IndexOf('"')) != -1)
                {
                    var recipient = input.Substring(0, index).Trim(new char[] { ' ', ',' });
                    var messageText = input.Substring(index + 1, input.LastIndexOf('"') - index - 1);

                    if (Enum.TryParse(recipient, true, out Kingdoms recipientKingdom))
                    {
                        output = string.Empty;
                        engine.SendMessage(new Message(Kingdoms.Space, recipientKingdom, messageText));
                    }
                    else
                    {
                        output = string.Format(Engine.Engine.InvalidKingdomErrorMessage, recipient);
                    }
                }
                else
                    output = engine.ProcessInput(input);

                Console.WriteLine(output);

            }
        }
    }
}
