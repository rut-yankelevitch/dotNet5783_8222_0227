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
    /// Interaction logic for CatalogWindow.xaml
    /// </summary>
    public partial class CatalogWindow : Window
    {

        private BlApi.IBl bl = BlApi.Factory.Get();

        public ObservableCollection<BO.ProductItem> ProductsItem
        {
            get { return (ObservableCollection<BO.ProductItem>)GetValue(ProductsItemProperty); }
            set { SetValue(ProductsItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProductsItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProductsItemProperty =
            DependencyProperty.Register("ProductsItem", typeof(ObservableCollection<BO.ProductItem>), typeof(Window), new PropertyMetadata(null));


        public CatalogWindow()
        {
            InitializeComponent();
            var temp=bl.Product.prod
            ProductsItem=bl
        }
    }
}
