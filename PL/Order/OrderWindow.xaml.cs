using BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PL.Order
{
    /// <summary>
    /// Interaction logic for Order.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {

        private BlApi.IBl bl = BlApi.Factory.Get();
        public BO.Order? OrderData
        {
            get { return (BO.Order?)GetValue(OrderDataProperty); }
            set { SetValue(OrderDataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OrderData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrderDataProperty =
            DependencyProperty.Register("OrderData", typeof(BO.Order), typeof(Window), new PropertyMetadata(null));
        public bool StatusWindow
        {
            get { return (bool)GetValue(StatusWindowProperty); }
            set { SetValue(StatusWindowProperty, value); }
        }


        // Using a DependencyProperty as the backing store for OrderData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StatusWindowProperty =
            DependencyProperty.Register("StatusWindow2", typeof(bool), typeof(Window), new PropertyMetadata(false));



        public OrderWindow(int id,bool statusWindow)
        {
            OrderData = bl.Order.GetOrderById(id);
            StatusWindow = statusWindow;
            InitializeComponent();
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            BO.OrderStatus? status = (OrderStatus?)OrderData?.Status;
            BO.Order? order;
            try
            {
                if (status != BO.OrderStatus.SendOrder && status != BO.OrderStatus.ProvidedOrder)
                {
                    OrderData?.Items?.ToList().ForEach(item => bl.Order.UpdateAmountOfOProductInOrder(OrderData!.ID, item!.ProductID, item.Amount));
                }

                if (status == BO.OrderStatus.ConfirmedOrder && NextStatusCheckbox.IsChecked == true)
                {
                    try
                    {
                        order = bl.Order.UpdateSendOrderByManager(OrderData!.ID);
                    }
                    catch (BO.BLDoesNotExistException ex)
                    {
                        MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (BO.BLImpossibleActionException ex)
                    {
                        MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else if (status == BO.OrderStatus.SendOrder && NextStatusCheckbox.IsChecked == true)
                {
                    try
                    {
                        order = bl.Order.UpdateSupplyOrderByManager(OrderData!.ID);
                    }
                    catch (BO.BLDoesNotExistException ex)
                    {
                        MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (BO.BLImpossibleActionException ex)
                    {
                        MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }
                Close();
            }
            catch (BO.BLImpossibleActionException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }
    }
}
