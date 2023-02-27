using System.Windows;
using BO;
using PL.Order;
using PL.Cart;
using PL.Product;
using System;
using System.Windows.Media.Imaging;
using PL.User;

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
        /// 
        public MainWindow()
        {
            InitializeComponent();
            Uri iconUri = new(@"../img/icon.ico",UriKind.RelativeOrAbsolute);
            Icon = BitmapFrame.Create(iconUri);
        }

        private void ShowOrderTraking_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int orderId;
                int.TryParse(OrderId.Text, out orderId);
                OrderTrackingWindow orderTracking = new OrderTrackingWindow(orderId);
                orderTracking.Show();
            }
            catch (BO.BLDoesNotExistException ex)
            {
                MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowManager_Click(object sender, RoutedEventArgs e)
        {
            Manager manager= new Manager();
            manager.Show();
        }
        private void ShowCatalog_Click(object sender, RoutedEventArgs e)
        {
                if (MessageBox.Show("Do you want to sign?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    new SignInWindow().Show();
                }
                else
                {
                CatalogWindow catalog = new();
                catalog.Show();
            }

        }

        private void simultorBtn_Click(object sender, RoutedEventArgs e)
        {
            //new SimulatorWindow(bl).Show();
        }

    }
}
