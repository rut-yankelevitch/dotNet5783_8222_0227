using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
using BlApi;
using BlImplementation;
using PL.Product;

namespace PL
{
    /// <summary>
    /// main  window class
    /// </summary>
    public partial class MainWindow : Window
    {
        private IBl bl = new Bl();

        /// <summary>
        /// MainWindow constractor 
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        /// <summary>
        /// A function is called when Show-Products Botton Clicked
        /// </summary>
        private void ShowProductsBotton_Click(object sender, RoutedEventArgs e)
        {
            ProductListWindow productListWindow = new ProductListWindow();
            productListWindow.Show();
            Close();
        }


    }
}
