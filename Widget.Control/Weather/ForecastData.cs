using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Widget.Weather
{
    public class ForecastData
    {
        public string Day { get; set; }

        public int HighTemperature { get; set; }

        public int LowTemperature { get; set; }

        public string Text { get; set; }

        public int SkyCode { get; set; }

        public int Precipitation { get; set; }

        public int WindString { get; set; }

        public string Url { get; set; }

        public ForecastData()
        {
            SkyCode = 1;
        }
    }
}
