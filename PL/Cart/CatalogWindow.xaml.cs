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
                categorySelector.ItemsSource = Enum.GetValues(typeof(BO.Category));
        }


        public CatalogWindow(BO.Cart? cart)
        {
            InitializeComponent();
            MyCartInCatalog = cart!;
            Category = Category.None;
            IEnumerable<BO.ProductItem?> temp = bl.Product.GetProductItemForCatalogNoFilter();
            ProductsItem = (temp == null) ? new() : new(temp!);
            categorySelector.ItemsSource = Enum.GetValues(typeof(BO.Category));
        }


        private void categorySelector_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            {
                BO.Category? category = categorySelector.SelectedItem as BO.Category?;
                if(category == Category.None)
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


        private void product_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.ProductItem? productItem = ((BO.ProductItem)((ListView)sender).SelectedItem);
            ProductItemWindow? productItemWindow = new(productItem.ID, MyCartInCatalog);
            productItemWindow.Show();
            Close();
        }


        private void showCartButton_Click(Object sender ,RoutedEventArgs e)
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
                ((Button)sender).Visibility = Visibility.Hidden;
                toFullCatalog_Button.Visibility = Visibility.Visible;
            }
            catch (BO.BLDoesNotExistException ex)
            {
                MessageBox.Show(ex.Message, ex.InnerException?.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void toFullCatalog_Click(object sender, RoutedEventArgs e)
        {
                IEnumerable<BO.ProductItem?> temp = bl.Product.GetProductItemForCatalogNoFilter();
                ProductsItem = (temp == null) ? new() : new(temp!);
                ((Button)sender).Visibility = Visibility.Hidden;
                toPopularProduct_Button.Visibility = Visibility.Visible;
        }
    }
}

