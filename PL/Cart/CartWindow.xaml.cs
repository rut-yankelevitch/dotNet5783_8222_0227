﻿using System;
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
        bool isRegisted;
        int userId;
        private readonly BlApi.IBl bl = BlApi.Factory.Get();

        public ObservableCollection<BO.OrderItem?> CartItems
        {
            get { return (ObservableCollection<BO.OrderItem?>)GetValue(CartItemsProperty); }
            set { SetValue(CartItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProductsItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CartItemsProperty =
            DependencyProperty.Register("CartItems", typeof(ObservableCollection<BO.OrderItem?>), typeof(Window), new PropertyMetadata(null));


        public double TotalPrice
        {
            get { return (double)GetValue(TotalPriceProperty); }
            set { SetValue(TotalPriceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TotalPrice.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TotalPriceProperty =
            DependencyProperty.Register("TotalPrice", typeof(double), typeof(Window), new PropertyMetadata(0.0));


        /// <summary>
        /// CartWindow constructor
        /// </summary>
        /// <param name="cart"></param>
        /// <param name="isRegister"></param>
        public CartWindow(BO.Cart cart,bool isRegister)
        {
            InitializeComponent();
            MyCart = cart;
            userId = (int)MyCart.UserID!;
            this.isRegisted = isRegister;
            IEnumerable<BO.OrderItem?>? temp = MyCart.Items;
            CartItems = (temp == null) ? new() : new(temp!);
            TotalPrice = (double)MyCart?.TotalPrice!;
        }


        /// <summary>
        /// confirm order Btn Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void confirmOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            new UserDetailsWindow(MyCart,isRegisted).Show();
            Close();
        }


        /// <summary>
        /// button btn_increase Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_increase_Click(object sender, RoutedEventArgs e)
        {
            int id = ((BO.OrderItem)((Button)sender).DataContext).ProductID;
            try
            {
                MyCart = bl.Cart.UpdateProductAmountInCart(MyCart, id, ((BO.OrderItem)((Button)sender).DataContext).Amount + 1,isRegisted);
                IEnumerable<BO.OrderItem?>? temp = MyCart.Items;
                CartItems = (temp == null) ? new() : new(temp!);
                TotalPrice = (double)MyCart?.TotalPrice!;
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


        /// <summary>
        /// button btn_decrease Click 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_decrease_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = ((BO.OrderItem)((Button)sender).DataContext).ProductID;
                int amount = ((BO.OrderItem)((Button)sender).DataContext).Amount;
                if (amount != 1)
                {
                    MyCart = bl.Cart.UpdateProductAmountInCart(MyCart, id, ((BO.OrderItem)((Button)sender).DataContext).Amount - 1,isRegisted);
                    IEnumerable<BO.OrderItem?>? temp = MyCart.Items;
                    CartItems = (temp == null) ? new() : new(temp!);
                    TotalPrice = (double)MyCart.TotalPrice;

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


        /// <summary>
        /// button returnToCatalog Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void returnToCatalog_Click(object sender, RoutedEventArgs e)
        {
            CatalogWindow catalog = new(MyCart,userId,isRegisted);
            catalog.Show();
            Close();
        }
         

        /// <summary>
        ///button removeFromCart Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeFromCart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = ((BO.OrderItem)((Button)sender).DataContext).ProductID;
                bl.Cart.UpdateProductAmountInCart(MyCart, id, 0,isRegisted);
                IEnumerable<BO.OrderItem?>? temp = MyCart.Items;
                CartItems = (temp == null) ? new() : new(temp!);
                TotalPrice = (double)MyCart.TotalPrice;
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

