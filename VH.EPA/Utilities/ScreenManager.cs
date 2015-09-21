using System;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Widget;
using Widget.Core;

namespace VH.View
{
    public class ScreenManager
    {
        private Rect _region;
        public WinMatrix Matrix { get; private set; }

        public Rect Region { get { return _region; } }

        public void Initialize()
        {
            _region = new Rect
                {
                    Height = SystemParameters.PrimaryScreenHeight - Envi.Margin.Top - Envi.Margin.Bottom,
                    Width = SystemParameters.PrimaryScreenWidth
                };

            Envi.RowsCount = (int)(_region.Height / (Envi.MinTileHeight - Envi.TileSpacing * 2));
            _region.Y = Envi.Margin.Top;
            _region.X = Envi.Margin.Left;
            Envi.ColumnsCount = (int)Math.Round(_region.Width * 2 / Envi.MinTileWidth);

            Matrix = new WinMatrix(Envi.ColumnsCount, Envi.RowsCount);
            Matrix.ZeroMatrix();
            Matrix[1, 0] = 1; //reserve cell for Mosaic title
        }

        public static BitmapSource GetScreenShot(int x, int y, int width, int height)
        {

            Bitmap screen = new Bitmap(width, height);

            Graphics g = Graphics.FromImage(screen);

            g.CopyFromScreen(x, y, 0, 0, new System.Drawing.Size(width, height));

            g.Dispose();

            return ConvertImage(screen);

        }

        private static BitmapSource ConvertImage(Bitmap image)
        {

            if (image == null)
            {

                return null;

            }

            return Imaging.CreateBitmapSourceFromHBitmap(image.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

        } 
    }
}