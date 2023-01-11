using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using BO;

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
            InitializeComponent();
            var temp = bl.Product.GetProductListForCustomer();
            ProductsItem = (temp == null) ? new() : new(temp!);
            categorySelector.ItemsSource = Enum.GetValues(typeof(Category));
        }

        private void categorySelector_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

            {
                BO.Category? category = categorySelector.SelectedItem as BO.Category?;

                if (category == BO.Category.None)
                {
                    var temp = bl.Product.GetProductListForCustomer();
                    ProductsItem = (temp == null) ? new() : new(temp!);
                }
                else
                {
                    var temp = bl.Product.GetProductListForCustomer();
                    ProductsItem = (temp == null) ? new() : new(temp!);
                }
            }
        }
    }
}

