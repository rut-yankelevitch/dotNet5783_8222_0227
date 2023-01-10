using System.Windows;
namespace PL.Order
{
    /// <summary>
    /// Interaction logic for OrderTrackingWindow.xaml
    /// </summary>
    public partial class OrderTrackingWindow : Window
    {

        private BlApi.IBl bl = BlApi.Factory.Get();
        public BO.OrderTracking OrderTracking
        {
            get { return (BO.OrderTracking)GetValue(OrderTrackingProperty); }
            set { SetValue(OrderTrackingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OrderTracking.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrderTrackingProperty =
            DependencyProperty.Register("OrderTracking", typeof(BO.OrderTracking), typeof(Window), new PropertyMetadata(null));


        public OrderTrackingWindow()
        {
            InitializeComponent();
            var temp = bl.Order.TrackingOrder(1);
            OrderTracking = (temp == null) ? new() : temp;

        }
    }
}
