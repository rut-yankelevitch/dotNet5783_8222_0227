using System;
using System.Windows;
using PL.Cart;
using System.Net.Mail;
namespace PL
{
    /// <summary>
    /// Interaction logic for userDetails.xaml
    /// </summary>
    public partial class UserDetailsWindow : Window
    {

        private readonly BlApi.IBl bl = BlApi.Factory.Get();
        bool isRegisted;

        public BO.Cart MyCartConfirm
        {
            get { return (BO.Cart)GetValue(MyCartConfirmProperty); }
            set { SetValue(MyCartConfirmProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyCartConfirm.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyCartConfirmProperty =
            DependencyProperty.Register("MyCartConfirm", typeof(BO.Cart), typeof(Window), new PropertyMetadata(null));


        public UserDetailsWindow(BO.Cart cart,bool isRegisted)
        {
            MyCartConfirm = cart;
            InitializeComponent();
            this.isRegisted=isRegisted;
        }


        private void return_Click(object sender, RoutedEventArgs e)
        {
            CartWindow cart = new(MyCartConfirm,isRegisted);
            cart.Show();
            Close();
        }


        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                checkInvalid();
                bl.Cart.MakeOrder(MyCartConfirm,isRegisted);
                Close();
            }
            catch (FormatException)
            {
                MessageBox.Show("Email address is not valid. Please enter a valid email address.");
            }
            catch (BO.BLImpossibleActionException ex)
            {
                MessageBox.Show(ex.Message, ex.InnerException?.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void checkInvalid()
        {

            string email = MyCartConfirm.CustomerEmail!;
            MailAddress mailAddress = new MailAddress(email);
        }
    }
    
}

