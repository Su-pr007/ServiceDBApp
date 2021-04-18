using System;
using System.Linq;
using System.Windows;
using static Service.Variables;

namespace Service
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Скрыть все кнопки
            ToTablesButton.Visibility = Visibility.Collapsed;
            ToQueriesButton.Visibility = Visibility.Visible;
            CreateOrderButton.Visibility = Visibility.Collapsed;
            // Показать каждому свои
            switch (ProfileId)
            {
                // Менеджер
                case 1:
                    ToTablesButton.Visibility = Visibility.Visible;
                    CreateOrderButton.Visibility = Visibility.Visible;
                    break;
                // Ремонтник
                case 2:
                    ToTablesButton.Visibility = Visibility.Visible;
                    break;
                //  Бухгалтер
                case 3:
                    break;
                //  Отдел кадров
                case 4:
                    ToTablesButton.Visibility = Visibility.Visible;
                    break;
                //  Админ базы данных
                case 5:
                    ToTablesButton.Visibility = Visibility.Visible;
                    CreateOrderButton.Visibility = Visibility.Visible;
                    break;
                //  Заказчик
                case 6:
                    CreateOrderButton.Visibility = Visibility.Visible;
                    ToQueriesButton.Visibility = Visibility.Collapsed;
                    break;
            }
            if (ProfileId >= 6)
            {
                LoginTextBlock.Text = "Клиент";
            }
            else
            {
                try
                {
                    LoginTextBlock.Text = new serviceDataSetTableAdapters.positionsTableAdapter().GetData().ElementAt(ProfileId - 1).p_name;
                Console.WriteLine(1);
                }
                catch (Exception err)
                {
                    Console.WriteLine(err.Message);
                    Notification.ShowError(err.Message);
                }
            }

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            ApplicationStop();
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            LoginWindow_Window.Show();
            ClearVariables();
        }

        private void ToTablesButton_Click(object sender, RoutedEventArgs e)
        {
            TablesWindow_Window.Show();
            this.Hide();
        }

        private void ToQueriesButton_Click(object sender, RoutedEventArgs e)
        {
            ReportWindow_Window.Show();
            this.Hide();
        }



        private void CreateOrderButton_Click(object sender, RoutedEventArgs e)
        {
            new CreateOrderWindow().Show();
            Hide();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.F1) ShowHelp();
        }
    }
}
