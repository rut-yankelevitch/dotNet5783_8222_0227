using System;
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
        public BO.Cart MyCart;

        private readonly BlApi.IBl bl = BlApi.Factory.Get();

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
            MyCart = cart;
            IEnumerable<BO.OrderItem?>? temp = MyCart.Items;
            CartItems = (temp == null) ? new() : new(temp!);
            totalPrice.Text = MyCart.TotalPrice.ToString();
        }


        private void confirmOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            new UserDetailsWindow(MyCart).Show();
            Close();
        }


        private void btn_increase_Click(object sender, RoutedEventArgs e)
        {
            int id = ((BO.OrderItem)((Button)sender).DataContext).ProductID;
            try
            {
                MyCart = bl.cart.UpdateProductAmountInCart(MyCart, id, ((BO.OrderItem)((Button)sender).DataContext).Amount + 1);
                IEnumerable<BO.OrderItem?>? temp = MyCart.Items;
                CartItems = (temp == null) ? new() : new(temp!);
                totalPrice.Text = (MyCart.TotalPrice).ToString();
            }
            catch(BO.BLImpossibleActionException ex)
            {
                 MessageBox.Show(ex.Message,"Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BO.BLDoesNotExistException ex)
            {
                MessageBox.Show( ex.Message, ex.InnerException?.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void btn_decrease_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = ((BO.OrderItem)((Button)sender).DataContext).ProductID;
                int amount = ((BO.OrderItem)((Button)sender).DataContext).Amount;
                if (amount != 1)
                {
                    MyCart = bl.cart.UpdateProductAmountInCart(MyCart, id, ((BO.OrderItem)((Button)sender).DataContext).Amount - 1);
                    IEnumerable<BO.OrderItem?>? temp = MyCart.Items;
                    CartItems = (temp == null) ? new() : new(temp!);
                    totalPrice.Text = (MyCart.TotalPrice).ToString();

                }
            }
            catch (BO.BLImpossibleActionException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BO.BLDoesNotExistException ex)
            {
                MessageBox.Show(ex.Message, ex.InnerException?.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void returnToCatalog_Click(object sender, RoutedEventArgs e)
        {
            CatalogWindow catalog = new(MyCart);
            catalog.Show();
            Close();
        }


        private void removeFromCart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = ((BO.OrderItem)((Button)sender).DataContext).ProductID;
                bl.cart.UpdateProductAmountInCart(MyCart, id, 0);
                IEnumerable<BO.OrderItem?>? temp = MyCart.Items;
                CartItems = (temp == null) ? new() : new(temp!);
                totalPrice.Text = (MyCart.TotalPrice).ToString();
            }
            catch (BO.BLImpossibleActionException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BO.BLDoesNotExistException ex)
            {
                MessageBox.Show(ex.Message, ex.InnerException?.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
