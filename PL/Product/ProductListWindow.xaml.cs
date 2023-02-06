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
            //this.status = status;   
            //if(status== 1)
            //{
            //    addProductButton.Visibility= Visibility.Hidden;
            //}
        }

        private void Product_List_Window_Activated(object sender, EventArgs e) => changeProductList(); 

        /// <summary>
        /// A function is called when a selection is changed in the category selector combobox
        /// </summary>
        private void CategorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e) => changeProductList();


        /// <summary>
        /// A function is called when the add product button is clicked
        /// </summary>
        private void addProductButton_Click(object sender, RoutedEventArgs e) 
        { 
            new ProductWindow().Show();
            ////
            //Close();
            ////
        }

        /// <summary>
        /// A function is called when a product is clicked
        /// </summary>
        private void Product_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.ProductForList? product = ((BO.ProductForList)((DataGrid)sender).SelectedItem);
            int varInt = product!.ID;
            //if (this.status == 0)
            //{
                ProductWindow productWindow = new ProductWindow(varInt);
                productWindow.Show();
            //}
            //else
            //{
            //    ProductItemWindow productItemWindow = new(varInt);
            //    productItemWindow.Show();

            //}
        }

        private void changeProductList()
        {
            var temp = Category == Category.None ?
               bl.Product.GetProductListForManagerNoFilter()
              : bl.Product.GetProductListForManagerByCategory(Category);
            Products = (temp == null) ? new() : new(temp);
        }

        //private void Product_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{

        //}
    }
}
