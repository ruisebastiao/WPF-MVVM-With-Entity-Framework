using System.Collections.Generic;

namespace VH.Model
{
    public class CustomerPhoneCollection : VHEntityList<CustomerPhone>
    {
         public CustomerPhoneCollection()
        {
            
        }

         public CustomerPhoneCollection(List<CustomerPhone> list)
             : base(list)
        {

        }
    }
}