using Npgsql;
using System;
using System.Windows;

namespace SaharovaPractica
{
    public partial class DobawTow : Window
    {
        private string connectionString = "Host=localhost;Database=TechnoPark;Username=postgres;Password=1234;Persist Security Info=True";

        public DobawTow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtproductname.Text) ||
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

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string getLastIdQuery = "SELECT idproduct FROM product ORDER BY idproduct DESC LIMIT 1;";
                    NpgsqlCommand getLastIdCommand = new NpgsqlCommand(getLastIdQuery, connection);
                    object lastId = getLastIdCommand.ExecuteScalar();
                    int newProductId = (lastId != null && lastId != DBNull.Value) ? Convert.ToInt32(lastId) + 1 : 1;

                    string insertQuery = "INSERT INTO product (idproduct, productname, description, price ) " +
                                         "VALUES (@idproduct, @productname, @description, @price )";
                    NpgsqlCommand insertCommand = new NpgsqlCommand(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@idproduct", newProductId);
                    insertCommand.Parameters.AddWithValue("@productname", txtproductname.Text);
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
            txtproductname.Text = string.Empty;
            txtdescription.Text = string.Empty;
            txtprice.Text = string.Empty;
        }
    }
}