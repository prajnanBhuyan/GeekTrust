using Engine;
using System;

namespace Problem2
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialising engine
            var engine = new Engine.Engine();

            // Variable to store input from the user
            string input = string.Empty;

            // Keep reading input from the user unit they enter "exit"
            while (!(input = Console.ReadLine().Trim().ToLower()).Contains("exit"))
            {
                string output = string.Empty;

                // If user wishes to enter the competing kingdoms
                if (input.Contains("enter") && input.Contains("kingdoms competing"))
                {
                    var noOfCompetitors = 0;
                    var possibleCandidates = Console.ReadLine().Trim().Split(" ");

                    foreach (var candidate in possibleCandidates)
                    {
                        if (!string.IsNullOrWhiteSpace(candidate) &&
                            Enum.TryParse(candidate, true, out Kingdoms competingKingdom))
                        {
                            engine.AllKingdoms[competingKingdom].IsCompeting = true;
                            noOfCompetitors++;
                        }
                    }

                    if (noOfCompetitors < 2)
                        output = Engine.Engine.NoCompetingKingdoms;
                    else if (noOfCompetitors < Enum.GetValues(typeof(Kingdoms)).Length)
                        engine.FindRulerByBallot();
                    else
                        output = Engine.Engine.TooManyCompetingKingdoms;
                }

                // Handle all other input
                else
                    output = engine.ProcessInput(input);

                Console.WriteLine(output);
            }
        }
    }
}
