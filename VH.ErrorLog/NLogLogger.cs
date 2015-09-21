using System;
using GalaSoft.MvvmLight.Messaging;
using VH.SimpleUI.Entities;
using nlog = NLog;

namespace VH.ErrorLog
{
    public static class NLogLogger
    {
        public static nlog.Logger Instance { get; private set; }

        static NLogLogger()
        {
            nlog.LogManager.ReconfigExistingLoggers();

            Instance = nlog.LogManager.GetCurrentClassLogger();
        }

        public static void LogError(Exception exception, string message = "")
        {
            Instance.ErrorException(message, exception);
        }

        public static void LogError(Exception exception, string errorType = "Error", string message = "", string userMessage = "")
        {
            Instance.ErrorException(message, exception);
            string userFrenMsg = userMessage + Environment.NewLine + exception.Message;
            var messageDailog = new MessageDailog()
                {
                    Caption = userFrenMsg,
                    DialogButton = DialogButton.Ok,
                    Title = errorType,
                    IsSizeToContent = true
                };
            Messenger.Default.Send(messageDailog);
        }


    }
}

