using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Widget.Weather
{
    public enum WeatherState
    {
        None = -1,
        Clear = 0,
        SmallRain = 1,
        HeavyRain = 2,
        Snow = 3,
        Storm = 4,
        Fog = 5,
        Clouds = 6,
        Wind = 7,
        PartlyCloud = 8,
        PartlySunny = 9
    }
}
