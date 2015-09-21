using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Widget.Weather
{
    public static class WeatherConverter
    {
        public static WeatherState ConvertSkyCodeToWeatherState(int skycode)
        {
            switch (skycode)
            {
                case 2:
                case 4:
                    return WeatherState.PartlySunny;
                case 3:
                case 6:
                case 34:
                case 35:
                case 36:
                case 38:
                    return WeatherState.PartlyCloud;
                case 8:
                case 7:
                    return WeatherState.Clouds;
                case 11:
                case 37:
                    return WeatherState.Fog;
                case 12:
                case 13:
                case 14:
                    return WeatherState.SmallRain;
                case 19:
                case 20:
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                    return WeatherState.Snow;
                case 32:
                    return WeatherState.Wind;
                case 18:
                    return WeatherState.HeavyRain;
                case 15:
                case 16:
                case 17:
                    return WeatherState.Storm;
                default:
                    return WeatherState.Clear;
            }

        }
    }
}
