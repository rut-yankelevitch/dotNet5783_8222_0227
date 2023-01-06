using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace PL.Product
{
    /// <summary>
    ///  product  window class
    /// </summary>
    public partial class ProductWindow : Window
    {

        BlApi.IBl bl =  BlApi.Factory.Get();


        public BO.Product ProductData
        {
            get { return (BO.Product)GetValue(ProductDataProperty); }
            set { SetValue(ProductDataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProductData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProductDataProperty =
        DependencyProperty.Register("ProductData", typeof(BO.Product), typeof(Window), new PropertyMetadata(null));



        /// <summary>
        /// ProductWindow constructor that accepts a product ID
        /// </summary>
        public ProductWindow(int id = -1)
        {
            try
            {
                InitializeComponent();
                categorySelector.ItemsSource = Enum.GetValues(typeof(BO.Category));
                if (id == -1)
                    confirmation_btn.Content = "add";
                else
                {
                    confirmation_btn.Content = "update";
                    delete_button.Visibility = Visibility.Visible;
                    try
                    {
                        ProductData = bl.Product.GetProductByIdForManager(id);
                    }
                    catch (BO.BLDoesNotExistException ex)
                    {
                        MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }


        /// <summary>
        /// A function is called when the confirmation button is clicked
        /// </summary>
        private void confirmation_btn_Click(object sender, RoutedEventArgs e)
        {
            BO.Product product = new BO.Product();
            int varInt;
            double varDouble;
            int.TryParse(idInput.Text, out varInt);
            product.ID = varInt;
            product.Name = nameInput.Text;
            double.TryParse(priceInput.Text, out varDouble);
            product.Price = varDouble;
            product.Category = ((BO.Category)categorySelector.SelectedItem);
            int.TryParse(instockInput.Text, out varInt);
            product.InStock = varInt;

            if ((string)confirmation_btn.Content == "add")
            {
                try
                {
                    bl.Product.AddProduct(product);
                    ProductListWindow productListWindow = new ProductListWindow();
                    productListWindow.Show();
                    this.Close();
                }
                catch (BO.BLAlreadyExistException ex)
                {
                    MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    ProductListWindow productListWindow = new ProductListWindow();
                    productListWindow.Show();
                    Close();

                }
                catch (BO.BLInvalidInputException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    ProductListWindow productListWindow = new ProductListWindow();
                    productListWindow.Show();
                }
            }
            else
            {
                try
                {
                    bl.Product.UpdateProduct(product);
                    ProductListWindow productListWindow = new ProductListWindow();
                    productListWindow.Show();
                    this.Close();
                }
                catch (BO.BLAlreadyExistException ex)
                {
                    MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (BO.BLInvalidInputException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        /// <summary>
        /// A function is called when idInput text changed
        /// </summary>
        private void idInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (idInput.Text.Length == 6)
                invalidCheck();
            else
                confirmation_btn.IsEnabled = false;
        }


        /// <summary>
        /// A function is called when category-selector selection changed
        /// </summary>
        private void categorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (categorySelector.SelectedItem != null)
                invalidCheck();
            else
                confirmation_btn.IsEnabled = false;
        }


        /// <summary>
        /// A function is called when name input text changed
        /// </summary>
        private void nameInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (nameInput.Text.Length > 0)
                invalidCheck();
            else
                confirmation_btn.IsEnabled = false;
        }


        /// <summary>
        /// A function is called when price input text changed
        /// </summary>
        private void priceInput_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (priceInput.Text.Length > 0)
            {

                invalidCheck();
            }
            else
                confirmation_btn.IsEnabled = false;
        }


        /// <summary>
        /// A function is called when instock input text changed
        /// </summary>
        private void instockInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (instockInput.Text.Length > 0)
                invalidCheck();
            else
                confirmation_btn.IsEnabled = false;
        }


        /// <summary>
        /// invalid check function
        /// </summary>
        private void invalidCheck()
        {
            if (idInput.Text.Length == 6 && nameInput.Text.Length > 0 && categorySelector.SelectedItem != null
                && priceInput.Text.Length > 0 && instockInput.Text.Length > 0)
            {
                confirmation_btn.IsEnabled = true;
            }
            else
            {
                confirmation_btn.IsEnabled = false;
            }
        }


        /// <summary>
        /// Numerical input function 
        /// </summary>
        private void idInput_onlyNumber_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            TextBox? text = sender as TextBox;
            if (text == null) return;
            if (e == null) return;
            //allow get out of the text box
            if (e.Key == Key.Enter || e.Key == Key.Return || e.Key == Key.Tab)
                return;
            //allow list of system keys (add other key here if you want to allow)
            if (e.Key == Key.Escape || e.Key == Key.Back || e.Key == Key.Delete ||
            e.Key == Key.CapsLock || e.Key == Key.LeftShift || e.Key == Key.Home
            || e.Key == Key.End || e.Key == Key.Insert || e.Key == Key.Down || e.Key == Key.Right
            || e.Key == Key.NumPad0 || e.Key == Key.NumPad1 || e.Key == Key.NumPad2 || e.Key == Key.NumPad3 || e.Key == Key.NumPad4
            || e.Key == Key.NumPad5 || e.Key == Key.NumPad6 || e.Key == Key.NumPad7 || e.Key == Key.NumPad8 || e.Key == Key.NumPad9)
                return;
            char c = (char)KeyInterop.VirtualKeyFromKey(e.Key);
            //allow control system keys
            if (Char.IsControl(c)) return;
            //allow digits (without Shift or Alt)
            if (Char.IsDigit(c))
                if (!(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightAlt)))
                    return; //let this key be written inside the textbox
                            //forbid letters and signs (#,$, %, ...)
            e.Handled = true; //ignore this key. mark event as handled, will not be routed to other 
            return;
        }


        /// <summary>
        /// A function is called when delete button Clicked
        private void delete_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id;
                int.TryParse(idInput.Text, out id);
                bl.Product.DeleteProduct(id);
                ProductListWindow productListWindow = new ProductListWindow();
                productListWindow.Show();
                Close();
            }
            catch (BO.BLImpossibleActionException ex)
            {
                MessageBox.Show(ex.Message,"Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ProductListWindow productListWindow = new ProductListWindow();
                productListWindow.Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

