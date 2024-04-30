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
    /// Логика взаимодействия для RedactYsug.xaml
    /// </summary>
    public partial class RedactYsug : Window
    {
        private string connectionString = "Host=localhost;Database=TechnoPark;Username=postgres;Password=1234;Persist Security Info=True";

         
        private int idservice;
        public RedactYsug()
        {
            InitializeComponent();
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtservicename.Text) ||
        string.IsNullOrWhiteSpace(txtdescription.Text) ||
        string.IsNullOrWhiteSpace(txtprice.Text))
            {
                MessageBox.Show("Заполните все обязательные поля перед сохранением.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    
                    if (!int.TryParse(txtidservice.Text, out int idProductToUpdate))
                    {
                        MessageBox.Show("Пожалуйста, введите действительный идентификатор услуги.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    
                    string updateQuery = "UPDATE service SET servicename = @servicename, description = @description, price = @price WHERE idservice = @id";
                    NpgsqlCommand command = new NpgsqlCommand(updateQuery, connection);
                    command.Parameters.AddWithValue("@servicename", txtservicename.Text);
                    command.Parameters.AddWithValue("@description", txtdescription.Text);
                    command.Parameters.AddWithValue("@price", Convert.ToDecimal(txtprice.Text));
                    command.Parameters.AddWithValue("@id", idProductToUpdate);

                    command.ExecuteNonQuery();
                }

                DialogResult = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при сохранении данных: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Changes_Click(object sender, RoutedEventArgs e)
        {
            
            string idProductText = txtidservice.Text;

        
            if (string.IsNullOrWhiteSpace(idProductText))
            {
                MessageBox.Show("Пожалуйста, введите идентификатор услуги для удаления.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

           
            if (!int.TryParse(idProductText, out int idProductToDelete))
            {
                MessageBox.Show("Пожалуйста, введите действительный идентификатор услуги для удаления.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    
                    string deleteQuery = "DELETE FROM service WHERE idservice = @id";
                    NpgsqlCommand command = new NpgsqlCommand(deleteQuery, connection);
                    command.Parameters.AddWithValue("@id", idProductToDelete);
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Услуга успешно удален.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Не удалось удалить услугу.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при удалении услуги: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
