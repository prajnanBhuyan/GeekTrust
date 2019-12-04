using System;

namespace Engine.Models
{
    /// <summary>
    /// A structure to hold all weather realted information
    /// </summary>
    /// <remarks>
    /// I decided to use a struct so that we can easily extend the changes due 
    /// to weather without updating any other portion of the code. For now, we
    /// only have a percentage of ChangeInCraters. We can easily add other weather
    /// affects (ex: Increase in traffic during a rainy day) to this struct and 
    /// easily use them in any behaviour already dependant on weather conditions
    /// </remarks>
    public struct Weather
    {
        /// <summary>
        /// The type of weather
        /// </summary>
        public Weathers WeatherType;

        /// <summary>
        /// Percentage change in the number of craters
        /// </summary>
        public decimal ChangeInCraters;

        /// <summary>
        /// Represents a weather condition and all the affects it has on the orbital routes
        /// </summary>
        /// <param name="weatherType">The type of weather</param>
        /// <param name="changeInCraters">Percentage change in craters</param>
        /// <example>
        /// new Weather(Weathers.Cloudy, 10)
        /// new Weather(Weathers.Snowy, 30)
        /// new Weather(Weathers.Sleeting, 35)
        /// new Weather(Weathers.Stormy, 40)
        /// new Weather(Weathers.Hailing, 50)
        /// new Weather(Weathers.Foggy, 15)
        /// new Weather(Weathers.CleakSky, -20)
        /// new Weather(Weathers.Rainbow, -100)
        /// </example>
        /// <remarks>
        /// The decimal value denotes the percentage change in the number of craters for
        /// the given weather. Negative value means the craters are reduced by the given 
        /// percentage, and a positive value means the number of craters increase by the 
        /// given percentage.
        /// The decimal value should be between -100 to 100
        /// </remarks>
        public Weather(Weathers weatherType, decimal changeInCraters)
        {
            WeatherType = weatherType;
            if (changeInCraters < -100 ||
                changeInCraters > 100)
                throw new ArgumentOutOfRangeException(nameof(changeInCraters), changeInCraters, $"{nameof(changeInCraters)} should be between -100 and 100");
            ChangeInCraters = changeInCraters;
        }
    }
}
