using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
namespace WpfTest
{
    public enum ProductCategory{Product,Service};
    public class TableData : IDataErrorInfo //, DependencyObject
    {


        /*
        public static readonly DependencyProperty DescriptionProperty;
        

        static TableData()
        {
            DescriptionProperty = DependencyProperty.Register("Description",typeof(string),typeof(TableData));
        }
        
        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty)}
        }

        */
        
        public TableData(string name, double quantity, double price, ProductCategory category)
        {
            this.Name = name;
            this.Quantity = quantity;
            this.Price = price;
            this.Category = category;
            this.Mark = false;
        }

        public TableData()
        {
        }


        public string Name { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public ProductCategory Category { get; set; }
        public bool Mark { get; set; }

        public string this[string columnName]
        {
            get
            {
                string result = String.Empty;
                switch (columnName)
                {
                    case "Quantity":
                        if (Quantity <= 0)
                        {
                            result = "Negative";
                           
                        }
                        break;
                    case "Name":
                        if(string.IsNullOrWhiteSpace(Name))
                        {
                            result = "Null string";
                        }
                        break;
                    case "Price":
                        if (Price <= 0)
                        {
                            result = "Negative";
                        }
                        break;
                    case "Category":
                        if (Category==null)
                        {
                            result = " Null";
                        }
                        break;
                }
                return result;
            }
        }
        public string Error
        {
            get { throw new NotImplementedException(); }
        }
    
    
    
    }
}
