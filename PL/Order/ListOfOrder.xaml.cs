using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PL.Order
{
    /// <summary>
    /// Interaction logic for ListOfOrder.xaml
    /// </summary>
    public partial class ListOfOrder : Window
    {
        private BlApi.IBl bl = BlApi.Factory.Get();
        //private ObservableCollection<BO.OrderForList?>? orders= new ObservableCollection<BO.OrderForList?>();
        public ListOfOrder()
        {
            InitializeComponent();
            var tmpOrder= bl.Order.GetOrderList();
            OrderForListData orders = (tmpOrder == null) ? new() : new(tmpOrder) ;

            listViewOrders.DataContext = orders;
        }
    }
    public class OrderForListData: DependencyObject
    {
        public ObservableCollection<BO.OrderForList> orders
        {
            get { return (ObservableCollection<BO.OrderForList>)GetValue(ordersProperty); }
            set { SetValue(ordersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for orders.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ordersProperty =
            DependencyProperty.Register("orders", typeof(ObservableCollection<BO.OrderForList>), typeof(OrderForListData), new PropertyMetadata(0));
    }
}
