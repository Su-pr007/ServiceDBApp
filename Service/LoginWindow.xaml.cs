using MySql.Data.MySqlClient;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Service
{
    /// <summary>
    /// Логика взаимодействия для TablesWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {


            TryOpenMenuWindow(LoginTextBox.Text, PasswordBox.Password);
        }

        public void TryOpenMenuWindow(string login, string password)
        {
            Variables.InitConnSett();

            Regex regex = new Regex(@"^Service");
            if (!regex.IsMatch(login) && login != "root")
            {
                login = "Service" + login;
                password = "Service" + password;
            }

            bool isConnected = false;
            int ProfileId = 0;
            // Подключение к бд
            MySqlConnection conn = DBUtils.GetDBConnection(login, password);

            // Попытка открытия соединения с базой
            try
            {
                Console.WriteLine("Openning of connection...");
                conn.Open();
                Console.WriteLine("Connection successfull!");
                ProfileId = EmployeePosId(login);
                if (ProfileId == 0)
                {
                    Notification.ShowError("Неверный логин или пароль.");
                }
                else
                {
                    Variables.InitVariables();
                    isConnected = true;
                }
                MySqlDataReader reader = new MySqlCommand("show tables;", conn).ExecuteReader();

                if (reader.HasRows)
                {
                    int i = 0;
                    while (reader.Read())
                    {
                        i++;
                    }
                    if (i != 8)
                    {
                        Notification.ShowError("Ошибка подключения. Возможно, указано неверное имя базы данных.");
                        isConnected = false;
                    }
                }
            }
            catch (Exception err)
            {
                string msg = err.Message;
                Console.WriteLine("Error: " + msg);
                string msgFW = msg.Split(' ').First();
                switch (msgFW)
                {
                    case "Authentication":
                        Notification.ShowError("Ошибка входа. \nПроверьте название сервера, логин и пароль", "Ошибка");
                        break;
                    default:
                        Notification.ShowError(msg, "Ошибка");
                        break;

                }


            }
            finally
            {
                conn.Close();
                Console.WriteLine("Connection closed");

                Variables.DBlogin = login;
                Variables.DBpassword = password;
            }
            if (isConnected)
            {
                Variables.ProfileId = ProfileId;
                Variables.MenuWindow_Window.Show();
                this.Hide();
            }
        }

        private static int EmployeePosId(string login)
        {
            int ProfileId;
            switch (login)
            {
                case "ServiceManager":
                    ProfileId = 1;
                    break;
                case "ServiceRepairer":
                    ProfileId = 2;
                    break;
                case "ServiceAccountant":
                    ProfileId = 3;
                    break;
                case "ServicePersDepart":
                    ProfileId = 4;
                    break;
                case "ServiceDBAdmin":
                    ProfileId = 5;
                    break;
                case "ServiceOrderer":
                    return 6;
                default:
                    Console.WriteLine("user not found");
                    ProfileId = 0;
                    break;
            }
            return ProfileId;
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TryOpenMenuWindow(LoginTextBox.Text, PasswordBox.Password);
            }
        }

        private void LoginWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Activate();
            LoginTextBox.Focus();
        }


        private void GuestButton_Click(object sender, RoutedEventArgs e)
        {
            TryOpenMenuWindow("ServiceOrderer", "12345");
        }

        private void LoginTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    PasswordBox.Focus();
                    break;
                case Key.F3:
                    GuestButton_Click(new object(), new RoutedEventArgs());
                    break;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Variables.ApplicationStop();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void ConnSettingWindowButton_Click(object sender, RoutedEventArgs e)
        {
            new ConnSettingWindow().ShowDialog();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1) Variables.ShowHelp();
        }
    }
}