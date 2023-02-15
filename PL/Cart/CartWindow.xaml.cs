using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace PL.Cart
{
    /// <summary>
    /// Interaction logic for CartWindow.xaml
    /// </summary>
    public partial class CartWindow : Window
    {
        BO.Cart myCart;
        private BlApi.IBl bl = BlApi.Factory.Get();
        public ObservableCollection<BO.OrderItem?> CartItems
        {
            get { return (ObservableCollection<BO.OrderItem?>)GetValue(CartItemsProperty); }
            set { SetValue(CartItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProductsItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CartItemsProperty =
            DependencyProperty.Register("CartItems", typeof(ObservableCollection<BO.OrderItem?>), typeof(Window), new PropertyMetadata(null));

        public CartWindow(BO.Cart cart)
        {
            InitializeComponent();
            myCart = cart;
            IEnumerable<BO.OrderItem?>? temp = myCart.Items;
            CartItems = (temp == null) ? new() : new(temp!);
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
                IEnumerable<BO.OrderItem?>? temp = myCart.Items;
                CartItems = (temp == null) ? new() : new(temp!);
            }
        }
    private void btn_decrease_Click(object sender, RoutedEventArgs e)
        {
            int id = ((BO.OrderItem)((Button)sender).DataContext).ProductID;
            int amount =((BO.OrderItem)((Button)sender).DataContext).Amount;
            if (amount != 1 )
            {
                bl.cart.UpdateProductAmountInCart(myCart, id, ((BO.OrderItem)((Button)sender).DataContext).Amount - 1);
                IEnumerable<BO.OrderItem?>? temp = myCart.Items;
                CartItems = (temp == null) ? new() : new(temp!);
            }
        }
        private void ReturnToCatalog_Click(object sender, RoutedEventArgs e)
        {
            CatalogWindow catalog = new();
            catalog.Show();
            Close();
        }
        private void removeFromCart_Click(object sender, RoutedEventArgs e)
        {
            int id = ((BO.OrderItem)((Button)sender).DataContext).ProductID;
            bl.cart.UpdateProductAmountInCart(myCart,id,0);
            IEnumerable<BO.OrderItem?>? temp = myCart.Items;
            CartItems = (temp == null) ? new() : new(temp!);
        }

    }
}
