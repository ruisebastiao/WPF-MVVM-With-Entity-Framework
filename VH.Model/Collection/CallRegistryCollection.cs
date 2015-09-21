using System.Collections.Generic;

namespace VH.Model
{
    public class CallRegistryCollection : VHEntityList<CallRegistry>
    {
        public CallRegistryCollection()
        {

        }

        public CallRegistryCollection(List<CallRegistry> list)
            : base(list)
        {

        }
    }
}