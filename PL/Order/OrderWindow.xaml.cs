using BO;
using System;
using System.ComponentModel;
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


        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="statusWindow"></param>
        public OrderWindow(int id, bool statusWindow)
        {
            try
            {
                OrderData = bl.Order.GetOrderById(id);
            }
            catch (BO.BLDoesNotExistException ex)
            {
                MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            StatusWindow = statusWindow;
            InitializeComponent();
        }


        /// <summary>
        /// update the order
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void update_Click(object sender, RoutedEventArgs e)
        {
            BO.OrderStatus? status = (OrderStatus?)OrderData?.Status;
            try
            {
                if (status == BO.OrderStatus.Confirmed_Order)
                {
                    try
                    {
                        OrderData = bl.Order.UpdateSendOrderByManager(OrderData!.ID);
                    }
                    catch (BO.BLDoesNotExistException ex)
                    {
                        MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (BO.BLImpossibleActionException ex)
                    {
                        MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    (sender as Button)!.Visibility = Visibility.Hidden;
                }
                else if (status == BO.OrderStatus.Send_Order)
                {
                    try
                    {
                        OrderData = bl.Order.UpdateSupplyOrderByManager(OrderData!.ID);
                    }
                    catch (BO.BLDoesNotExistException ex)
                    {
                        MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (BO.BLImpossibleActionException ex)
                    {
                        MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    (sender as Button)!.Visibility = Visibility.Hidden;
                }
            }
            catch (BO.BLImpossibleActionException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /// <summary>
        /// update amount in order Item
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productAmount"></param>
        private void amountChange(int productId,int productAmount)
        {

                var temp = bl.Order.UpdateAmountOfOProductInOrder(OrderData!.ID, productId, productAmount);
                if (temp != null)
                {
                    OrderData = temp;
                }
                else
                {
                    Close();
                }
        }


        /// <summary>
        /// decrease amount in orderItem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void decrease_btn(object sender, RoutedEventArgs e)
        {
            var prodId = (TypeDescriptor.GetProperties((sender as Button)?.DataContext!)["ProductID"]?.GetValue((sender as Button)?.DataContext))!;
            string strHelp = prodId?.ToString()!;
            strHelp = strHelp ?? "0";
            int productId;
            int.TryParse(strHelp, out productId);
            var amount = (TypeDescriptor.GetProperties((sender as Button)?.DataContext!)["Amount"]?.GetValue((sender as Button)?.DataContext))!;
            strHelp = amount?.ToString() ?? "0";
            int productAmount;
            int.TryParse(strHelp, out productAmount);
            try
            {
                amountChange( productId, productAmount - 1);
            }
            catch (BO.BLImpossibleActionException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                var orderItem = OrderData?.Items!.FirstOrDefault<BO.OrderItem>(prod => prod.ProductID == productId);
                if (orderItem != null)
                {
                    (sender as TextBlock)!.Text = orderItem?.Amount.ToString();
                }
            }
            catch (BO.BLDoesNotExistException ex)
            {
                MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                var orderItem = OrderData?.Items!.FirstOrDefault<BO.OrderItem>(prod => prod.ProductID == productId);
                if (orderItem != null)
                {
                    (sender as TextBlock)!.Text = orderItem?.Amount.ToString();
                }
            }
            catch (BO.BLInvalidInputException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                var orderItem = OrderData?.Items!.FirstOrDefault<BO.OrderItem>(prod => prod.ProductID == productId);
                if (orderItem != null)
                {
                    (sender as TextBlock)!.Text = orderItem?.Amount.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                var orderItem = OrderData?.Items!.FirstOrDefault<BO.OrderItem>(prod => prod.ProductID == productId);
                if (orderItem != null)
                {
                    (sender as TextBlock)!.Text = orderItem?.Amount.ToString();
                }
            }


        }
        
        
        /// <summary>
        /// increase amount of orderItem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void increase_btn(object sender, RoutedEventArgs e)
        {
            var prodId = (TypeDescriptor.GetProperties((sender as Button)?.DataContext!)["ProductID"]?.GetValue((sender as Button)?.DataContext))!;
            string strHelp = prodId?.ToString()!;
            strHelp = strHelp ?? "0";
            int productId;
            int.TryParse(strHelp, out productId);
            var amountTextBlock = (TypeDescriptor.GetProperties((sender as Button)?.DataContext!)["Amount"]?.GetValue((sender as Button)?.DataContext))!;
            strHelp = amountTextBlock?.ToString() ?? "0";
            int productAmount;
            int.TryParse(strHelp, out productAmount);
            try
            {
                amountChange( productId, productAmount + 1);
            }
            catch (BO.BLImpossibleActionException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                var orderItem = OrderData?.Items!.FirstOrDefault<BO.OrderItem>(prod => prod.ProductID == productId);
                if (orderItem != null)
                {
                    amountTextBlock = orderItem?.Amount.ToString();
                }
            }
            catch (BO.BLDoesNotExistException ex)
            {
                MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                var orderItem = OrderData?.Items!.FirstOrDefault<BO.OrderItem>(prod => prod.ProductID == productId);
                if (orderItem != null)
                {
                    amountTextBlock = orderItem?.Amount.ToString();
                }
            }
            catch (BO.BLInvalidInputException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                var orderItem = OrderData?.Items!.FirstOrDefault<BO.OrderItem>(prod => prod.ProductID == productId);
                if (orderItem != null)
                {
                    amountTextBlock = orderItem?.Amount.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                var orderItem = OrderData?.Items!.FirstOrDefault<BO.OrderItem>(prod => prod.ProductID == productId);
                if (orderItem != null)
                {
                    amountTextBlock = orderItem?.Amount.ToString();
                }
            }

        }


        /// <summary>
        /// close the order window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (StatusWindow == true)
            {
                ListOfOrder listOfOrder = new ListOfOrder();
                listOfOrder.Show();
            }
            else
            {
                OrderTrackingWindow orderTracking = new OrderTrackingWindow(OrderData!.ID);
                orderTracking.Show();
            }
            Close();
        }
    }
}
