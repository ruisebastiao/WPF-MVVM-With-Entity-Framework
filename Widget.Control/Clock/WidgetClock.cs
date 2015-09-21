using System;
using System.Windows;
using Widget.Base;

namespace Widget.Clock
{
    public class WidgetClock : WidgetDerived
    {
        private ClockWidget _widgetControl;
        public static ClockSettings Settings;

        public override string Name
        {
            get { return "Clock"; }
        }

        public override FrameworkElement WidgetControl
        {
            get { return _widgetControl; }
        }

        public override Uri IconPath
        {
            get { return new Uri("/Widget;component/Clock/ClockResources/clock.png", UriKind.Relative); }
        }

        public override int ColumnSpan
        {
            get { return 2; }
        }

        public override void Load()
        {
            //(ClockSettings)XmlSerializable.Load(typeof(ClockSettings), Envi.WidgetsRoot + "\\Clock\\Clock.config") ??
            Settings =  new ClockSettings();
            _widgetControl = new ClockWidget();
            _widgetControl.Load();
        }

        public override void Unload()
        {
            Settings.Save(Envi.WidgetsRoot + "\\Clock\\Clock.config");
            _widgetControl.Unload();
        }

        
    }
}
