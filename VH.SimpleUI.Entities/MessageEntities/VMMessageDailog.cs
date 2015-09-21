using System;
using VH.Bases;

namespace VH.SimpleUI.Entities
{
    public class VMMessageDailog
    {
        public Action<dynamic> Callback { get; private set; }

        public IBaseViewModel ChildViewModel { get; set; }

        public void ProcessCallback(dynamic result)
        {
            if (this.Callback == null)
                return;
            this.Callback(result);
        } 
    }
}