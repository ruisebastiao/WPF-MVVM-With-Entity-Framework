using System;
using System.Globalization;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using UserControl = System.Windows.Controls.UserControl;

namespace Widget.Clock
{
    /// <summary>
    /// Interaction logic for ClockWidget.xaml
    /// </summary>
    public partial class ClockWidget : UserControl
    {
        private DispatcherTimer timer;
        
        private DispatcherTimer autoLockTimer;
        private bool isLocked;

        public ClockWidget()
        {
            InitializeComponent();
        }

        public void Load()
        {
            timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(1)};
            timer.Tick += TimerTick;
            timer.Start();


            autoLockTimer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(WidgetClock.Settings.AutolockTime)};
            autoLockTimer.Tick += AutoLockTimerTick;

            Day.Text = DateTime.Now.ToString("dddd");
            Day.Text = char.ToUpper(Day.Text[0]) + Day.Text.Substring(1);
            Date.Text = DateTime.Now.ToString("MMMM") + " " + DateTime.Now.Day;

            TimerTick(null, EventArgs.Empty);
        }

        void AutoLockTimerTick(object sender, EventArgs e)
        {
            //if (!isLocked)
            //    Lock();
        }

        private System.Drawing.Point _lastMousePos;
        void TimerTick(object sender, EventArgs e)
        {
            Time.Text = DateTime.Now.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern);

            var power = SystemInformation.PowerStatus;
            if (power.BatteryChargeStatus == BatteryChargeStatus.NoSystemBattery)
                BatteryIcon.Visibility = Visibility.Collapsed;
            else
            {
                BatteryIcon.Visibility = Visibility.Visible;
                var iconNumber = (int)(power.BatteryLifePercent * 10) + 1;
                if (iconNumber >= 10)
                    BatteryIcon.Source = new BitmapImage(new Uri("/Widget;component/Clock/ClockResources/batt10.png", UriKind.Relative));
                else
                    BatteryIcon.Source = new BitmapImage(new Uri(string.Format("/Widget;component/Clock/ClockResources/batt{0}.png", iconNumber), UriKind.Relative));
            }

            if (WidgetClock.Settings.Autolock)
            {
                var mousePos = System.Windows.Forms.Control.MousePosition;
                if (mousePos == _lastMousePos)
                {
                    if (!autoLockTimer.IsEnabled)
                    {
                        autoLockTimer.Start();
                    }
                }
                else
                {
                    _lastMousePos = mousePos;
                    autoLockTimer.Stop();
                }
            }
        }

        
        public void Unload()
        {
            timer.Tick -= TimerTick;
            timer.Stop();
        }
    }
}
