using System.Windows;

namespace PL
{
    /// <summary>
    /// main  window class
    /// </summary>
    public partial class MainWindow : Window
    {
        private BlApi.IBl bl = BlApi.Factory.Get();

        /// <summary>
        /// MainWindow constractor 
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// A function is called when Show-Products Botton Clicked
        /// </summary>
        //private void ShowProductsBotton_Click(object sender, RoutedEventArgs e)
        //{
        //    ProductListWindow productListWindow = new ProductListWindow();
        //    productListWindow.Show();
        //    Close();
        //}

        private void ShowOrderTraking_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ShowManager_Click(object sender, RoutedEventArgs e)
        {
            Manager manager= new Manager();
            manager.Show();
            Close();
        }

        private void ShowNewOrder_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
