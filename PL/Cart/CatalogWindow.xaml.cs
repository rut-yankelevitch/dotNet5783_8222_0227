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

        public ObservableCollection<BO.ProductItem> ProductsItem
        {
            get { return (ObservableCollection<BO.ProductItem>)GetValue(ProductsItemProperty); }
            set { SetValue(ProductsItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProductsItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProductsItemProperty =
            DependencyProperty.Register("ProductsItem", typeof(ObservableCollection<BO.ProductItem>), typeof(Window), new PropertyMetadata(null));


        public CatalogWindow()
        {
            InitializeComponent();
            MyCartInCatalog = new() { Items = new() };
            Category = Category.None;
            IEnumerable<BO.ProductItem?> temp = bl.Product.GetProductItemForCatalogNoFilter();
            ProductsItem = (temp == null) ? new() : new(temp!);
        }


        public CatalogWindow(BO.Cart? cart)
        {
            InitializeComponent();
            MyCartInCatalog = cart!;
            Category = Category.None;
            IEnumerable<BO.ProductItem?> temp = bl.Product.GetProductItemForCatalogNoFilter();
            ProductsItem = (temp == null) ? new() : new(temp!);
        }


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


        private void product_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.ProductItem? productItem = ((BO.ProductItem)((ListView)sender).SelectedItem);
            ProductItemWindow? productItemWindow = new(productItem.ID, MyCartInCatalog);
            productItemWindow.Show();
            Close();
        }


        private void showCartButton_Click(Object sender, RoutedEventArgs e)
        {
            CartWindow? productItemWindow = new(MyCartInCatalog!);
            productItemWindow.Show();
            Close();
        }


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
