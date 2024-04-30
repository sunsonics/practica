using SaharovaPractica.Model;
using SaharovaPractica.Pages;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Data.Entity;
using System.Data.SqlClient;
using Npgsql;


namespace SaharovaPractica
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FrmMain.Navigate(new Avtor());
        }

        private void FrmMain_ContentRendered(object sender, EventArgs e)
        {

        }
    }
}

    