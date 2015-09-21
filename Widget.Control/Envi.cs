using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace Widget
{
    public static class Envi
    {
        public static string Root { get; private set; }
        public static string WidgetsRoot { get; private set; }
        public static string AppsRoot { get; private set; }

        public static double MinTileHeight { get; set; }
        public static double MinTileWidth { get; set; }
        public static int TileSpacing { get; set; } //a space between tiles
        public static int RowsCount { get; set; }
        public static int ColumnsCount { get; set; }
        public static Thickness Margin { get; set; } //mosaic region margins
        public static string Language { get; set; }
        public static bool AnimationEnabled { get; set; }
        public static Color BackgroundColor { get; set; }

        static Envi()
        {
            TileSpacing = 4;
            Margin = new Thickness(0, 0, 0, 0);
            MinTileHeight = 180;
            MinTileWidth = 180;

            Root = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            WidgetsRoot = Root + "\\Widgets";
            AppsRoot = Root + "\\Apps";
        }
    }
}