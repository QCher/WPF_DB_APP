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

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for IndexWindow.xaml
    /// </summary>
    public partial class IndexWindow : Window
    {

        
        public IndexWindow()
        {
            InitializeComponent();
            
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            double index;
            if (!Double.TryParse(TextBox.Text, out index))
            {
                MessageBox.Show("Wrong Input");
                this.DialogResult = false;
                this.Close();
                return;
            }

            DBController.getDBController().IndexProducts(Double.Parse(TextBox.Text));
            this.DialogResult = true;
            this.Close();
        }
    }
}
