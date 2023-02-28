using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BO;
using PL.Product;

namespace PL.Cart
{
    /// <summary>
    /// Interaction logic for CatalogWindow.xaml
    /// </summary>
    public partial class CatalogWindow : Window
    {

        private readonly BlApi.IBl bl = BlApi.Factory.Get();

        public Category Category { get; set; }

        public BO.Cart MyCartInCatalog;
        bool isRegisted;

        public ObservableCollection<BO.ProductItem> ProductsItem
        {
            get { return (ObservableCollection<BO.ProductItem>)GetValue(ProductsItemProperty); }
            set { SetValue(ProductsItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProductsItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProductsItemProperty =
            DependencyProperty.Register("ProductsItem", typeof(ObservableCollection<BO.ProductItem>), typeof(Window), new PropertyMetadata(null));
        /// <summary>
        /// CatalogWindow constructor
        /// </summary>
        /// <param name="cart"></param>
        /// <param name="userId"></param>
        /// <param name="isRegisted"></param>
        public CatalogWindow(BO.Cart? cart=null,int userId= 0, bool isRegisted=false)
        {
            InitializeComponent();
            if (cart == null)
            {
                MyCartInCatalog = new() { Items = new() };
            }
            else
            {
                MyCartInCatalog = cart!;
            }
            this.isRegisted = isRegisted;
            MyCartInCatalog.UserID = userId;
            Category = Category.None;
            IEnumerable<BO.ProductItem?> temp = bl.Product.GetProductItemForCatalogNoFilter();
            ProductsItem = (temp == null) ? new() : new(temp!);
        }


        /// <summary>
        /// event category_MouseLeftButtonDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void category_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            {
                string categoryString = (string)((TextBlock)sender).Text;
                if (categoryString == "Popular Product")
                    toPopularProduct_Click(sender, e);
                else
                {
                    string categoryString2 = categoryString == "↺" ? "None" : categoryString.Replace(' ', '_');

                    BO.Category category = (BO.Category)Enum.Parse(typeof(Category), categoryString2);

                    if (category == Category.None)
                    {
                        var temp = bl.Product.GetProductItemForCatalogNoFilter();
                        ProductsItem = (temp == null) ? new() : new(temp!);
                    }
                    else
                    {
                        var temp = bl.Product.GetProducItemForCatalogByCategory(category);
                        ProductsItem = (temp == null) ? new() : new(temp!);
                    }
                }
            }
        }


        /// <summary>
        /// button product_MouseDouble Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void product_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.ProductItem? productItem = ((BO.ProductItem)((ListView)sender).SelectedItem);
            ProductItemWindow? productItemWindow = new(productItem.ID, MyCartInCatalog,isRegisted);
            productItemWindow.Show();
            Close();
        }


        /// <summary>
        /// button showCartButton Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showCartButton_Click(Object sender, RoutedEventArgs e)
        {
            CartWindow? productItemWindow = new(MyCartInCatalog!,isRegisted);
            productItemWindow.Show();
            Close();
        }


        /// <summary>
        ///  toPopularProduct Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toPopularProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IEnumerable<BO.ProductItem?> temp = bl.Product.GetPopularProductList();
                ProductsItem = (temp == null) ? new() : new(temp!);
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
