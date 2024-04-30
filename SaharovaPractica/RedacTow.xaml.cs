using Npgsql;
using SaharovaPractica.Model;
using System;
using System.Windows;

namespace SaharovaPractica
{
    public partial class RedacTow : Window
    {
         
        private string connectionString = "Host=localhost;Database=TechnoPark;Username=postgres;Password=1234;Persist Security Info=True";

       
        private int idproduct;

        public RedacTow()
        {
            InitializeComponent();
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtproductname.Text) ||
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

                 
                    if (!int.TryParse(txtidproduct.Text, out int idProductToUpdate))
                    {
                        MessageBox.Show("Пожалуйста, введите действительный идентификатор продукта.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                   
                    string updateQuery = "UPDATE product SET productname = @productname, description = @description, price = @price WHERE idproduct = @id";
                    NpgsqlCommand command = new NpgsqlCommand(updateQuery, connection);
                    command.Parameters.AddWithValue("@productname", txtproductname.Text);
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
         
            string idProductText = txtidproduct.Text;

           
            if (string.IsNullOrWhiteSpace(idProductText))
            {
                MessageBox.Show("Пожалуйста, введите идентификатор товара для удаления.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            
            if (!int.TryParse(idProductText, out int idProductToDelete))
            {
                MessageBox.Show("Пожалуйста, введите действительный идентификатор товара для удаления.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                   
                    string deleteQuery = "DELETE FROM product WHERE idproduct = @id";
                    NpgsqlCommand command = new NpgsqlCommand(deleteQuery, connection);
                    command.Parameters.AddWithValue("@id", idProductToDelete);
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Товар успешно удален.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Не удалось удалить товар.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при удалении товара: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}


