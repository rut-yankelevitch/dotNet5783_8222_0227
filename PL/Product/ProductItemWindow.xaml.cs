
using System.Windows;




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
        public BO.ProductItem ProductData1
        {
            get { return (BO.ProductItem)GetValue(ProductDataProperty); }
            set { SetValue(ProductDataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProductData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProductDataProperty =
        DependencyProperty.Register("ProductData1", typeof(BO.ProductItem), typeof(Window), new PropertyMetadata(null));
        public ProductItemWindow(int id, BO.Cart? cart)
          {
              InitializeComponent();
              this.cart = cart;
              productID = id;
              ProductData1 = bl.Product.GetProductByIdForCustomer(productID);

          }

        }
    
}
