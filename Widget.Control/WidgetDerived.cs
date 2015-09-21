using System;
using System.Reflection;
using System.Windows;
using Widget.Base;
using Widget.Weather;

namespace Widget
{
    public class WidgetDerived : WidgetBase
    {
        private WeatherWidget widgetControl;
        public static Settings Settings;

        public override string Name
        {
            get { return "Weather"; }
        }

        public override FrameworkElement WidgetControl
        {
            get { return widgetControl; }
        }

        public override Uri IconPath
        {
            get { return new Uri("/Widget;component/Weather/WeatherResources/sun.png", UriKind.Relative); ; }
        }

        public override int ColumnSpan
        {
            get { return 2; }
        }

        public override void Load()
        {
            var weatherConfig = Assembly.GetExecutingAssembly().GetManifestResourceStream("Widget.Weather.Weather.config");
            //Settings = (Settings)XmlSerializable.Load(typeof(Settings), Envi.WidgetsRoot + "\\Weather\\Weather.config") ?? new Settings();
            Settings = (Settings)XmlSerializable.Load(typeof(Settings), weatherConfig) ?? new Settings();
            widgetControl = new WeatherWidget();
            widgetControl.Load();
        }

        public override void Unload()
        {
            Settings.Save(Envi.WidgetsRoot + "\\Weather\\Weather.config");
            widgetControl.Unload();
        }
    }
}
