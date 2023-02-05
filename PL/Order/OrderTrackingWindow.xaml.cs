using System.Windows;
using System.Windows.Input;
using PL.Order;


namespace PL.Order
{
    /// <summary>
    /// Interaction logic for OrderTrackingWindow.xaml
    /// </summary>
    public partial class OrderTrackingWindow : Window
    {

        private BlApi.IBl bl = BlApi.Factory.Get();
        public BO.OrderTracking TrackingOrder
        {
            get { return (BO.OrderTracking)GetValue(TrackingOrderProperty); }
            set { SetValue(TrackingOrderProperty, value); }

        }

        // Using a DependencyProperty as the backing store for OrderTracking.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TrackingOrderProperty =
            DependencyProperty.Register("TrackingOrder", typeof(BO.OrderTracking), typeof(Window), new PropertyMetadata(null));


        public OrderTrackingWindow(int id)
        {
            InitializeComponent();
            var temp = bl.Order.TrackingOrder(id);
            TrackingOrder = (temp == null) ? new() : temp;
        }

        public void showOrder_Click(object sender, RoutedEventArgs e)
        {
            OrderWindow orderWindow = new OrderWindow(TrackingOrder.ID, false);
            orderWindow.Show();
        }
    }
}

