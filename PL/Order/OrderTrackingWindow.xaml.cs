using System.Windows;


namespace PL.Order
{
    /// <summary>
    /// Interaction logic for OrderTrackingWindow.xaml
    /// </summary>
    public partial class OrderTrackingWindow : Window
    {

        private readonly BlApi.IBl bl = BlApi.Factory.Get();

        public BO.OrderTracking TrackingOrder
        {
            get { return (BO.OrderTracking)GetValue(TrackingOrderProperty); }
            set { SetValue(TrackingOrderProperty, value); }

        }

        // Using a DependencyProperty as the backing store for OrderTracking.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TrackingOrderProperty =
            DependencyProperty.Register("TrackingOrder", typeof(BO.OrderTracking), typeof(Window), new PropertyMetadata(null));

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="id"></param>
        public OrderTrackingWindow(int id)
        {
                InitializeComponent();
                var temp = bl.Order.TrackingOrder(id);
                TrackingOrder = (temp == null) ? new() : temp;
        }

        /// <summary>
        /// show order window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showOrder_Click(object sender, RoutedEventArgs e)
        {
            OrderWindow orderWindow = new OrderWindow(TrackingOrder.ID, false);
            orderWindow.Show();
            Close();    
        }
    }
}

