
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

    public void OrdersListview_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        BO.OrderForList order = ((BO.OrderForList)OrdersListview.SelectedItem);
        int varInt = order.ID;
        OrderWindow orderWindow = new OrderWindow(varInt,true);
        orderWindow.Show();
    }
    private void Order_List_Window_Activated(object sender, EventArgs e)
    {
        var temp = bl.Order.GetOrderList();
        Orders = (temp == null) ? new() : new(temp);
    }

}

