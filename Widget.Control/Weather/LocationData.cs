namespace Widget.Weather
{
    public class LocationData
    {
        /// <summary>
        /// location code
        /// </summary>
        public string Code;

        /// <summary>
        /// Location name
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Country name
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// Latitude
        /// </summary>
        public double Lat;
        /// <summary>
        /// Longtitude
        /// </summary>
        public double Lon;
    }
}
