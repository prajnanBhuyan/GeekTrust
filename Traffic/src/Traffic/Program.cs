using Engine;
using System;
using System.IO;
using System.Linq;

namespace traffic
{
    class Program
    {
        static void Main(string[] args)
        {
            // ConsoleMethods object for interaction with the console
            var console = new ConsoleMethods();

            // We expect the filename containing the input to be passed in by the user as an argument
            // We only require the first argument and simply ignore the rest
            if (args.Length > 0)
            {
                // Just read the first parm for input file
                var providedFilePath = args[0];

                // Check if the file exists
                if (File.Exists(providedFilePath))
                {
                    using (var reader = new StreamReader(providedFilePath))
                    {
                        while (!reader.EndOfStream)
                        {
                            var inputs = reader.ReadLine().Split(' ');

                            if (inputs.Length == TrafficEngine.Orbits.Count + 1)
                            {
                                try
                                {
                                    var trafficEngine = new TrafficEngine(inputs[0], inputs.Skip(1).ToArray());
                                    console.WriteLine(trafficEngine.FindFastestRouteAndVehicle());
                                }
                                catch(Exception ex)
                                {
                                    console.WriteLine(ex.Message);
                                }
                            }

                            else
                            {
                                // Write a helpful message for the user
                                console.WriteLine($"Incorrect input. The program expected {TrafficEngine.Orbits.Count + 1} values as input in the following syntax:{Environment.NewLine}");
                                string temp = string.Empty;
                                TrafficEngine.Orbits.ForEach(o => temp += $"<{o.Name} Traffic Speed> ");
                                console.WriteLine($"<Weather> {temp} {Environment.NewLine}");
                                console.WriteLine($"Possible values for weather are:");
                                console.WriteLine(string.Join(", ", Enum.GetNames(typeof(Weathers))));
                            }
                        }
                    }
                }

                // If the user provided an incorrect file path
                else
                {
                    console.WriteLine($"File does not exits: {providedFilePath}");
                }
            }

            // If the user did not provide any arguments
            else
            {
                console.WriteLine($"The program expects the input filepath to be passed as an argument:");
                console.WriteLine($"dotnet geektrust.dll <absolute_path_to_the_input_file>");
            }
        }
    }
}
