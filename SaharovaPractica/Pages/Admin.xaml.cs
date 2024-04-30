using Npgsql;
using SaharovaPractica.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Provider;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Navigation;
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
 

namespace SaharovaPractica.Pages
{
    /// <summary>
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : Page
    {
        public Admin()
        {
            InitializeComponent();
            var Polzovatels = GetEmployeeData();
            this.DataContext = Polzovatels;
            LViewProduct.ItemsSource = Polzovatels;
        }

        private ObservableCollection<employee> GetEmployeeData()
        {
            ObservableCollection<employee> employees = new ObservableCollection<employee>();

            string connectionString = "Host=localhost;Database=TechnoPark;Username=postgres;Password=1234;Persist Security Info=True";
            string query = "SELECT * FROM employee";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                             
                            employee emp = new employee()
                            {
                                idemployee = Convert.ToInt32(reader["idemployee"]),
                                idrole = Convert.ToInt32(reader["idrole"]),
                                name = reader["name"].ToString(),
                                surname = reader["surname"].ToString(),
                                patronymic = reader["patronymic"].ToString(),
                                email = reader["email"].ToString(),


                            };

                            employees.Add(emp);
                        }
                    }
                }
            }

            return employees;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Dobaw dobawPage = new Dobaw();
            bool? result = dobawPage.ShowDialog();
            if (result == true)
            {
                GetEmployeeData();
            }
        }

       

        private void LViewProduct_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Redac editWindow = new Redac();
            editWindow.DataContext = LViewProduct.SelectedItem;
            bool? result = editWindow.ShowDialog();

            if (result == true)
            {
                GetEmployeeData();
            }
        }

        private void LViewProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Clickk(object sender, RoutedEventArgs e)
        {
            Yslugi newAdminPage = new Yslugi();
            NavigationService.Navigate(newAdminPage);
        }

        private void Button_Clickkk(object sender, RoutedEventArgs e)
        {
            Tovar newAdminPage = new Tovar();
            NavigationService.Navigate(newAdminPage);
        }
    }
}