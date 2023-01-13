using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

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


        public bool StatusWindow { get; set; } = true;
        public OrderWindow(int id,bool statusWindow)
        {
            StatusWindow=statusWindow;
            InitializeComponent();
            OrderData = bl.Order.GetOrderById(id);
            IEnumerable<BO.OrderStatus> allStatus = (IEnumerable<BO.OrderStatus>)Enum.GetValues(typeof(BO.OrderStatus));

            var filterStatus = allStatus.Where(status => status == BO.OrderStatus.SendOrder || status == BO.OrderStatus.ProvidedOrder);
            Status.ItemsSource= filterStatus;

        }

        private void UpdateStatus_Click(object sender, RoutedEventArgs e)
        {
            BO.OrderStatus? status = Status.SelectedItem as BO.OrderStatus?;
            BO.Order? order;

                    if (status == BO.OrderStatus.SendOrder)
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
                    else
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
                }
            }
}
