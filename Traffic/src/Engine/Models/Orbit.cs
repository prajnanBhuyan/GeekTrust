using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models
{
    class Orbit
    {
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
        public Orbit(decimal distance, int craters)
        {
            if (distance <= 0)
                throw new ArgumentException($"{nameof(distance)} cannot be zero or negative", nameof(distance));
            Distance = distance;

            if (distance <= 0)
                throw new ArgumentException($"{nameof(craters)} cannot be negative", nameof(craters));
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
            Craters *= (int)Math.Floor(1M + (weather.ChangeInCraters / 100));
        }

        public void SetTrafficSpeed(decimal speedOfTraffic)
        {
            if (speedOfTraffic < 0)
                throw new ArgumentException($"{nameof(speedOfTraffic)} cannot be negative", nameof(speedOfTraffic));
            SpeedOfTraffic = speedOfTraffic;
        }
    }
}
