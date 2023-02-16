using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace PL.Product
{
    /// <summary>
    ///  product  window class
    /// </summary>
    public partial class ProductWindow : Window
    {

        BlApi.IBl bl = BlApi.Factory.Get();
        string imgName;
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
                if (id != -1)
                {
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
                //למחוק את התמונה 
                ProductListWindow productListWindow = new ProductListWindow();
                Close();
            }
            catch (BO.BLImpossibleActionException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ProductListWindow productListWindow = new ProductListWindow();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            BO.Product product = new BO.Product();
            int varInt;
            double varDouble;
            String varString;
            int.TryParse(idInput.Text, out varInt);
            product.ID = varInt;
            product.Name = nameInput.Text;
            double.TryParse(priceInput.Text, out varDouble);
            product.Price = varDouble;
            product.Category = ((BO.Category)categorySelector.SelectedItem);
            int.TryParse(instockInput.Text, out varInt);
            product.InStock = varInt;
            product.Image = imgName;

            try
            {
                bl.Product.AddProduct(product);
                ProductListWindow productListWindow = new ProductListWindow();
                Close();
            }
            catch (BO.BLAlreadyExistException ex)
            {
                MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                ProductListWindow productListWindow = new ProductListWindow();
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
            }
        }

        private void update_Click(object sender, RoutedEventArgs e)
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
            if (imgName != null)
                product.Image = imgName;
            else
                product.Image = ProductImg.Source.ToString();
            try
            {
                bl.Product.UpdateProduct(product);
                ProductListWindow productListWindow = new ProductListWindow();

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

        private void changeImageButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog f = new Microsoft.Win32.OpenFileDialog();
            f.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            if (f.ShowDialog() == true)
            {
                this.ProductImg.Source = new BitmapImage(new Uri(f.FileName));
                imgName = f.FileName;
            }

        }
    }
}

