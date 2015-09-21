using System.Collections.Generic;

namespace VH.Model
{
    public class LocationCollection : MTObservableCollection<Location>
    {
        public LocationCollection()
        {

        }

        public LocationCollection(List<Location> list)
            : base(list)
        {

        }
    }
}