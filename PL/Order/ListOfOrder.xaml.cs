
//public partial class ProductForListWindow : Window
//{
//    //A private variable to access the logic layer
//    BlApi.IBl bl = BlApi.Factory.Get();

//    public ObservableCollection<BO.ProductForList?> Products
//    {
//        get { return (ObservableCollection<BO.ProductForList?>)GetValue(ProductsProperty); }
//        set { SetValue(ProductsProperty, value); }
//    }

//    // Using a DependencyProperty as the backing store for Products.  This enables animation, styling, binding, etc...
//    public static readonly DependencyProperty ProductsProperty =
//        DependencyProperty.Register("Products", typeof(ObservableCollection<BO.ProductForList?>), typeof(ProductForListWindow), new PropertyMetadata(null));

//    /// <summary>
//    /// A constructor action that initializes the controls
//    /// </summary>
//    public ProductForListWindow()
//    {
//        InitializeComponent();

//        //Default all products
//        cmxCategorySelector.SelectedIndex = 5;

//        //Request the logical layer to bring all the products
//        // ProductListview.ItemsSource = bl.product.GetListOfProducts();

//        var temp = bl.product.GetListOfProducts();
//        Products = temp == null ? new() : new(temp);
//        //Put all the categories
//        cmxCategorySelector.ItemsSource = Enum.GetValues(typeof(BO.Category));
//    }


using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;


namespace PL.Order;

    /// <summary>
    /// Interaction logic for ListOfOrder.xaml
    /// </summary>
    public partial class ListOfOrder : Window
    {
        private BlApi.IBl bl = BlApi.Factory.Get();
        public ObservableCollection<BO.OrderForList?> Orders
    {
            get { return (ObservableCollection<BO.OrderForList?>)GetValue(OrdersProperty); }
            set { SetValue(OrdersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for orders.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrdersProperty =
            DependencyProperty.Register("orders", typeof(ObservableCollection<BO.OrderForList?>), typeof(ListOfOrder), new PropertyMetadata(null));

        //private ObservableCollection<BO.OrderForList?>? orders= new ObservableCollection<BO.OrderForList?>();
        public ListOfOrder()
        {
            InitializeComponent();
            var temp = bl.Order.GetOrderList();
            Orders = (temp == null) ? new() : new(temp);
        }
    }

