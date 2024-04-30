using Npgsql;
using SaharovaPractica.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SaharovaPractica
{
    /// <summary>
    /// Логика взаимодействия для Tovar.xaml
    /// </summary>
    public partial class Tovar : Page
    {
        public Tovar()
        {
            InitializeComponent();
            var Polzovatels = GetProductData();
            this.DataContext = Polzovatels;
            LViewProduct.ItemsSource = Polzovatels;
        }
        private ObservableCollection<product> GetProductData()
        {
            ObservableCollection<product> products = new ObservableCollection<product>();

            string connectionString = "Host=localhost;Database=TechnoPark;Username=postgres;Password=1234;Persist Security Info=True";
            string query = "SELECT * FROM product";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        { 
                            int idproduct = reader["idproduct"] != DBNull.Value ? Convert.ToInt32(reader["idproduct"]) : 0;
                            string productName = reader["productname"] != DBNull.Value ? reader["productname"].ToString() : string.Empty;
                            string description = reader["description"] != DBNull.Value ? reader["description"].ToString() : string.Empty;
                            decimal price = reader["price"] != DBNull.Value ? Convert.ToDecimal(reader["price"]) : 0m;

                            
                            if (idproduct != 0 && !string.IsNullOrWhiteSpace(productName) && productName.Length <= 40 && !string.IsNullOrWhiteSpace(description)  )
                            {
                                product prod = new product()
                                {
                                    idproduct = idproduct,
                                    productname = productName,
                                    description = description,
                                    price = price
                                };

                                products.Add(prod);
                            }
                        }
                    }
                }
            }

            return products;
        }


    


        private void LViewProduct_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RedacTow editWindow = new RedacTow();
            editWindow.DataContext = LViewProduct.SelectedItem;
            bool? result = editWindow.ShowDialog();

            if (result == true)
            {
                GetProductData();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DobawTow editWindow = new DobawTow();
            editWindow.DataContext = LViewProduct.SelectedItem;
            bool? result = editWindow.ShowDialog();

            if (result == true)
            {
                GetProductData();
            }
        }
    }
}
