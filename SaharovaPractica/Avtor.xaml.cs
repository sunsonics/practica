using Npgsql;
using SaharovaPractica.Model;
using SaharovaPractica.Pages;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SaharovaPractica
{
    /// <summary>
    /// Логика взаимодействия для Avtor.xaml
    /// </summary>
    public partial class Avtor : Page
    {
        public Avtor()
        {
            InitializeComponent();
            
        }
        private void SuccessfulLogin(string roleName)
        {
            MessageBox.Show("Вы вошли под: " + roleName);
            LoadForm(roleName, null);
            
        }



        private void LoadForm(string roleName, Client client)
        {
             
            switch (roleName)
            {
                case "администратор":
                    NavigationService.Navigate(new Admin());
                    break;
                case "клиент":
                    NavigationService.Navigate(new Client(client));
                    break;
                case "механик":
                    NavigationService.Navigate(new Mehanic(client));
                    break;
                case "менеджер по продажам":
                    NavigationService.Navigate(new Menedjer(client));
                    break;
                case "складской работник":
                    NavigationService.Navigate(new RabotnicSclada(client));
                    break;
                default:
                    MessageBox.Show("Роль не распознана");
                    break;
            }
        }


        private void Whod_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginP.Text.Trim();
            string password = PasswordP.Text.Trim();

            using (var bd = new TechnoParkEntities())
            {
               
                string sqlQuery = @"SELECT c.login, c.password, r.rolename
                    FROM client c
                    INNER JOIN role r ON c.idroles = r.idrole
                    WHERE c.login = @login AND c.password = @password";

                
                var parameters = new[]
                {
            new NpgsqlParameter("login", login),
            new NpgsqlParameter("password", password)
        };

               
                var result = bd.Database.SqlQuery<LoginRoleResult>(sqlQuery, parameters).FirstOrDefault();

                if (result != null)
                {
                    SuccessfulLogin(result.RoleName);  
                }
                else
                {
                    MessageBox.Show("Пользователь с указанным логином и паролем не найден.");
                }
            }
        }
        public class LoginRoleResult
        {
            public string Login { get; set; }
            public string Password { get; set; }
            public string RoleName { get; set; }
        }
    }
}
    

