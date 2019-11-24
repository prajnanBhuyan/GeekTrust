using Engine;
using Engine.Factory;
using Engine.Interfaces;
using System;

namespace TameOfThrones
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Create new ConsoleMethods object
            var consoleMethods = new ConsoleMethods();

            // Checking if input arguments were supplied.
            if (args.Length > 0)
            {
                // Create SimpleProblemFactory object
                var simpleProblemFactory = new SimpleProblemFactory();
                var problem = args[0];

                // Create variable to hold customGameObject
                ICustomGameObject gameObject;

                try
                {
                    // Fetch problem to start execution
                    gameObject = simpleProblemFactory.GetProblem(problem, consoleMethods);

                    // Initialize game engine
                    var mGameEngine = new GameEngine(consoleMethods)
                    {
                        CustomInputValidator = gameObject.CustomInputValidator,
                        CustomInputAction = gameObject.CustomInputAction
                    };

                    // Start program execution
                    mGameEngine.Execute();
                }
                catch (ArgumentException aex)
                {
                    consoleMethods.WriteLine(aex.Message);
                }
            }
            else
            {
                consoleMethods.WriteLine("Please specify which problem you wish to execute.");
                consoleMethods.WriteLine("Usage: TameOfThrones.dll <Problem1|Problem2>");
            }
        }
    }
}
