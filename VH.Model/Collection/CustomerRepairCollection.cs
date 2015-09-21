using System.Collections.Generic;

namespace VH.Model
{
    public class CustomerRepairCollection : VHEntityList<CustomerRepair>
    {
        public CustomerRepairCollection()
        {

        }

        public CustomerRepairCollection(List<CustomerRepair> list)
            : base(list)
        {

        }
    }
}