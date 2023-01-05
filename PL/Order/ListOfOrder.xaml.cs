
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
            DependencyProperty.Register("Orders", typeof(ObservableCollection<BO.OrderForList?>), typeof(Window), new PropertyMetadata(null));

        public ListOfOrder()
        {
            InitializeComponent();
            var temp = bl.Order.GetOrderList();
            Orders = (temp == null) ? new() : new(temp);
        }
    }

