using Engine.Interfaces;
using System;
using System.Collections.Generic;

namespace Engine
{
    public enum Kingdoms
    {
        Space = 0,
        Land = 1,
        Water = 2,
        Ice = 3,
        Air = 4,
        Fire = 5
    };

    public class GameEngine : IGameEngine, ISoutheros
    {
        public IConsoleMethods console;
        public static readonly Dictionary<Kingdoms, string> Emblems = new Dictionary<Kingdoms, string>()
        {
            {Kingdoms.Space, "gorilla" },
            {Kingdoms.Land, "panda" },
            {Kingdoms.Water, "octopus" },
            {Kingdoms.Ice, "mammoth" },
            {Kingdoms.Air, "owl" },
            {Kingdoms.Fire, "dragon" }
        };
        public Dictionary<Kingdoms, Kingdom> AllKingdoms { get; }
        public Kingdom RulingKingdom { get; set; }

        public Func<string, bool> CustomInputValidator { get; set; }
        public Func<string, ISoutheros, string> CustomInputAction { get; set; }

        public GameEngine(IConsoleMethods consoleMethods)
        {
            // Using consoleMethos instead of directly using System.Console
            this.console = consoleMethods;

            // Initialise dictionary with all available kingdoms
            AllKingdoms = new Dictionary<Kingdoms, Kingdom>();
            foreach (Kingdoms kingdom in Enum.GetValues(typeof(Kingdoms)))
            {
                try
                {
                    AllKingdoms.Add(kingdom, new Kingdom(kingdom, Emblems[kingdom]));
                }
                catch (KeyNotFoundException keyNotFoundException)
                {
                    throw new KeyNotFoundException($"Emblem not defined for {kingdom}", keyNotFoundException);
                }
            }

            // Initialise CustomInputValidator
            CustomInputValidator = (string anything) => { return false; };
        }

        /// <summary>
        /// This will run the actual program.
        /// </summary>
        /// <remarks>It will keep reading input from the user until they type exit</remarks>
        public void Execute()
        {
            // Variable to store the input
            string input;

            // Keep reading input from the user unit they enter "exit"
            while (!(input = console.ReadLine().Trim().ToLower()).Contains("exit"))
            {
                // Variable to store the output from the user
                var output = string.Empty;
                if (!string.IsNullOrWhiteSpace(input))
                    output = ProcessInput(input);

                if (!string.IsNullOrWhiteSpace(output))
                    console.WriteLine(output);
            }
        }

        /// <summary>
        /// Function to process all input.
        /// </summary>
        /// <param name="input">The input from the user</param>
        /// <returns>The most suitable output</returns>
        public string ProcessInput(string input)
        {
            string output = "Invalid input";

            // If the user asks who the ruler is
            if (input.Contains("who is the ruler") || input.Contains("ruler of southeros"))
            {
                if (RulingKingdom != null)
                    output = string.IsNullOrWhiteSpace(RulingKingdom.King) ? RulingKingdom.Name.ToString() : RulingKingdom.King;
                else
                    output = "None";
            }

            // Or if they ask how are the allies of the ruler
            else if (input.Contains("allies of"))
            {
                if (RulingKingdom != null)
                    output = string.Join(", ", RulingKingdom.Allies);
                else
                    output = "None";
            }

            // Or if the input is successfully parsed by the custom parser
            else if (CustomInputValidator(input))
            {
                output = CustomInputAction(input, this);
            }

            return output;
        }
    }

    public class ConsoleMethods : IConsoleMethods
    {
        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}
