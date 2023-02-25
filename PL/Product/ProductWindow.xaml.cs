using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PL.Product
{
    /// <summary>
    ///  product  window class
    /// </summary>
    public partial class ProductWindow : Window
    {

        BlApi.IBl bl = BlApi.Factory.Get();
        string? imgName;
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
                var categories = Enum.GetValues(typeof(BO.Category)).Cast<BO.Category>()
                                      .Where(c => c != BO.Category.None);
                categorySelector.ItemsSource = categories;
                if (id != -1)
                {
                    try
                    {
                        ProductData = bl.Product.GetProductByIdForManager(id);
                        imgName = ProductData.Image;

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
        /// A function is called when delete button Clicked
        private void delete_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id;
                int.TryParse(idInput.Text, out id);
                bl.Product.DeleteProduct(id);

                ProductListWindow productListWindow = new ProductListWindow();
                Close();
            }
            catch (BO.BLImpossibleActionException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ProductListWindow productListWindow = new ProductListWindow();
                Close();
            }
            catch (BO.BLDoesNotExistException ex)
            {
                MessageBox.Show(ex.Message, ex.InnerException?.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
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
            product.Image = imgName;
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
                productImg.Source = new BitmapImage(new Uri(f.FileName));
                imgName = f.FileName;
            }

        }

        private void onlyNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            string fieldValue = ((TextBox)sender).Text;
            if (((TextBox)sender).Name == "priceInput")
            {
                if (!(fieldValue == "" || int.TryParse(fieldValue, out int result) || double.TryParse(fieldValue, out double result2)))
                {
                    MessageBox.Show("The field value is not a valid number. Please enter a valid number.");
                    ((TextBox)sender).Text = "";
                }
            }
            else
            {
                if (!(fieldValue == "" || int.TryParse(fieldValue, out int result3)))
                {
                    MessageBox.Show("The field value is not a valid number. Please enter a valid number.");
                    ((TextBox)sender).Text = "";
                }
            }
        }
    }
}
