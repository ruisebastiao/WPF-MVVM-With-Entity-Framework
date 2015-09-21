using System.Collections.Generic;

namespace VH.Model
{
    public class CustomerHearingAidOrderCollection : VHEntityList<CustomerHearingAidOrder>
    {
        public CustomerHearingAidOrderCollection()
        {

        }

        public CustomerHearingAidOrderCollection(List<CustomerHearingAidOrder> list)
            : base(list)
        {

        }
    }
}