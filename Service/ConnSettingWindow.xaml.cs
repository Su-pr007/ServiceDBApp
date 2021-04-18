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

namespace Service
{
    /// <summary>
    /// Логика взаимодействия для ConnSettingWindow.xaml
    /// </summary>
    public partial class ConnSettingWindow : Window
    {
        DBConnectionSettings DBConnSettings = new DBConnectionSettings();


        public ConnSettingWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DBIpTextBox.Text = Variables.DBConnSettings.IP;
            DBPortTextBox.Text  = Variables.DBConnSettings.Port;
            DBNameTextBox.Text = Variables.DBConnSettings.Name;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Variables.DBConnSettings.IP = DBIpTextBox.Text;
            Variables.DBConnSettings.Port = DBPortTextBox.Text;
            Variables.DBConnSettings.Name = DBNameTextBox.Text;
            Variables.DBConnSettings.Save();

            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DBIpTextBox.Text = Variables.DBConnSettings.DefaultIP;
            DBPortTextBox.Text = Variables.DBConnSettings.DefaultPort;
            DBNameTextBox.Text = Variables.DBConnSettings.DefaultName;
        }

        private void DBIpTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DBPortTextBox.SelectAll();
            }
        }

        private void DBPortTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DBNameTextBox.SelectAll();
            }
        }

        private void DBNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SaveButton_Click(new object(), new RoutedEventArgs());
            }
        }


        private void ConnectionSettingsWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) CancelButton_Click(new object(), new RoutedEventArgs());
            if (e.Key == Key.F1) Variables.ShowHelp();
        }
    }
}
