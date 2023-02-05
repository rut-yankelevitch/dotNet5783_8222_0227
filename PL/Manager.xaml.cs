using System.Windows;
using PL.Product;
using PL.Order;

namespace PL
{
    /// <summary>
    /// Interaction logic for Manager.xaml
    /// </summary>
    public partial class Manager : Window
    {
        public Manager()
        {
            InitializeComponent();
        }



        private void ShowProductsBotton_Click(object sender, RoutedEventArgs e)
        {
            ProductListWindow productListWindow = new ProductListWindow();
            productListWindow.Show();
            Close();
        }


        private void ShowOrdersBotton_Click(object sender, RoutedEventArgs e)
        {
           ListOfOrder  listOfOrder = new ListOfOrder();
            listOfOrder.Show();
            Close();
        }
    }
}
