using System;
using System.Linq;

namespace VH.Model
{
    public class Location
    {
        public int CityId { get; set; }

        public string CityName { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string State { get; set; }

        public Location GetLocation(string line)
        {
            line = line.Replace("(", "");
            line = line.Replace(")", "");
            line = line.Replace("'", "");
            line = line.Trim();
            var strArrya = line.Split(',').
                                Select(tag => tag.Trim()).
                                Where(tag => !string.IsNullOrEmpty(tag)).ToList();
            if (strArrya.Count < 5)
                return null;
            var properties = this.GetType().GetProperties();
            int i = 0;
            foreach (var propertyInfo in properties)
            {
                propertyInfo.SetValue(this, Convert.ChangeType(strArrya[i], propertyInfo.PropertyType), null);
                i++;
            }
            return this;
        }
    }
}