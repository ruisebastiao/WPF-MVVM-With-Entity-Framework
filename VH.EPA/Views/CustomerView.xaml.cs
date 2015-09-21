using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using VH.Model;

namespace VH.View
{
    /// <summary>
    /// Interaction logic for CustomerView.xaml
    /// </summary>
    public partial class CustomerView : UserControl
    {
        public CustomerView()
        {
            InitializeComponent();

            //var custdata = GetData();

            ////Bind the DataGrid to the customer data
            //DG1.DataContext = custdata;
        }

        //private ObservableCollection<Customer> GetData()
        //{
        //    var customers = new ObservableCollection<Customer>
        //        {
        //            new Customer {FirstName = "Orlando", LastName = "Gee", Email = "orlando0@adventure-works.com"},
        //            new Customer {FirstName = "Keith", LastName = "Harris", Email = "keith0@adventure-works.com"},
        //            new Customer {FirstName = "Donna", LastName = "Carreras", Email = "donna0@adventure-works.com"},
        //            new Customer {FirstName = "Janet", LastName = "Gates", Email = "janet0@adventure-works.com"},
        //            new Customer {FirstName = "Lucy", LastName = "Harrington", Email = "lucy0@adventure-works.com"},
        //            new Customer {FirstName = "Rosmarie", LastName = "Carroll", Email = "rosmarie0@adventure-works.com"},
        //            new Customer {FirstName = "Dominic", LastName = "Gash", Email = "dominic0@adventure-works.com"},
        //            new Customer {FirstName = "Kathleen", LastName = "Garza", Email = "kathleen0@adventure-works.com"},
        //            new Customer {FirstName = "Katherine", LastName = "Harding", Email = "katherine0@adventure-works.com"},
        //            new Customer {FirstName = "Johnny", LastName = "Caprio", Email = "johnny0@adventure-works.com"}
        //        };

        //    return customers;
        //}
    }
}
