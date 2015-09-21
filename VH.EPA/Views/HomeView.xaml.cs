using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Widget;
using Widget.Clock;
using Widget.Control;
using Widget.Core;

namespace VH.View
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            App.WindowManager.Initialize();
            //MarkupGrid();
            var wid = new WidgetProxy { WidgetComponent = new WidgetDerived() };
            WidgetManagerWidgetLoaded(wid);

            var widClock = new WidgetProxy { WidgetComponent = new WidgetClock() };
            WidgetManagerWidgetLoaded(widClock);
        }

        private void MarkupGrid()
        {
            WidgetHost.RowDefinitions.Clear();
            WidgetHost.ColumnDefinitions.Clear();

            for (var i = 0; i < App.WindowManager.Matrix.RowsCount; i++)
            {
                var row = new RowDefinition { Height = new GridLength(Envi.MinTileHeight) };
                WidgetHost.RowDefinitions.Add(row);
            }

            for (var i = 0; i < App.WindowManager.Matrix.ColumnsCount; i++)
            {
                var column = new ColumnDefinition { Width = new GridLength(Envi.MinTileWidth) };
                WidgetHost.ColumnDefinitions.Add(column);
            }
        }

        void WidgetManagerWidgetLoaded(WidgetProxy widget)
        {
            var control = new WidgetControl(widget) { Order = WidgetHost.Children.Count };
            control.Load();
            PlaceWidget(control);
            //WidgetHost.HorizontalAlignment = HorizontalAlignment.Right;
            WidgetHost.Children.Add(control);
        }

        public void PlaceWidget(WidgetControl widget)
        {
            var colSpan = Grid.GetColumnSpan(widget);
            AppCell cell = App.WindowManager.Matrix.GetFreeCell(colSpan);
            Grid.SetColumn(widget, (int)cell.Column);
            Grid.SetRow(widget, (int)cell.Row);
            widget.HorizontalAlignment = HorizontalAlignment.Right;
            App.WindowManager.Matrix.ReserveSpace(cell.Column, cell.Row, colSpan);
            widget.Width = colSpan * Envi.MinTileWidth - Envi.TileSpacing * 2;
            widget.Height = Envi.MinTileHeight - Envi.TileSpacing * 2;

        }
    }
}
