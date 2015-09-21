using System;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Threading;
using System.Windows.Xps.Packaging;
using VH.ViewModel;

namespace VH.View
{
    /// <summary>
    /// Interaction logic for PrintView.xaml
    /// </summary>
    public partial class PrintView : UserControl
    {
        public PrintViewModel PrintViewModel { get; set; }

        public PrintView()
        {
            InitializeComponent();

            this.ShowReceipt();
        }

        private void ShowReceipt()
        {
             Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(delegate
            {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Assembly.GetExecutingAssembly()
                    .GetManifestResourceNames()
                    .FirstOrDefault(x => x.ToLower().Contains("Receipt.xaml".ToLower())));

           if(stream == null)
               return;

            var document = (FlowDocument)XamlReader.Load(stream);
            //document.DataContext =  PrintViewModel.Entity;

            FlowReader.Document = document;
            
            this.Dispatcher.Invoke(DispatcherPriority.SystemIdle, new Action(() => { }));
            var xpsDoc = LoadAsXPS(((IDocumentPaginatorSource)document).DocumentPaginator);
            FixedReader.Document = xpsDoc.GetFixedDocumentSequence();
            xpsDoc.Close();
            }));
        }

        private XpsDocument LoadAsXPS(DocumentPaginator paginator)
        {
            var stream = new MemoryStream();
            var docPackage = Package.Open(stream, FileMode.Create, FileAccess.ReadWrite);

            var uri = new Uri(@"memorystream://myXps.xps");
            PackageStore.RemovePackage(uri);
            PackageStore.AddPackage(uri, docPackage);
            var xpsDoc = new XpsDocument(docPackage) { Uri = uri };

            XpsDocument.CreateXpsDocumentWriter(xpsDoc).Write(paginator);

            return xpsDoc;
        }
    }
}
