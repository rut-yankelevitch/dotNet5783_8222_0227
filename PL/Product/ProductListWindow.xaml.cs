using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BO;

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
        


        public Category Category { get; set; } = Category.None;
        //private int status;


        /// <summary>
        ///ProductListWindow constructor
        /// </summary>
        public ProductListWindow()
        {
            InitializeComponent();
            changeProductList();
        }

        private void product_List_Window_Activated(object sender, EventArgs e) => changeProductList(); 

        /// <summary>
        /// A function is called when a selection is changed in the category selector combobox
        /// </summary>
        private void categorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e) => changeProductList();


        /// <summary>
        /// A function is called when the add product button is clicked
        /// </summary>
        private void addProductButton_Click(object sender, RoutedEventArgs e) 
        { 
            new ProductWindow().Show();
        }

        /// <summary>
        /// A function is called when a product is clicked
        /// </summary>
        private void product_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.ProductForList? product = ((BO.ProductForList)((DataGrid)sender).SelectedItem);
            int varInt = product!.ID;

                ProductWindow productWindow = new ProductWindow(varInt);
                productWindow.Show();

        }

        private void changeProductList()
        {
            var temp = Category == Category.None ?
               bl.Product.GetProductListForManagerNoFilter()
              : bl.Product.GetProductListForManagerByCategory(Category);
            Products = (temp == null) ? new() : new(temp);
        }

    }
}
