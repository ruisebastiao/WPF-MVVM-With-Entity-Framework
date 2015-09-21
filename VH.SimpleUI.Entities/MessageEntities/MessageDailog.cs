using System;

namespace VH.SimpleUI.Entities
{
    public class MessageDailog 
    {
        public Action<DialogResult> Callback { get; private set; }

        #region Properties
        
        public string Caption { get; set; }

        public DialogResult DefaultResult { get; set; }

        public DialogButton DialogButton { get; set; }

        public double DialogHeight { get; set; }

        public double DialogWidth { get; set; }

        public String Title { get; set; }

        public bool IsSizeToContent { get; set; }
        #endregion

        #region Constructors
        public MessageDailog(Action<DialogResult> callback)
        {
            this.Callback = callback;
        }
        public MessageDailog()
        {
        }
        #endregion
       

        public void ProcessCallback(DialogResult result)
        {
            if (this.Callback == null)
                return;
            this.Callback(result);
        }
    }
}