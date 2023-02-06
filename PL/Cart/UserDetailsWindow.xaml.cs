using BlApi;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for userDetails.xaml
    /// </summary>
    public partial class UserDetailsWindow : Window
    {
        BO.Cart myCart;
        private BlApi.IBl bl = BlApi.Factory.Get();
        public UserDetailsWindow( BO.Cart cart)
        {
            InitializeComponent();
            myCart = cart;
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myCart.CustomerName = NameTxt.Text;
                myCart.CustomerEmail = EmailTxt.Text;
                myCart.CustomerAddress = AddressTxt.Text;
                bl.cart.MakeOrder(myCart);
            }
            catch (BO.BLImpossibleActionException ex)
            {
                MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Close();
        }
    }
}
