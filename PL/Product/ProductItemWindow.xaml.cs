
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

                amountInput.Text =  num.ToString();
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
            this.cart = cart;
            //productID = id;
            ProductData = bl.Product.GetProductByIdForCustomer(id);
            MaxValue = bl.Product.GetProductByIdForManager(id).InStock;
            MinValue = 0;
            Value = 0;

        }

        private void add_to_cart_remove_from_cart_Click(object sender, RoutedEventArgs e)
        {
            if ((string)((Button)sender).Content == "add to cart")
            {
                if (ProductData.Amount ==0)
                {
                    if (Value != 0)
                    {
                        cart = bl.cart.AddProductToCart(cart!, ProductData.ID);

                        cart = bl.cart.UpdateProductAmountInCart(cart!, ProductData.ID,Value-1);
                        ((Button)sender).Content = "remove from cart";
                        Close();
                    }
                }
                else
                {
                    bl.cart.UpdateProductAmountInCart(cart!, ProductData.ID,Value);
                }
            }
            else
            {
                bl.cart.UpdateProductAmountInCart(cart!, ProductData.ID, 0);
                ((Button)sender).Content = "add to cart";
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

