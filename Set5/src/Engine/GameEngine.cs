﻿using Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

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

        public GameEngine()
        {
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
                    var message = $"Looks like the dev forgot to add the emblem for {kingdom}{Environment.NewLine}";
                    message += $"Message:{Environment.NewLine}{keyNotFoundException.Message}{Environment.NewLine}";
                    message += $"Stacktrace:{Environment.NewLine}{keyNotFoundException.StackTrace}{Environment.NewLine}";
                    Console.WriteLine(message);
                }
            }
        }

        /// <summary>
        /// This will run the actual program.
        /// </summary>
        /// <remarks>It will keep reading input from the user until they type exit</remarks>
        public void Execute()
        {
            // Variables to store input and output from the user
            string input, output;

            try
            {
                // Keep reading input from the user unit they enter "exit"
                while (!(input = Console.ReadLine().Trim().ToLower()).Contains("exit"))
                {
                    output = ProcessInput(input);

                    if (!string.IsNullOrWhiteSpace(output)) Console.WriteLine(output);
                }
            }
            catch(Exception ex)
            {
                var message = $"Well, this is embarrassing. You weren't supposed to see this.";
                message += $"Message:{Environment.NewLine}{ex.Message}{Environment.NewLine}";
                message += $"Stacktrace:{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}";
                Console.WriteLine(message);
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
            if (input.Contains("who is the ruler of southeros"))
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
}
