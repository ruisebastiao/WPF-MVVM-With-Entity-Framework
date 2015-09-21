using System.Collections.Generic;

namespace VH.Model
{
    public class CustomerWarrantyInformedCollection : VHEntityList<CustomerWarrantyInformed>
    {
        public CustomerWarrantyInformedCollection()
        {

        }

        public CustomerWarrantyInformedCollection(List<CustomerWarrantyInformed> list)
            : base(list)
        {

        }
    }
}