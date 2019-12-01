using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models
{
    struct Weather
    {
        public Weathers WeatherType;
        public decimal ChangeInCraters;

        public Weather(Weathers weatherType, decimal changeInCraters)
        {
            WeatherType = weatherType;
            ChangeInCraters = changeInCraters;
        }
    }
}
