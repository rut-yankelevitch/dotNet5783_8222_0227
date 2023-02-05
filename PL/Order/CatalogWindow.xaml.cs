using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BO;
using PL.Product;

namespace PL.Order
{
    /// <summary>
    /// Interaction logic for CatalogWindow.xaml
    /// </summary>
    public partial class CatalogWindow : Window
    {

        private BlApi.IBl bl = BlApi.Factory.Get();


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
            var temp = bl.Product.GetProductItemForCatalogNoFilter();
            InitializeComponent();
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
            BO.ProductItem? product = Product.SelectedItem as BO.ProductItem;
            int varInt = product!.ID;
            ProductWindow productWindow = new ProductWindow(varInt);
            productWindow.Show();
            Close();
        }
    }
}

