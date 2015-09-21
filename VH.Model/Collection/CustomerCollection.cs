using System.Collections.Generic;

namespace VH.Model
{
    public class CustomerCollection : VHEntityList<Customer>
    {
        public CustomerCollection()
        {
            
        }

        public CustomerCollection(List<Customer> list) : base(list)
        {

        }

    }
}