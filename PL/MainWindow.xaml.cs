using System.Windows;
using PL.Order;
using PL.Cart;
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


        public string OrderId
        {
            get { return (string)GetValue(OrderIdProperty); }
            set { SetValue(OrderIdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OrderId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrderIdProperty =
            DependencyProperty.Register("OrderId", typeof(string), typeof(Window), new PropertyMetadata(null));



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


        /// <summary>
        /// Show Order Traking window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowOrderTraking_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int orderId;
                int.TryParse(OrderId, out orderId);
                OrderTrackingWindow orderTracking = new OrderTrackingWindow(orderId);
                orderTracking.Show();
                OrderId = null;
            }
            catch (BO.BLDoesNotExistException ex)
            {
                MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /// <summary>
        /// Show Manager window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowManager_Click(object sender, RoutedEventArgs e)
        {
            Manager manager= new Manager();
            manager.Show();
        }


        /// <summary>
        /// ShowCatalog window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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


        /// <summary>
        /// Simultor window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simultorBtn_Click(object sender, RoutedEventArgs e)
        {
            new SimulatorWindow(bl).Show();
        }

    }
}
