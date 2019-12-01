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
        Dictionary<Weathers, Weather> PossibleWeathers = new Dictionary<Weathers, Weather>()
        {
            { Weathers.Sunny, new Weather(Weathers.Sunny, -10) },
            { Weathers.Sunny, new Weather(Weathers.Rainy, +20) },
            { Weathers.Sunny, new Weather(Weathers.Windy, 0) }
        };

        /// <summary>
        /// List of available vehicles
        /// </summary>
        List<Vehicle> Vehicles = new List<Vehicle>()
        {
            new Vehicle("Bike", 10, 2, new List<Weathers>()
            {
                Weathers.Sunny,
                Weathers.Windy
            }),

            new Vehicle("Tuktuk", 12, 1, new List<Weathers>()
            {
                Weathers.Sunny,
                Weathers.Rainy
            }),

            new Vehicle("Car", 22, 3, new List<Weathers>()
            {
                Weathers.Sunny,
                Weathers.Rainy,
                Weathers.Windy
            })
        };
    }
}
