using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models
{
    public class Vehicle
    {
        /// <summary>
        /// Speed of the vehicle in megamiles per hour (mm/hr)
        /// </summary>
        readonly decimal speed;

        /// <summary>
        /// Time required by the vehicle to cross a crater in minutes (min)
        /// </summary>
        readonly decimal timePerCrater;

        /// <summary>
        /// Weathers the vehicle can travel in
        /// </summary>
        public readonly List<Weathers> SuitableWeathers;

        /// <summary>
        /// Name of the vehicle
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Precedence of the vehicle, lower the number, higher the priority
        /// </summary>
        public readonly int Precedence;

        /// <summary>
        /// Represents a type of vehicle
        /// </summary>
        /// <param name="name">Name of the vehicle</param>
        /// <param name="precedence">Precedence of the vehicle, lower the number, higher the priority</param>
        /// <param name="speed">Speed of the vehicle in megamiles per hour (mm/hr)</param>
        /// <param name="timePerCrater">Time required by the vehicle to cross a crater in minutes (min)</param>
        /// <param name="suitableWeathers">Weathers the vehicle can travel in</param>
        /// <exception cref="ArgumentException"></exception>
        public Vehicle(string name, int precedence, decimal speed, decimal timePerCrater, List<Weathers> suitableWeathers)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"{nameof(name)} cannot be null or whitespace", nameof(name));
            Name = name;

            if (speed <= 0)
                throw new ArgumentOutOfRangeException(nameof(speed), speed, $"{nameof(speed)} should be greater than 0");
            this.speed = speed;

            if (timePerCrater < 0)
                throw new ArgumentOutOfRangeException(nameof(timePerCrater), timePerCrater, $"{nameof(timePerCrater)} must be positive");
            this.timePerCrater = timePerCrater;

            if (suitableWeathers == null ||
                suitableWeathers.Count == 0)
                throw new ArgumentException($"There should be at least one suitable weather", nameof(suitableWeathers));
            SuitableWeathers = suitableWeathers;

            Precedence = precedence;
        }

        /// <summary>
        /// Gets the time required by the vehicle to travel the entire route
        /// </summary>
        /// <param name="orbit">The orbit the vehicle will be travelling in</param>
        /// <returns>The time required in hours (hr)</returns>
        public decimal GetTimeRequired(Orbit orbit)
        {
            // The vehicle cannot travel faster than the speed of the traffic
            var actualSpeed = Math.Min(speed, orbit.SpeedOfTraffic);

            var travelTimeInHrs = orbit.Distance / actualSpeed;

            // Dividing by 60 to convert into hours
            var craterTimeInHrs = (orbit.Craters * timePerCrater) / 60;

            return travelTimeInHrs + craterTimeInHrs;
        }

        /// <summary>
        /// Returns a boolean representing if the current vehicle has a higher priority
        /// </summary>
        /// <param name="competingVehicle">The vehicle to compare current vehicle's priority against</param>
        /// <returns></returns>
        public bool HasPrecedenceOver(Vehicle competingVehicle)
        {
            return Precedence < competingVehicle.Precedence;
        }
    }
}
