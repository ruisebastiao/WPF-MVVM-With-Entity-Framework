using System;
using System.Linq;
using System.Reflection;

namespace Widget.Core
{
    public class WidgetProxy
    {
        public readonly string Path;
        private Assembly assembly;
        public WidgetDerived WidgetComponent { get; set; }
        //private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public bool IsLoaded { get; private set; }
        public string Name { get; private set; }
        public bool HasErrors { get; private set; }
        public int Column { get; set; }
        public int Row { get; set; }
        public WidgetType WidgetType { get; set; }

         public WidgetProxy()
         {
             
         }
        public WidgetProxy(string path, string name = null, bool isGenerated = false, bool isSocial = false)
        {
            Path = path;
            Column = -1;
            Row = -1;

            if (isGenerated)
            {
                InitializeGenerated();
                return;
            }

            Initialize();
        }

        private void Initialize()
        {
            Type widgetType = null;
            try
            {
                assembly = Assembly.LoadFrom(Path);
                widgetType = assembly.GetTypes().FirstOrDefault(type => typeof(WidgetDerived).IsAssignableFrom(type));
            }
            catch (ReflectionTypeLoadException ex)
            {
               // logger.Error("Failed to load provider from " + Path + ".\n" + ex);
                HasErrors = true;
                return;
            }

            if (widgetType == null)
            {
                //logger.Error("Failed to find IWeatherProvider in " + Path);
                HasErrors = true;
                return;
            }

            WidgetComponent = Activator.CreateInstance(widgetType) as WidgetDerived;
            if (WidgetComponent == null)
            {
                HasErrors = true;
                return;
            }

            Name = WidgetComponent.Name;
            WidgetType = WidgetType.Native;
        }

        private void InitializeGenerated()
        {
            //WidgetComponent = Path.StartsWith("http://") ? new MosaicWebPreviewWidget() : new AppWidget();
            WidgetComponent =  new AppWidget();
            WidgetType = WidgetType.Generated;
            Name = string.Empty;
        }

       public void Load()
        {
            if (WidgetType == WidgetType.Generated)
                if (string.IsNullOrEmpty(Name))
                    WidgetComponent.Load(Path);
                else
                    WidgetComponent.Load(Path, Name, Environment.TickCount * Row * Column);
            else
                WidgetComponent.Load();
            IsLoaded = true;
        }

        public void Unload()
        {
            WidgetComponent.Unload();
            IsLoaded = false;
        }
    }
}
