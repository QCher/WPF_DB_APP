using System;
using System.Collections.Generic;
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
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
namespace WpfTest
{
    /// <summary>
    /// Interaction logic for AddProductWindow.xaml
    /// </summary>
    /// 
    
   
    public partial class AddProductWindow : Window
    {

        TableData product;
        public AddProductWindow()
        {
            InitializeComponent();
            CategoryList.Text = "Product";
            product = new TableData();
            this.DataContext = product;
        }

        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
           
        }

        private void Button_Click_OK(object sender, RoutedEventArgs e)
        {
            ProductCategory category;
            if (CategoryList.Text == "Product")
                category = ProductCategory.Product;
            else
                category = ProductCategory.Service;

            double quantity=0;
            double price=0;
            double check;
            if (!(Double.TryParse(ProductQuantity.Text, System.Globalization.NumberStyles.Any, CultureInfo.CurrentCulture, out quantity) && Double.TryParse(ProductPrice.Text, System.Globalization.NumberStyles.Any, CultureInfo.CurrentCulture, out price)) || string.IsNullOrWhiteSpace(ProductName.Text) || (price == 0) || (quantity == 0) || Double.TryParse(ProductName.Text, System.Globalization.NumberStyles.Any, CultureInfo.CurrentCulture, out check))
            {
                MessageBox.Show("Wrong Input");
                this.DialogResult = false;
                this.Close();
                return;
            }

            var item = new TableData(ProductName.Text, quantity, price, category);
            DBController.getDBController().AddProducts(item); 
            this.DialogResult = true;
            this.Close();
        }

    }
    
}
