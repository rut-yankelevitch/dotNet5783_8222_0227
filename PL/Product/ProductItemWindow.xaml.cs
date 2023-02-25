
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


        public ProductItemWindow(int id, BO.Cart? cart)
        {
            ProductItemData = bl.Product.GetProductByIdForCustomer(id);
            var product = cart!.Items!.FirstOrDefault(item => item!.ProductID == ProductItemData!.ID);
            ProductItemData.Amount = product == null ? 0 : product.Amount;
            InitializeComponent();
            this.cart = cart;
            MaxValue = bl.Product.GetProductByIdForManager(id).InStock;
            Value = ProductItemData.Amount;
        }


        private void add_to_cart_Click(object sender, RoutedEventArgs e)
        {
                    try
                    {
                        cart = bl.cart.AddProductToCart(cart!, ProductItemData.ID, Value);
                        CatalogWindow catalog = new(cart);
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
        private void update_to_cart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.cart.UpdateProductAmountInCart(cart!, ProductItemData.ID, Value);
                CatalogWindow catalog = new(cart);
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
        private void remove_from_cart_Click(object sender, RoutedEventArgs e)
        {
            if (ProductItemData.Amount != 0)
            {
                cart = bl.cart.UpdateProductAmountInCart(cart!, ProductItemData.ID, 0);
                CatalogWindow catalog = new(cart);
                catalog.Show();
                Close();
            }
        }


        private void btn_increase_Click(object sender, RoutedEventArgs e) => Value++;


        private void btn_decrease_Click(object sender, RoutedEventArgs e) => Value--;


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


        private void showCartButton_Click(object sender, RoutedEventArgs e)
        {
            CartWindow? productItemWindow = new(cart!);
            productItemWindow.Show();
            Close();
        }


        private void returnToCatalog_Click(object sender, RoutedEventArgs e)
        {
            CatalogWindow catalog = new(cart);
            catalog.Show();
            Close();
        }
    }
}

