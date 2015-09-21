using System.Collections.Generic;

namespace VH.Model
{
    public class CustomerAddressCollection : VHEntityList<CustomerAddress>
    {
        public CustomerAddressCollection()
        {

        }

        public CustomerAddressCollection(List<CustomerAddress> list)
            : base(list)
        {

        }
    }
}