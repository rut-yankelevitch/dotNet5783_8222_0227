using PL.Cart;
using System;
using System.Net.Mail;
using System.Windows;


namespace PL.User
{
    /// <summary>
    /// Interaction logic for SignInWindow.xaml
    /// </summary>
    public partial class SignInWindow : Window
    {
        public bool IsLogin
        {
            get { return (bool)GetValue(IsLoginProperty); }
            set { SetValue(IsLoginProperty, value); }
        }
        // Using a DependencyProperty as the backing store for IsLogin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsLoginProperty =
            DependencyProperty.Register("IsLogin", typeof(bool), typeof(Window), new PropertyMetadata(false));


        public BO.User User
        {
            get { return (BO.User)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }
        // Using a DependencyProperty as the backing store for User.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UserProperty =
            DependencyProperty.Register("User", typeof(BO.User), typeof(Window), new PropertyMetadata(null));


        private readonly BlApi.IBl bl = BlApi.Factory.Get();

        /// <summary>
        /// constructor
        /// </summary>
        public SignInWindow()
        {
            User = new BO.User();
            InitializeComponent();
        }


        /// <summary>
        /// To Login form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToLogin_Click(object sender, RoutedEventArgs e)
        {
            User = new();
            IsLogin = false;

        }


        /// <summary>
        ///  To SignIn form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToSignIn_Click(object sender, RoutedEventArgs e)
        {
            User = new();
            IsLogin = true;
        }


        /// <summary>
        /// signIn user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void signIn_Click(object sender, RoutedEventArgs e)
        {
            BO.Cart cart = new(); ;
            int? userId = 0;
            try
            {
                checkInvalid();
                userId = bl.User.AddUser(User);
                cart = bl.Cart.GetCart(userId);
                new CatalogWindow(cart, (int)userId!, true).Show();
                Close();
            }
            catch (FormatException)
            {
                MessageBox.Show("Email address is not valid. Please enter a valid email address.");

            }
            catch (BO.BLAlreadyExistException ex)
            {
                MessageBox.Show(ex.Message, ex.InnerException?.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }

            catch (BO.BLDoesNotExistException ex)
            {
                MessageBox.Show(ex.Message, ex.InnerException?.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BO.BLInvalidPassword ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        /// <summary>
        /// login user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void login_Click(object sender, RoutedEventArgs e)
        {
            BO.Cart cart=new();;
            int? userId;
            try
            {

                checkInvalid();

                userId = bl.User.IsRegistered(User.Email!, User.Password!);
                cart = bl.Cart.GetCart(userId);
                new CatalogWindow(cart, (int)userId!, true).Show();
                Close();
            }
            catch (FormatException) { 
                MessageBox.Show("Email address is not valid. Please enter a valid email address."); 
                User.Email= null;
            
            }
            catch (BO.BLDoesNotExistException ex)
            {
                MessageBox.Show(ex.Message, ex.InnerException?.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BO.BLInvalidPassword ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// check Invalid email
        /// </summary>
        private void checkInvalid()
        {

            string email = User.Email!;
            MailAddress mailAddress = new MailAddress(email);
        }
    }
}
