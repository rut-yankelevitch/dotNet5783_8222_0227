using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BlApi;
using BlImplementation;

namespace PL.Product
{
    /// <summary>
    /// Interaction logic for Product.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        //???
        IBl bl = new Bl();

        public ProductWindow()
        {
            InitializeComponent();
            categorySelector.ItemsSource =Enum.GetValues(typeof(BO.Category));
            confirmation_btn.Content = "add";
            
        }
        public ProductWindow(int id)
        {
            InitializeComponent();
            BO.Product product= new BO.Product();
            categorySelector.ItemsSource = Enum.GetValues(typeof(BO.Category));
            confirmation_btn.Content = "update";
            product =bl.Product.GetProductByIdForManager(id);

            idInput.Text =product.ID.ToString();
            idInput.IsEnabled = false;
            nameInput.Text = product.Name;
            nameInput.IsEnabled = false;
            categorySelector.SelectedValue=product.Category;
            categorySelector.IsEnabled = false;
            priceInput.Text = product.Price.ToString();
            instockInput.Text=product.InStock.ToString();
        }
        private void confirmation_btn_Click(object sender, RoutedEventArgs e)
        {
            BO.Product product = new BO.Product();
            int varInt;
            double varDouble;
            int.TryParse(idInput.Text,out varInt);
            product.ID = varInt;
            product.Name=nameInput.Text;
            double.TryParse(priceInput.Text, out varDouble);
            product.Price= varDouble;
            product.Category = ((BO.Category)categorySelector.SelectedItem);
            int.TryParse(instockInput.Text, out varInt);
            product.InStock=varInt;

            if (confirmation_btn.Content == "add")
            {
                bl.Product.AddProduct(product);
            }
            else
            {
                bl.Product.UpdateProduct(product);
            }
            ProductListWindow productListWindow = new ProductListWindow();
            productListWindow.Show();
        }

        private void idInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (idInput.Text.Length == 6)
                invalidCheck();
            else
                confirmation_btn.IsEnabled = false;
        }

        private void categorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (categorySelector.SelectedItem != null)
                invalidCheck();
            else
                confirmation_btn.IsEnabled = false;
        }

        private void nameInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (nameInput.Text.Length > 0)
                invalidCheck();
            else
                confirmation_btn.IsEnabled = false;
        }

        private void priceInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (priceInput.Text.Length > 0)
                invalidCheck();
            else
                confirmation_btn.IsEnabled = false;
        }


        private void instockInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (instockInput.Text.Length > 0)
                invalidCheck();
            else
                confirmation_btn.IsEnabled = false;
        }

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

        private void idInput_onlyNumber_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            TextBox text = sender as TextBox;
            if (text == null) return;
            if (e == null) return;
            //allow get out of the text box
            if (e.Key == Key.Enter || e.Key == Key.Return || e.Key == Key.Tab)
                return;
            //allow list of system keys (add other key here if you want to allow)
            if (e.Key == Key.Escape || e.Key == Key.Back || e.Key == Key.Delete ||
            e.Key == Key.CapsLock || e.Key == Key.LeftShift || e.Key == Key.Home
            || e.Key == Key.End || e.Key == Key.Insert || e.Key == Key.Down || e.Key == Key.Right
            ||e.Key==Key.NumPad0|| e.Key == Key.NumPad1|| e.Key == Key.NumPad2|| e.Key == Key.NumPad3|| e.Key == Key.NumPad4
            || e.Key == Key.NumPad5|| e.Key == Key.NumPad6|| e.Key == Key.NumPad7|| e.Key == Key.NumPad8|| e.Key == Key.NumPad9|)
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

    }
}

