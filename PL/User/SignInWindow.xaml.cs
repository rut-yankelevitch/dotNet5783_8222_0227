using BlApi;
using PL.Cart;
using PL.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
        public SignInWindow()
        {
            InitializeComponent();
            User=new BO.User();
        }

        private void ToLogin_Click(object sender, RoutedEventArgs e)
        {
            IsLogin = false;

        }

        private void ToSignIn_Click(object sender, RoutedEventArgs e)
        {
            IsLogin = true;
        }

        private void signIn_Click(object sender, RoutedEventArgs e)
        {

            try { bl.User.AddUser(User); }
            catch (BO.BLAlreadyExistException ex) { MessageBox.Show(ex.Message, ex.InnerException?.ToString(), MessageBoxButton.OK, MessageBoxImage.Error); }
            new CatalogWindow(null, User.ID, true).Show();
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            BO.Cart cart=new();;
            int? userId;
            int userId2;
            try 
            { 
                userId = bl.User.IsRegistered(User.Email, User.Password);
                cart = bl.Cart.GetCart(userId);
                userId2= (int)userId!;
                new CatalogWindow(cart, userId2, true).Show();
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
            User=new BO.User();
        }
    }
}
