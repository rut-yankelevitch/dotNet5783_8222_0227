using BO;
using DO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace PL.Order
{
    /// <summary>
    /// Interaction logic for Order.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {

        private readonly BlApi.IBl bl = BlApi.Factory.Get();
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


        private void amountChange(object sender, RoutedEventArgs e)
        {
            try
            {

                var prodId = (TypeDescriptor.GetProperties((sender as TextBox)?.DataContext!)["ProductID"]
    ?.GetValue((sender as TextBox)?.DataContext))!;
                string strHelp = prodId?.ToString()!;
                strHelp=strHelp ?? "0";
                int productId;
                int.TryParse(strHelp, out productId);
                strHelp = (sender as TextBox)?.Text!;
                int productAmount;
                int.TryParse(strHelp, out productAmount);
                bl.Order.UpdateAmountOfOProductInOrder(OrderData!.ID, productId,productAmount);


            }
            catch (BO.BLImpossibleActionException ex)
            {
                MessageBox.Show(ex.Message,"Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BO.BLDoesNotExistException ex)
            {
                MessageBox.Show(ex.InnerException?.ToString(),ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BO.BLInvalidInputException ex)
            {
                var prodId = (TypeDescriptor.GetProperties((sender as TextBox)?.DataContext!)["ProductID"]
?.GetValue((sender as TextBox)?.DataContext))!;
                string strHelp = prodId?.ToString()!;
                strHelp = strHelp ?? "0";
                int productId;
                int.TryParse(strHelp, out productId);
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                var orderItem = OrderData?.Items!.FirstOrDefault<BO.OrderItem>(prod => prod.ProductID == productId);
                if (orderItem != null)
                {
                    (sender as TextBox).Text = orderItem?.Amount.ToString();
                }
            }
        }




    }
}
