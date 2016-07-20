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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            ButtonColumn.MinWidth = this.Width*0.15;
            ButtonColumn.MaxWidth = this.Width * 0.30;
            DBController.getDBController().UpdateProducts();
            GoodsGrid.ItemsSource = DBController.table;
            
            // Update app table after click
            AddButton.Click += UpdateItemSource;
            UpdateButton.Click += UpdateItemSource;
            IndexButton.Click += UpdateItemSource;
            DeleteButton.Click += UpdateItemSource;

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            
            var addWindow=new AddProductWindow();
            addWindow.ShowDialog();
            
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            DBController.getDBController().UpdateProducts();
            
        }

        private void IndexButton_Click(object sender, RoutedEventArgs e)
        {
            var indexWindow = new IndexWindow();
            indexWindow.ShowDialog();
            
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DBController.getDBController().DeleteProducts();
            
        }

        private void UpdateItemSource(object sender, RoutedEventArgs e)
        {
            GoodsGrid.ItemsSource = DBController.table;
        }

    }
}
