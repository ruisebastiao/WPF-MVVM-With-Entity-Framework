using System.Windows;
using VH.ErrorLog;
using VH.Resources;
using Widget;
using Widget.Core;

namespace VH.View
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ScreenManager WindowManager;
        public static WidgetManager WidgetManager;
        private void ApplicationDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            NLogLogger.LogError(e.Exception, ExceptionResources.ExceptionOccured);
        }

        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            Envi.Language = System.Threading.Thread.CurrentThread.CurrentUICulture.ToString();
            Envi.AnimationEnabled = true;

            WindowManager = new ScreenManager();
            WidgetManager = new WidgetManager();
        }
    }
}
