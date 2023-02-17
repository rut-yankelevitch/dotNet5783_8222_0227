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

        private void showProductsBotton_Click(object sender, RoutedEventArgs e)
        {
            ProductListWindow productListWindow = new ProductListWindow();
            productListWindow.Show();
            Close();
        }


        private void showOrdersBotton_Click(object sender, RoutedEventArgs e)
        {
           ListOfOrder  listOfOrder = new ListOfOrder();
            listOfOrder.Show();
            Close();
        }
    }
}
