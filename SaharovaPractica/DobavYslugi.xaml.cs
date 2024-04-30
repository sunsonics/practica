using Npgsql;
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

namespace SaharovaPractica
{
    /// <summary>
    /// Логика взаимодействия для DobavYslugi.xaml
    /// </summary>
    public partial class DobavYslugi : Window
    {
        private string connectionString = "Host=localhost;Database=TechnoPark;Username=postgres;Password=1234;Persist Security Info=True";
        public DobavYslugi()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtservicename.Text) ||
                 string.IsNullOrWhiteSpace(txtdescription.Text) ||
                 string.IsNullOrWhiteSpace(txtprice.Text))
            {
                MessageBox.Show("Заполните все обязательные поля перед сохранением.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            
            decimal price;
            if (!decimal.TryParse(txtprice.Text, out price))
            {
                MessageBox.Show("Пожалуйста, введите действительную цену.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (txtservicename.Text.Length > 50)
            {
                MessageBox.Show("Название услуги не может превышать 50 символов..", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string getLastIdQuery = "SELECT idservice FROM service ORDER BY idservice DESC LIMIT 1;";
                    NpgsqlCommand getLastIdCommand = new NpgsqlCommand(getLastIdQuery, connection);
                    object lastId = getLastIdCommand.ExecuteScalar();
                    int newServiceId = (lastId != null && lastId != DBNull.Value) ? Convert.ToInt32(lastId) + 1 : 1;

                    string insertQuery = "INSERT INTO service (idservice, servicename, description, price ) " +
                                         "VALUES (@idservice, @servicename, @description, @price )";
                    NpgsqlCommand insertCommand = new NpgsqlCommand(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@idservice", newServiceId);
                    insertCommand.Parameters.AddWithValue("@servicename", txtservicename.Text);
                    insertCommand.Parameters.AddWithValue("@description", txtdescription.Text);
                    insertCommand.Parameters.AddWithValue("@price", price);

                    insertCommand.ExecuteNonQuery();
                }

                DialogResult = true;  
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при сохранении данных: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CleanButton_Click(object sender, RoutedEventArgs e)
        {
            txtservicename.Text = string.Empty;
            txtdescription.Text = string.Empty;
            txtprice.Text = string.Empty;
        }
    }
}
