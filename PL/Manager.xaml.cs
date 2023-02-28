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

        /// <summary>
        /// constructor
        /// </summary>
        public Manager()
        {
            InitializeComponent();
        }


        /// <summary>
        ///  show Products window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showProductsBotton_Click(object sender, RoutedEventArgs e)
        {
            ProductListWindow productListWindow = new ProductListWindow();
            productListWindow.Show();
            Close();
        }


        /// <summary>
        /// show Orders window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showOrdersBotton_Click(object sender, RoutedEventArgs e)
        {
           ListOfOrder  listOfOrder = new ListOfOrder();
            listOfOrder.Show();
            Close();
        }
    }
}
