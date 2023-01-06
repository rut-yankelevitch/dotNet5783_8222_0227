using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DalApi;

namespace PL.Product
{
    /// <summary>
    /// product list window calss
    /// </summary>
    public partial class ProductListWindow : Window
    {
        private BlApi.IBl bl =BlApi.Factory.Get();
        public ObservableCollection<BO.ProductForList?> Products
        {
            get { return (ObservableCollection<BO.ProductForList?>)GetValue(ProductsProperty); }
            set { SetValue(ProductsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for products.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProductsProperty =
        DependencyProperty.Register("Products", typeof(ObservableCollection<BO.ProductForList?>), typeof(Window), new PropertyMetadata(null));

        
        /// <summary>
        ///ProductListWindow constructor
        /// </summary>
        public ProductListWindow()
        {
            InitializeComponent();
            var temp = bl.Product.GetProductListForManagerNoFilter();
            Products = (temp == null) ? new() : new(temp);
            categorySelector.ItemsSource = Enum.GetValues(typeof(BO.Category));
        }


        /// <summary>
        /// A function is called when a selection is changed in the category selector combobox
        /// </summary>
        private void categorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             BO.Category? category= categorySelector.SelectedItem as BO.Category?;
            
            if (category == BO.Category.None)
            {
                var temp = bl.Product.GetProductListForManagerNoFilter();
                Products = (temp == null) ? new() : new(temp);
            }
            else
            {
                var temp = bl.Product.GetProductListForManagerByCategory(category);
                Products = (temp == null) ? new() : new(temp);
            }
        }


        /// <summary>
        /// A function is called when the add product button is clicked
        /// </summary>
        private void addProductButton_Click(object sender, RoutedEventArgs e) 
        { 
            new ProductWindow().Show();
            //
            Close();
            //
        }


        /// <summary>
        /// A function is called when a product is clicked
        /// </summary>
        private void productListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
                BO.ProductForList product = ((BO.ProductForList)productListView.SelectedItem);
                int varInt = product.ID;
                ProductWindow productWindow = new ProductWindow(varInt);
                productWindow.Show();
            Close();
        }
    }
}
