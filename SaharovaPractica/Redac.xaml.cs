using SaharovaPractica.Model;
using Npgsql;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SaharovaPractica
{
    public partial class Redac : Window
    {
        private string connectionString = "Host=localhost;Port=5432;Database=TechnoPark;Username=postgres;Password=1234";

        public Redac()
        {
            InitializeComponent();
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtname.Text) ||
        string.IsNullOrWhiteSpace(txtsurname.Text) ||
        string.IsNullOrWhiteSpace(txtpatronymic.Text) ||
        string.IsNullOrWhiteSpace(txtemail.Text))
            {
                MessageBox.Show("Заполните все обязательные поля перед сохранением.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                     
                    int idEmployeeToUpdate = Convert.ToInt32(idemployee.Text);

                   
                    string updateQuery = "UPDATE employee SET name = @name, surname = @surname, patronymic = @patronymic, email = @email WHERE idemployee = @id";
                    NpgsqlCommand command = new NpgsqlCommand(updateQuery, connection);
                    command.Parameters.AddWithValue("@name", txtname.Text);
                    command.Parameters.AddWithValue("@surname", txtsurname.Text);
                    command.Parameters.AddWithValue("@patronymic", txtpatronymic.Text);
                    command.Parameters.AddWithValue("@email", txtemail.Text);
                    command.Parameters.AddWithValue("@id", idEmployeeToUpdate);

                    command.ExecuteNonQuery();
                }

                DialogResult = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при удалении сотрудника: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Changes_Click(object sender, RoutedEventArgs e)
        {
           
            string idEmployeeText = idemployee.Text;

          
            if (string.IsNullOrWhiteSpace(idEmployeeText))
            {
                MessageBox.Show("Пожалуйста, введите идентификатор сотрудника для удаления.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

             
            if (!int.TryParse(idEmployeeText, out int idEmployeeToDelete))
            {
                MessageBox.Show("Пожалуйста, введите допустимый идентификатор сотрудника для удаления.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                   
                    string deleteQuery = "DELETE FROM employee WHERE idemployee = @id";
                    NpgsqlCommand command = new NpgsqlCommand(deleteQuery, connection);
                    command.Parameters.AddWithValue("@id", idEmployeeToDelete);
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Сотрудник успешно удален.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Не удалось удалить сотрудника.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"При удалении сотрудника произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}