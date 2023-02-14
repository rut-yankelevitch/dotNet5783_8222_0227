using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PL.Cart
{
    /// <summary>
    /// Interaction logic for CartWindow.xaml
    /// </summary>
    public partial class CartWindow : Window
    {
        BO.Cart myCart;
        private BlApi.IBl bl = BlApi.Factory.Get();
        public CartWindow(BO.Cart cart)
        {
            InitializeComponent();
            myCart = cart;
            ProductsItemListview.ItemsSource = myCart.Items;

        }
        private void confirmOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            new UserDetailsWindow(myCart).Show();
        }

        private void btn_increase_Click(object sender, RoutedEventArgs e)
        {
            int id = ((BO.OrderItem)((Button)sender).DataContext).ProductID;
            int amountInstock = bl.Product.GetProductByIdForManager(id).InStock;
            int amount = ((BO.OrderItem)((Button)sender).DataContext).Amount;
            if (amount < amountInstock)
            {
                bl.cart.UpdateProductAmountInCart(myCart, id, ((BO.OrderItem)((Button)sender).DataContext).Amount +1);
                ProductsItemListview.ItemsSource = myCart.Items;
            }
        }
    private void btn_decrease_Click(object sender, RoutedEventArgs e)
        {
            int id = ((BO.OrderItem)((Button)sender).DataContext).ProductID;
            int amount =((BO.OrderItem)((Button)sender).DataContext).Amount;
            if (amount != 1 )
            {
                bl.cart.UpdateProductAmountInCart(myCart, id, ((BO.OrderItem)((Button)sender).DataContext).Amount - 1);
                ProductsItemListview.ItemsSource = myCart.Items;
            }
        }

        private void removeFromCart_Click(object sender, RoutedEventArgs e)
        {
            int id = ((BO.OrderItem)((Button)sender).DataContext).ProductID;
            bl.cart.UpdateProductAmountInCart(myCart,id,0);
            ProductsItemListview.ItemsSource = myCart.Items;
        }
    }
}
