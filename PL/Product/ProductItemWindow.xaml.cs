
using PL.Cart;
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
        //int productID;
        BO.Cart? cart;
        public BO.ProductItem ProductData
        {
            get { return (BO.ProductItem)GetValue(ProductDataProperty); }
            set { SetValue(ProductDataProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ProductData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProductDataProperty =
        DependencyProperty.Register("ProductData", typeof(BO.ProductItem), typeof(Window), new PropertyMetadata(null));

        private int num = 0;
        public int Value
        {
            get { return num; }
            set
            {
                if (value > MaxValue)
                    num = MaxValue;
                else if (value < MinValue)
                    num = MinValue;
                else
                    num = value;

                amountInput.Text = num.ToString();
            }
        }
        public int MinValue { get; set; }
        //  public int MaxValue { get; set; }



        public int MaxValue
        {
            get { return (int)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(int), typeof(NumericUpDownControl), new PropertyMetadata(100));




        public ProductItemWindow(int id, BO.Cart? cart)
        {
            InitializeComponent();
            ProductData = bl.Product.GetProductByIdForCustomer(id);
            this.cart = cart;
            //productID = id;
            MaxValue = bl.Product.GetProductByIdForManager(id).InStock;
            MinValue = 0;
            var product = cart!.Items!.FirstOrDefault(item => item!.ProductID == ProductData!.ID);
            ProductData.Amount = product == null ? 0 : product.Amount;
            Value = ProductData.Amount;

        }

        private void add_to_cart_Click(object sender, RoutedEventArgs e)
        {
            //if ((string)((Button)sender).Content == "add to cart")
            //{
            if (ProductData.Amount == 0)
            {
                if (Value != 0)
                {
                    try
                    {
                        cart = bl.cart.AddProductToCart(cart!, ProductData.ID);
                        cart = bl.cart.UpdateProductAmountInCart(cart!, ProductData.ID, Value);
                        CatalogWindow catalog = new(cart);
                        catalog.Show();
                        Close();
                    }
                    catch (BO.BLDoesNotExistException ex)
                    {
                        MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                bl.cart.UpdateProductAmountInCart(cart!, ProductData.ID, Value);
                CatalogWindow catalog = new(cart);
                catalog.Show();
                Close();
            }
            //}
            //else
            //{
            //    bl.cart.UpdateProductAmountInCart(cart!, ProductData.ID, 0);
            //    ((Button)sender).Content = "add to cart";
            //}
        }
        private void remove_from_cart_Click(object sender, RoutedEventArgs e)
        {
            if (ProductData.Amount != 0)
            {
                cart = bl.cart.UpdateProductAmountInCart(cart!, ProductData.ID, 0);
                CatalogWindow catalog = new();
                catalog.Show();
                Close();
            }
        }
        private void btn_increase_Click(object sender, RoutedEventArgs e)
        {
            Value++;
        }

        private void btn_decrease_Click(object sender, RoutedEventArgs e)
        {
            Value--;
        }
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

        private void ShowCartButton_Click(object sender, RoutedEventArgs e)
        {
            CartWindow? productItemWindow = new(cart!);
            productItemWindow.Show();
            Close();
        }
        private void ReturnToCatalog_Click(object sender, RoutedEventArgs e)
        {
            CatalogWindow catalog = new();
            catalog.Show();
            Close();
        }












        //        private void cmdUp_Click(object sender, RoutedEventArgs e)
        //        {
        //            Value++;
        //        }

        //        private void cmdDown_Click(object sender, RoutedEventArgs e)
        //        {
        //            Value--;
        //        }

        //        private void txtNum_TextChanged(object sender, TextChangedEventArgs e)
        //        {
        //            if (textNumber == null || textNumber.Text == "" || textNumber.Text == "-")
        //            {
        //                Value = null;
        //                return;
        //            }

        //            if (!float.TryParse(textNumber.Text, out float val))
        //                textNumber.Text = Value.ToString();
        //            else
        //                Value = val;
        //        }
        //    }
        //}

    }
}

