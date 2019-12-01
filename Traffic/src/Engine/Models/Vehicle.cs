using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models
{
    class Vehicle
    {
        readonly decimal speed;
        readonly decimal timePerCrater;
        public readonly List<Weathers> SuitableWeathers;
        public readonly string Name;

        /// <summary>
        /// Represents a type of vehicle
        /// </summary>
        /// <param name="name">Name of the vehicle</param>
        /// <param name="speed">Speed of the vehicle in megamiles per hour (mm/hr)</param>
        /// <param name="timePerCrater">Time required by the vehicle to cross a crater in minutes (min)</param>
        /// <param name="suitableWeathers">Weathers the vehicle can travel in</param>
        /// <exception cref="ArgumentException"></exception>
        public Vehicle(string name, decimal speed, decimal timePerCrater, List<Weathers> suitableWeathers)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"{nameof(name)} cannot be null or whitespace", nameof(name));
            Name = name;

            if (speed <= 0)
                throw new ArgumentException($"{nameof(speed)} should be greater than 0", nameof(speed));
            this.speed = speed;

            if(timePerCrater < 0)
                throw new ArgumentException($"{nameof(timePerCrater)} cannot be negative", nameof(timePerCrater));
            this.timePerCrater = timePerCrater;

            if (suitableWeathers == null ||
                suitableWeathers.Count == 0)
                throw new ArgumentException($"There should be at least one suitable weather", nameof(suitableWeathers));
            SuitableWeathers = suitableWeathers;
        }


        public decimal GetTimeRequired(Orbit orbit)
        {
            var travelTimeInHrs = orbit.Distance / speed;
            var craterTimeInHrs = (orbit.Craters * timePerCrater) / 60;

            return travelTimeInHrs + craterTimeInHrs;
        }
    }
}
