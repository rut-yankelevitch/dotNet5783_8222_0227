using System.Windows;
using BO;
using PL.Order;

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

        private void ShowOrderTraking_Click(object sender, RoutedEventArgs e)
        {
            OrderTrackingWindow orderTracking = new ();
            orderTracking.Show ();
            
        }


        private void ShowManager_Click(object sender, RoutedEventArgs e)
        {
            Manager manager= new Manager();
            manager.Show();
            Close();
        }


        private void ShowCatalog_Click(object sender, RoutedEventArgs e)
        {
            CatalogWindow catalog = new CatalogWindow();
            catalog.Show();
        }
    }
}
