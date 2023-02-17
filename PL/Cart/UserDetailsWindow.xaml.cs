using System.Windows;


namespace PL
{
    /// <summary>
    /// Interaction logic for userDetails.xaml
    /// </summary>
    public partial class UserDetailsWindow : Window
    {

        private readonly BlApi.IBl bl = BlApi.Factory.Get();
        public BO.Cart MyCart
        {
            get { return (BO.Cart)GetValue(MyCartProperty); }
            set { SetValue(MyCartProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyCart.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyCartProperty =
            DependencyProperty.Register("MyCart", typeof(BO.Cart), typeof(Window), new PropertyMetadata(null));

        public UserDetailsWindow( BO.Cart cart)
        {
            InitializeComponent();
            MyCart = cart;
        }


        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.cart.MakeOrder(MyCart);
            }
            catch (BO.BLImpossibleActionException ex)
            {
                MessageBox.Show( ex.Message, ex.InnerException?.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Close();
        }
    }
}
