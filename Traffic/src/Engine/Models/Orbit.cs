using System;

namespace Engine.Models
{
    public class Orbit
    {
        /// <summary>
        /// Name of the orbit
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// The distance between Silk Dorb and Lengaburu in megamiles (mm)
        /// </summary>
        public decimal Distance { get; private set; }

        /// <summary>
        /// The number of craters in the route
        /// </summary>
        public int Craters { get; private set; }

        /// <summary>
        /// The speed of traffic in the route in megamiles per hour (mm/hr)
        /// </summary>
        public decimal SpeedOfTraffic { get; private set; }

        /// <summary>
        /// Represents a route between Silk Dorb and Lengaburu
        /// </summary>
        /// <param name="distance">The distance between Silk Dorb and Lengaburu in megamiles (mm)</param>
        /// <param name="craters">The number of craters in the route</param>
        /// <exception cref="ArgumentException"></exception>
        public Orbit(string name, decimal distance, int craters)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"{nameof(name)} cannot be null or whitespace", nameof(name));
            Name = name;

            if (distance <= 0)
                throw new ArgumentOutOfRangeException(nameof(distance), distance, $"{nameof(distance)} cannot be zero or negative");
            Distance = distance;

            if (craters < 0)
                throw new ArgumentOutOfRangeException(nameof(craters), craters, $"{nameof(craters)} cannot be negative");
            Craters = craters;
        }

        /// <summary>
        /// Updates the number of craters based on the weather conditions
        /// </summary>
        /// <param name="weather">The current weather</param>
        /// <exception cref="NullReferenceException">Can throw a NullReferenceException if weather is null</exception>
        public void UpdateWeatherConditions(Weather weather)
        {
            // We use floor because 10% increase in 15 craters would be 16.5 craters
            // but that is not possible in the real world. So we select 16 instead.
            Craters = (int)Math.Floor(Craters * (1M + (weather.ChangeInCraters / 100)));
        }

        /// <summary>
        /// Sets the speed of the traffic on the orbital route
        /// </summary>
        /// <param name="speedOfTraffic">The speed passed in by the user as an argument</param>
        /// <exception cref="ArgumentOutOfRangeException">If speedOfTraffic is negative</exception>
        public void SetTrafficSpeed(decimal speedOfTraffic)
        {
            if (speedOfTraffic < 0)
                throw new ArgumentOutOfRangeException(nameof(speedOfTraffic), speedOfTraffic, $"{nameof(speedOfTraffic)} cannot be negative");
            SpeedOfTraffic = speedOfTraffic;
        }
    }
}
