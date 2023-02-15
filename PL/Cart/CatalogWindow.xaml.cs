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

        private BlApi.IBl bl = BlApi.Factory.Get();
        public Category Category { get; set; }


        public ObservableCollection<BO.ProductItem> ProductsItem
        {
            get { return (ObservableCollection<BO.ProductItem>)GetValue(ProductsItemProperty); }
            set { SetValue(ProductsItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProductsItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProductsItemProperty =
            DependencyProperty.Register("ProductsItem", typeof(ObservableCollection<BO.ProductItem>), typeof(Window), new PropertyMetadata(null));

        public BO.Cart? MyCart;
        //{
        //    get { return (BO.Cart)GetValue(CartProperty); }
        //    set { SetValue(CartProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for products.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty CartProperty =
        //DependencyProperty.Register("MyCart", typeof(BO.Cart), typeof(Window), new PropertyMetadata(null));


        public CatalogWindow()
        {
            MyCart = new() { Items = new() };
            Category =  Category.None;
            InitializeComponent();
            IEnumerable<BO.ProductItem?> temp = bl.Product.GetProductItemForCatalogNoFilter();
            ProductsItem = (temp == null) ? new() : new(temp!);
            categorySelector.ItemsSource = Enum.GetValues(typeof(BO.Category));
        }
        public CatalogWindow(BO.Cart? cart)
        {
            MyCart = cart;
            Category = Category.None;
            InitializeComponent();
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

        private void Product_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.ProductItem? productItem = ((BO.ProductItem)((ListView)sender).SelectedItem);
            ProductItemWindow? productItemWindow = new(productItem.ID, MyCart);
            productItemWindow.Show();
            Close();
        }
        private void ShowCartButton_Click(Object sender ,RoutedEventArgs e)
        {

            CartWindow? productItemWindow = new(MyCart!);
            productItemWindow.Show();
            Close();
        }

        private void PopularProduct_Click(object sender, RoutedEventArgs e)
        {
            if ((string)((Button)sender).Content== "Popular Product")
            {
                try
                {
                    IEnumerable<BO.ProductItem?> temp = bl.Product.GetPopularProductList();
                    ProductsItem = (temp == null) ? new() : new(temp!);
                    ((Button)sender).Content = "to full product catalog";
                }
                catch (BO.BLDoesNotExistException ex)
                {
                    MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                IEnumerable<BO.ProductItem?> temp = bl.Product.GetProductItemForCatalogNoFilter();
                ProductsItem = (temp == null) ? new() : new(temp!);
                ((Button)sender).Content = "Popular Product";
            }
        }
    }
}

