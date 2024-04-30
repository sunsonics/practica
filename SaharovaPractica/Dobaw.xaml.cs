using Npgsql;
using SaharovaPractica.Model;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace SaharovaPractica
{
    public partial class Dobaw : Window
    {
        private string connectionString = "Host=localhost;Database=TechnoPark;Username=postgres;Password=1234;Persist Security Info=True";
        public Dobaw()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIma.Text) ||
                 string.IsNullOrWhiteSpace(txtsurname.Text) ||
                 string.IsNullOrWhiteSpace(txtpatronymic.Text) ||
                 string.IsNullOrWhiteSpace(txtemail.Text) ||
                 myComboBox.SelectedItem == null)
            {
                MessageBox.Show("Заполните все обязательные поля перед сохранением.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!IsValidName(txtIma.Text))
            {
                MessageBox.Show("Пожалуйста, введите допустимое имя.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!IsValidSurname(txtsurname.Text))
            {
                MessageBox.Show("Пожалуйста, введите допустимую фамилию.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!IsValidPatronymic(txtpatronymic.Text))
            {
                MessageBox.Show("Пожалуйста, введите допустимое отчество.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                  
                    string getLastIdQuery = "SELECT idemployee FROM employee ORDER BY idemployee DESC LIMIT 1;";
                    NpgsqlCommand getLastIdCommand = new NpgsqlCommand(getLastIdQuery, connection);
                    object lastId = getLastIdCommand.ExecuteScalar();
                    int newEmployeeId = (lastId != null && lastId != DBNull.Value) ? Convert.ToInt32(lastId) + 1 : 1;

                   
                    string insertQuery = "INSERT INTO employee (idemployee, name, surname, patronymic, email, idrole) " +
                                         "VALUES (@idemployee, @name, @surname, @patronymic, @email, @idrole)";
                    NpgsqlCommand insertCommand = new NpgsqlCommand(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@idemployee", newEmployeeId);
                    insertCommand.Parameters.AddWithValue("@name", txtIma.Text);
                    insertCommand.Parameters.AddWithValue("@surname", txtsurname.Text);
                    insertCommand.Parameters.AddWithValue("@patronymic", txtpatronymic.Text);
                    insertCommand.Parameters.AddWithValue("@email", txtemail.Text);
                    insertCommand.Parameters.AddWithValue("@idrole", Convert.ToInt32((myComboBox.SelectedItem as ComboBoxItem).Tag));

                    insertCommand.ExecuteNonQuery();
                }

                DialogResult = true;  
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CleanButton_Click(object sender, RoutedEventArgs e)
        {
            txtIma.Text = string.Empty;
            txtsurname.Text = string.Empty;
            txtpatronymic.Text = string.Empty;
            txtemail.Text = string.Empty;

            myComboBox.SelectedIndex = -1;
        }

        private bool IsValidName(string name)
        {
          
            if (name.Length > 24)
                return false;
 
            return Regex.IsMatch(name, @"^[a-zA-Zа-яА-Я\s]+$");
        }
        private bool IsValidSurname(string name)
        {
           
            if (name.Length > 34)
                return false;
             
            return Regex.IsMatch(name, @"^[a-zA-Zа-яА-Я\s]+$");
        }
        private bool IsValidPatronymic(string name)
        { 
            if (name.Length > 30)
                return false;
 
            return Regex.IsMatch(name, @"^[a-zA-Zа-яА-Я\s]+$");
        }
        
    }
}