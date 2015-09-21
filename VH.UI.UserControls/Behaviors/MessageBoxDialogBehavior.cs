using System.Linq;
using System.Windows.Interactivity;
using System.Windows;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Messaging;
using VH.SimpleUI.Entities;

namespace VH.UI.UserControls
{
    public class MessageBoxDialogBehavior : Behavior<FrameworkElement>
    {
        readonly IMessenger _messenger = Messenger.Default;

        protected override void OnAttached()
        {
            base.OnAttached();

            _messenger.Register<MessageDailog>(this, Identifier, ShowDialog);
        }

        public string Identifier { get; set; }

        private void ShowDialog(MessageDailog dm)
        {
            Dispatcher.InvokeAsync(() =>
                {
                    var messageWindow = new MessageWindowElement(dm);
                    messageWindow.ShowWindow();

                    dm.ProcessCallback(messageWindow.DialogResult);
                }, DispatcherPriority.Normal);
           
        }
    }
}
