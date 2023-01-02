using System;
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

        /// <summary>
        ///ProductListWindow constructor
        /// </summary>
        public ProductListWindow()
        {
            InitializeComponent();
            productListView.ItemsSource = bl.Product.GetProductListForManagerNoFilter();
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
                productListView.ItemsSource = bl?.Product.GetProductListForManagerNoFilter();
            }
            else
            {
                productListView.ItemsSource=bl?.Product.GetProductListForManagerByCategory(category);
            }
        }


        /// <summary>
        /// A function is called when the add product button is clicked
        /// </summary>
        private void addProductButton_Click(object sender, RoutedEventArgs e) 
        { 
            new ProductWindow().Show();
            Close();
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
