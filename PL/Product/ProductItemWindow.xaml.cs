﻿
using PL.Cart;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PL.Product
{
    /// <summary>
    /// Interaction logic for ProductItemWindow.xaml
    /// </summary>
    public partial class ProductItemWindow : Window
    {
        private BlApi.IBl bl = BlApi.Factory.Get();
        BO.Cart? cart;
        bool isRegisted;
        public BO.ProductItem ProductItemData
        {
            get { return (BO.ProductItem)GetValue(ProductItemDataProperty); }
            set { SetValue(ProductItemDataProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ProductData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProductItemDataProperty =
        DependencyProperty.Register("ProductItemData", typeof(BO.ProductItem), typeof(Window), new PropertyMetadata(null));

        private int num = 0;
        public int Value
        {
            get { return num; }
            set
            {
                if (value > MaxValue)
                    num = MaxValue;
                else
                    num = value;

                amountInput.Text = num.ToString();
            }
        }

        public int MaxValue
        {
            get { return (int)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }
        // Using a DependencyProperty as the backing store for MaxValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(int), typeof(Window), new PropertyMetadata(0));


        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cart"></param>
        /// <param name="isRegisted"></param>
        public ProductItemWindow(int id, BO.Cart? cart, bool isRegisted)
        {
            ProductItemData = bl.Product.GetProductByIdForCustomer(id);
            var product = cart!.Items!.FirstOrDefault(item => item!.ProductID == ProductItemData!.ID);
            ProductItemData.Amount = product == null ? 0 : product.Amount;
            InitializeComponent();
            this.cart = cart;
            MaxValue = bl.Product.GetProductByIdForManager(id).InStock;
            Value = ProductItemData.Amount;
            this.isRegisted = isRegisted;

        }

        /// <summary>
        /// add order item to cart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void add_to_cart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cart = bl.Cart.AddProductToCart(cart!, ProductItemData.ID, Value, isRegisted);
                CatalogWindow catalog = new(cart, (int)cart.UserID!, isRegisted);
                catalog.Show();
                Close();
            }
            catch (BO.BLDoesNotExistException ex)
            {
                MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BO.BLImpossibleActionException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BO.BLInvalidInputException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            var product = cart!.Items!.FirstOrDefault(item => item!.ProductID == ProductItemData!.ID);
            ProductItemData.Amount = product == null ? 0 : product.Amount;
            Value = ProductItemData.Amount;
        }

        /// <summary>
        /// update cart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void update_to_cart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.Cart.UpdateProductAmountInCart(cart!, ProductItemData.ID, Value, isRegisted);
                CatalogWindow catalog = new(cart, (int)cart?.UserID!, isRegisted);
                catalog.Show();
                Close();
            }
            catch (BO.BLDoesNotExistException ex)
            {
                MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BO.BLImpossibleActionException ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            var product = cart!.Items!.FirstOrDefault(item => item!.ProductID == ProductItemData!.ID);
            ProductItemData.Amount = product == null ? 0 : product.Amount;
            Value = ProductItemData.Amount;
        }


        /// <summary>
        /// remove product from cart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void remove_from_cart_Click(object sender, RoutedEventArgs e)
        {
            if (ProductItemData.Amount != 0)
            {
                cart = bl.Cart.UpdateProductAmountInCart(cart!, ProductItemData.ID, 0, isRegisted);
                CatalogWindow catalog = new(cart, (int)cart?.UserID!, isRegisted);
                catalog.Show();
                Close();
            }
        }

        /// <summary>
        /// increase amount in product in cart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btn_increase_Click(object sender, RoutedEventArgs e) => Value++;

        /// <summary>
        /// decrese amount in product in cart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_decrease_Click(object sender, RoutedEventArgs e) => Value--;


       /// <summary>
       /// update amount in text
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void txtNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (amountInput == null || amountInput.Text == "" || amountInput.Text == "-")
            {
                Value = 0;
                return;
            }

            if (!int.TryParse(amountInput.Text, out int val))
                amountInput.Text = Value.ToString();
            else
                Value = val;
        }


        /// <summary>
        /// show window cart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showCartButton_Click(object sender, RoutedEventArgs e)
        {
            CartWindow? cartWindow = new(cart!, isRegisted);
            cartWindow.Show();
            Close();
        }


        /// <summary>
        /// return to catalog window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void returnToCatalog_Click(object sender, RoutedEventArgs e)
        {
            CatalogWindow catalog = new(cart, (int)cart?.UserID!, isRegisted);
            catalog.Show();
            Close();
        }
    }
}

