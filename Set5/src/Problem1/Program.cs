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
            engine.AddCompetingKingdom(Kingdoms.Space, "King Shan");

            // Variable to store input from the user
            string input = string.Empty;

            // Keep reading input from the user unit they enter "exit"
            while (input.ToLower() != "exit")
            {
                if ((input = Console.ReadLine().ToLower()) == "exit") break;

                var output = engine.ProcessInput(input);

                if (!string.IsNullOrWhiteSpace(output))
                    Console.WriteLine(output);
            }
        }
    }
}
