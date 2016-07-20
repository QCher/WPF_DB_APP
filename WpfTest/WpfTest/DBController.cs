using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
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
    public class DBController
    {
        public static DbProviderFactory providerFactory { get; set; }
        public static List<TableData> table { get; set; }
        public static string connectionStr { get; set; }
        static DBController()
        {
            instance = new DBController();
            providerFactory = DbProviderFactories.GetFactory(ConfigurationManager.ConnectionStrings["DefaultConnection"].ProviderName);
            connectionStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            table = new List<TableData>();
        }
        private DBController()
        {

        }
        private static DBController instance;
        public static DBController getDBController()
        {
            return instance;
        }

        public void UpdateProducts()
        {
            using (DbConnection connection = providerFactory.CreateConnection())
            {
                // Configurate connection
                connection.ConnectionString = connectionStr;
                connection.Open();
                // Configurate command
                DbCommand cmd = providerFactory.CreateCommand();
                cmd.Connection = connection;
                cmd.CommandText = "select Storage.ProductName, Quantity, Price,Category from Storage inner join Product on Storage.ProductName=Product.ProductName";
                // Read data by DataReader
                using (DbDataReader dR = cmd.ExecuteReader())
                {
                    table = new List<TableData>();
                    while (dR.Read())
                    {
                        ProductCategory category;
                        if (dR["Category"].ToString() == "P")
                        {
                            category = ProductCategory.Product;
                        }
                        else
                        {
                            category = ProductCategory.Service;
                        }
                        table.Add(new TableData(dR["ProductName"].ToString(), Double.Parse(dR["Quantity"].ToString()), Double.Parse(dR["Price"].ToString()), category));
                    }
                }
            }
        }

        public void DeleteProducts()
        {
            // Configurate connection
            using (DbConnection connection = providerFactory.CreateConnection())
            {
                connection.ConnectionString = connectionStr;
                connection.Open();
                // Configurate command for delete object
                using (DbCommand cmd = providerFactory.CreateCommand())
                {
                    cmd.Connection = connection;

                    foreach (TableData element in table)
                    {
                        if (element.Mark == true)
                        {

                           
                            string sqlDeleteCmd = string.Format("delete  from Storage  where ProductName='{0}' and Price={1}", element.Name, element.Price);
                            cmd.CommandText = sqlDeleteCmd;
                            cmd.ExecuteNonQuery();


                            if (!ExsistSameInStorage(element))
                            {
                                string sqlDeleteProduct = string.Format("delete  from Product  where ProductName='{0}'", element.Name);
                                cmd.CommandText = sqlDeleteProduct;
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }

            UpdateProducts();
        }

        public void IndexProducts(double index)
        {
            //Configurate connection
            using (DbConnection connection = providerFactory.CreateConnection())
            {
                connection.ConnectionString = connectionStr;
                connection.Open();
                //Configurate command
                using (DbCommand cmd = providerFactory.CreateCommand())
                {
                    cmd.Connection = connection;

                    foreach (TableData element in table)
                    {
                        string sqlUpdateCmd = string.Format("update Storage set Price=Price+Price*{0}*0.01", index);
                        cmd.CommandText = sqlUpdateCmd;
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            UpdateProducts();
        }

        public void AddProducts(TableData product)
        {
            if (ExsistInProduct(product))
            {
                if (ExsistIndenticalInStorage(product))
                    AddExistingQuntity(product);
                else
                    AddExistingProduct(product);

            }
            else
            {
                AddNewProduct(product);
            
            }

        }







        public void AddNewProduct(TableData product)
        {

            if (product == null)
                return;
            string category;
            if (product.Category == ProductCategory.Product)
                category = "P";
            else
                category = "S";

            //Configurate connection
            using (DbConnection connection = providerFactory.CreateConnection())
            {
                connection.ConnectionString = connectionStr;
                connection.Open();
                //Configurate command
                using (DbCommand cmd = providerFactory.CreateCommand())
                {
                    cmd.Connection = connection;
                    string sqlAddToProduct = string.Format("insert into  Product (ProductName,Category) values(@ProductName, @Category)");
                    cmd.CommandText = sqlAddToProduct;

                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@ProductName";
                    param.Value = product.Name;
                    param.SqlDbType = System.Data.SqlDbType.VarChar;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter();
                    param.ParameterName = "@Category";
                    param.Value = category;
                    param.SqlDbType = System.Data.SqlDbType.VarChar;
                    cmd.Parameters.Add(param);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        MessageBox.Show("Some SQL error\n" + e.Message);

                    }

                }
                using (DbCommand cmd = providerFactory.CreateCommand())
                {
                    cmd.Connection = connection;
                    string sqlInsertToStorage = string.Format("insert into  Storage (ProductName, Quantity, Price) values(@ProductName, @Quantity, @Price)");//,product.Name,product.Quantity,product.Price);
                    cmd.CommandText = sqlInsertToStorage;

                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@ProductName";
                    param.Value = product.Name;
                    param.SqlDbType = System.Data.SqlDbType.VarChar;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter();
                    param.ParameterName = "@Quantity";
                    param.Value = product.Quantity;
                    param.SqlDbType = System.Data.SqlDbType.Float;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter();
                    param.ParameterName = "@Price";
                    param.Value = product.Price;
                    param.SqlDbType = System.Data.SqlDbType.Float;
                    cmd.Parameters.Add(param);


                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        MessageBox.Show("Some SQL error\n" + e.Message);

                    }
                }
            }

            UpdateProducts();

        }

        public void AddExistingProduct(TableData product)
        {

            if (product == null)
                return;
            string category;
            if (product.Category == ProductCategory.Product)
                category = "P";
            else
                category = "S";

            //Configurate connection
            using (DbConnection connection = providerFactory.CreateConnection())
            {
                connection.ConnectionString = connectionStr;
                connection.Open();
                //Configurate command

                using (DbCommand cmd = providerFactory.CreateCommand())
                {
                    cmd.Connection = connection;
                    string sqlInsertToStorage = string.Format("insert into  Storage (ProductName, Quantity, Price) values(@ProductName, @Quantity, @Price)");//,product.Name,product.Quantity,product.Price);
                    cmd.CommandText = sqlInsertToStorage;

                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@ProductName";
                    param.Value = product.Name;
                    param.SqlDbType = System.Data.SqlDbType.VarChar;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter();
                    param.ParameterName = "@Quantity";
                    param.Value = product.Quantity;
                    param.SqlDbType = System.Data.SqlDbType.Float;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter();
                    param.ParameterName = "@Price";
                    param.Value = product.Price;
                    param.SqlDbType = System.Data.SqlDbType.Float;
                    cmd.Parameters.Add(param);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        MessageBox.Show("Some SQL error\n" + e.Message);

                    }
                }
            }

            UpdateProducts();

        }

        public void AddExistingQuntity(TableData product)
        {


            if (product == null)
                return;
            string category;
            if (product.Category == ProductCategory.Product)
                category = "P";
            else
                category = "S";

            //Configurate connection
            using (DbConnection connection = providerFactory.CreateConnection())
            {
                connection.ConnectionString = connectionStr;
                connection.Open();
                //Configurate command

                using (DbCommand cmd = providerFactory.CreateCommand())
                {
                    cmd.Connection = connection;
                    string sqlInsertToStorage = string.Format("update  Storage set Quantity=Quantity+@Quantity where ProductName=@ProductName and Price=@Price");//,product.Name,product.Quantity,product.Price);
                    cmd.CommandText = sqlInsertToStorage;

                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@ProductName";
                    param.Value = product.Name;
                    param.SqlDbType = System.Data.SqlDbType.VarChar;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter();
                    param.ParameterName = "@Quantity";
                    param.Value = product.Quantity;
                    param.SqlDbType = System.Data.SqlDbType.Float;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter();
                    param.ParameterName = "@Price";
                    param.Value = product.Price;
                    param.SqlDbType = System.Data.SqlDbType.Float;
                    cmd.Parameters.Add(param);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        MessageBox.Show("Some SQL error\n" + e.Message);

                    }
                }
            }

            UpdateProducts();

        }

        public bool ExsistInProduct(TableData product)
        {

            if (product == null)
                return false;

            //Configurate connection
            using (DbConnection connection = providerFactory.CreateConnection())
            {
                connection.ConnectionString = connectionStr;
                connection.Open();
                //Configurate command
                using (DbCommand cmd_check_Product = providerFactory.CreateCommand())
                {
                    cmd_check_Product.Connection = connection;
                    string sqlCheckProduct = string.Format("select count(*) Count from Product where ProductName=@ProductName");
                    cmd_check_Product.CommandText = sqlCheckProduct;

                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@ProductName";
                    param.Value = product.Name;
                    param.SqlDbType = System.Data.SqlDbType.VarChar;
                    cmd_check_Product.Parameters.Add(param);

                    using (DbDataReader dR = cmd_check_Product.ExecuteReader())
                    {
                            dR.Read();

                            if (Double.Parse(dR["Count"].ToString()) == 0)
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                    }
                }
            }
        }

        public bool ExsistIndenticalInStorage(TableData product)
        { 
        
            
            if (product == null)
                return false;

            //Configurate connection
            using (DbConnection connection = providerFactory.CreateConnection())
            {
                connection.ConnectionString = connectionStr;
                connection.Open();
                //Configurate command
                using (DbCommand cmd_check_Product = providerFactory.CreateCommand())
                {
                    cmd_check_Product.Connection = connection;
                    string sqlCheckProduct = string.Format("select count(*) Count from Storage where ProductName=@ProductName and Price=@Price");
                    cmd_check_Product.CommandText = sqlCheckProduct;

                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@ProductName";
                    param.Value = product.Name;
                    param.SqlDbType = System.Data.SqlDbType.VarChar;
                    cmd_check_Product.Parameters.Add(param);

                    SqlParameter param2 = new SqlParameter();
                    param2.ParameterName = "@Price";
                    param2.Value = product.Price;
                    param2.SqlDbType = System.Data.SqlDbType.Float;
                    cmd_check_Product.Parameters.Add(param2);



                    using (DbDataReader dR = cmd_check_Product.ExecuteReader())
                    {
                            dR.Read();

                            if (Double.Parse(dR["Count"].ToString()) == 0)
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                    }
                }
            }
        }


        public bool ExsistSameInStorage(TableData product)
        {


            if (product == null)
                return false;

            //Configurate connection
            using (DbConnection connection = providerFactory.CreateConnection())
            {
                connection.ConnectionString = connectionStr;
                connection.Open();
                //Configurate command
                using (DbCommand cmd_check_Product = providerFactory.CreateCommand())
                {
                    cmd_check_Product.Connection = connection;
                    string sqlCheckProduct = string.Format("select count(*) Count from Storage where ProductName=@ProductName");
                    cmd_check_Product.CommandText = sqlCheckProduct;

                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@ProductName";
                    param.Value = product.Name;
                    param.SqlDbType = System.Data.SqlDbType.VarChar;
                    cmd_check_Product.Parameters.Add(param);

                    


                    using (DbDataReader dR = cmd_check_Product.ExecuteReader())
                    {
                        dR.Read();

                        if (Double.Parse(dR["Count"].ToString()) == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
        }



      

      




    }
}


