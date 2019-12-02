using Engine.Interfaces;
using Engine.Models;
using System;
using System.Collections.Generic;

namespace Engine
{
    public enum Weathers
    {
        Sunny,
        Rainy,
        Windy
    }

    public class TrafficEngine
    {
        /// <summary>
        /// Dictionary of possible weathers
        /// </summary>
        public static Dictionary<Weathers, Weather> PossibleWeathers = new Dictionary<Weathers, Weather>()
        {
            { Weathers.Sunny, new Weather(Weathers.Sunny, -10) },
            { Weathers.Rainy, new Weather(Weathers.Rainy, +20) },
            { Weathers.Windy, new Weather(Weathers.Windy, 0) }
        };

        /// <summary>
        /// List of available vehicles
        /// </summary>
        /// <remarks>
        /// Enter the vehicles on the basis of their precedence, i.e. the vehicles with 
        /// higher precedence should come first, and will be choosen in case of a tie.
        /// </remarks>
        public static List<Vehicle> Vehicles = new List<Vehicle>()
        {
            new Vehicle("BIKE", 1, 10, 2, new List<Weathers>()
            {
                Weathers.Sunny,
                Weathers.Windy
            }),

            new Vehicle("TUKTUK", 2, 12, 1, new List<Weathers>()
            {
                Weathers.Sunny,
                Weathers.Rainy
            }),

            new Vehicle("CAR", 3, 22, 3, new List<Weathers>()
            {
                Weathers.Sunny,
                Weathers.Rainy,
                Weathers.Windy
            })
        };

        /// <summary>
        /// List of available orbits
        /// </summary>
        /// <remarks>
        /// These should be entered in the same order as the parameters passed in by the user
        /// </remarks>
        public static List<Orbit> Orbits = new List<Orbit>()
        {
            new Orbit("ORBIT1", 18, 20),
            new Orbit("ORBIT2", 20, 10),
        };

        /// <summary>
        /// Current weather condition in Lengaburu 
        /// </summary>
        public readonly Weather CurrentWeather;

        /// <summary>
        /// Creates a new instance of the TrafficEngine and updates the 
        /// available orbits based on the weather condition and traffic speeds
        /// </summary>
        /// <param name="weather">Current weather condition provided as input</param>
        /// <param name="trafficSpeeds">The speed of the traffic on the orbits provided as input</param>
        public TrafficEngine(string weather, string[] trafficSpeeds)
        {
            if (Enum.TryParse<Weathers>(weather, true, out Weathers tmpWeather))
                CurrentWeather = PossibleWeathers[tmpWeather];
            else
                throw new ArgumentException($"'{weather}' is not a valid {nameof(weather)}", nameof(weather));

            // Update orbit conditions based on values passed in by the user
            for (int i = 0; i < Orbits.Count; i++)
            {
                if (decimal.TryParse(trafficSpeeds[i], out decimal tmpSpeed))
                {
                    if (tmpSpeed < 0)
                        throw new ArgumentOutOfRangeException(nameof(trafficSpeeds), tmpSpeed, $"{nameof(trafficSpeeds)} need to be positive");

                    // Update traffic speed for the orbits
                    Orbits[i].SetTrafficSpeed(tmpSpeed);
                }
                else
                    throw new ArgumentException($"{trafficSpeeds[i]} is not a valid speed for an orbit", nameof(trafficSpeeds));

                // Update road condition to weather
                Orbits[i].UpdateWeatherConditions(CurrentWeather);
            }
        }

        /// <summary>
        /// Compares and return the fastest orbit and vehicle to travle in the given weather and traffic speeds
        /// </summary>
        /// <returns>A string of the format "[Vehicle \name] [Orbit \name]"</returns>
        public string FindFastestRouteAndVehicle()
        {
            Vehicle fastestVehicle = null;
            Orbit fastestOrbit = null;
            decimal currentLowest = decimal.MaxValue;

            foreach (var orbit in Orbits)
            {
                if (orbit.SpeedOfTraffic <= 0)
                    continue;

                foreach (var vehicle in Vehicles)
                {
                    if (!vehicle.SuitableWeathers.Contains(CurrentWeather.WeatherType))
                        continue;

                    var currentSpeed = vehicle.GetTimeRequired(orbit);

                    if (currentSpeed < currentLowest)
                    {
                        fastestVehicle = vehicle;
                        fastestOrbit = orbit;
                        currentLowest = currentSpeed;
                    }

                    // If there is a tie
                    else if (currentSpeed == currentLowest &&
                             vehicle.HasPrecedenceOver(fastestVehicle))
                    {
                        fastestVehicle = vehicle;
                        fastestOrbit = orbit;
                    }
                }
            }

            return $"{fastestVehicle.Name} {fastestOrbit.Name}";
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
