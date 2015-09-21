using System.Collections.Generic;

namespace VH.Model
{
    public class CustomerEarMoldOrderCollection : VHEntityList<CustomerEarMoldOrder>
    {
        public CustomerEarMoldOrderCollection()
        {

        }

        public CustomerEarMoldOrderCollection(List<CustomerEarMoldOrder> list)
            : base(list)
        {

        }
    }
}