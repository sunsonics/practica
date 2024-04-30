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
    /// Логика взаимодействия для Yslugi.xaml
    /// </summary>
    public partial class Yslugi : Page
    {
        public Yslugi()
        {
            InitializeComponent();
            var Polzovatels = GetProductData();
            this.DataContext = Polzovatels;
            LViewProduct.ItemsSource = Polzovatels;
        }
        private ObservableCollection<service> GetProductData()
        {
            ObservableCollection<service> services = new ObservableCollection<service>();

            string connectionString = "Host=localhost;Database=TechnoPark;Username=postgres;Password=1234;Persist Security Info=True";
            string query = "SELECT * FROM service";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            service prod = new service()
                            {
                                idservice = Convert.ToInt32(reader["idservice"]),
                                servicename = reader["servicename"].ToString(),
                                description = reader["description"].ToString(),
                                price = Convert.ToDecimal(reader["price"])
                            };

                            services.Add(prod);
                        }
                    }
                }
            }

            return services;
        }

         
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DobavYslugi editWindow = new DobavYslugi();
            editWindow.DataContext = LViewProduct.SelectedItem;
            bool? result = editWindow.ShowDialog();

            if (result == true)
            {
                GetProductData();
            }
        }

        private void LViewProduct_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RedactYsug editWindow = new RedactYsug();
            editWindow.DataContext = LViewProduct.SelectedItem;
            bool? result = editWindow.ShowDialog();

            if (result == true)
            {
                GetProductData();
            }
        }
    }
}
