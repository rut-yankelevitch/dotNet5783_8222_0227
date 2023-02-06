
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
        int productID;
        BO.Cart? cart;
        public BO.ProductItem ProductData
        {
            get { return (BO.ProductItem)GetValue(ProductDataProperty); }
            set { SetValue(ProductDataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProductData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProductDataProperty =
        DependencyProperty.Register("ProductData", typeof(BO.ProductItem), typeof(Window), new PropertyMetadata(null));
        public ProductItemWindow(int id, BO.Cart? cart)
        {
            InitializeComponent();
            this.cart = cart;
            productID = id;
            ProductData = bl.Product.GetProductByIdForCustomer(productID);

        }

        private void add_to_cart_remove_from_cart_Click(object sender, RoutedEventArgs e)
        {
            if ((string)((Button)sender).Content == "add to cart")
            {
                bl.cart.AddProductToCart(cart!, productID);
                ((Button)sender).Content = "remove from cart";
            }
            else
            {
                bl.cart.UpdateProductAmountInCart(cart!, productID, 0);
                ((Button)sender).Content = "add to cart";
            }



            //private void btn_increase_Click(object sender, RoutedEventArgs e)
            //{

            //}
        }
    }
}
