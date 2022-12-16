using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BlApi;
using BlImplementation;
using BO;

namespace PL.Product
{
    /// <summary>
    /// Interaction logic for ListOfProduct.xaml
    /// </summary>
    public partial class ProductListWindow : Window
    {
        private IBl bl = new Bl();

        public ProductListWindow()
        {
            InitializeComponent();
            productListView.ItemsSource = bl.Product.GetProductListForManager();
            
            categorySelector.ItemsSource = Enum.GetValues(typeof(BO.Category));
        }

        private void categorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             BO.Category? category= categorySelector.SelectedItem as BO.Category?;
            
            if (category == BO.Category.None)
            {
                productListView.ItemsSource = bl.Product.GetProductListForManager();

            }
            else
            {
                productListView.ItemsSource=bl.Product.GetProductListForManager(BO.Filter.FilterByCategory,category);
            }
        }

        private void addProductButton_Click(object sender, RoutedEventArgs e) => new ProductWindow().Show();

        private void productListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
                BO.ProductForList product = ((BO.ProductForList)productListView.SelectedItem);

                int varInt = product.ID;
                ProductWindow productWindow = new ProductWindow(varInt);
                productWindow.Show();
        }
    }
}
