using MySql.Data.MySqlClient;
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
    /// Логика взаимодействия для CreateOrderWindow.xaml
    /// </summary>
    public partial class CreateOrderWindow : Window
    {
        public CreateOrderWindow()
        {
            InitializeComponent();
        }

        private void SaveOrderButton_Click(object sender, RoutedEventArgs e)
        {
            string OrdererName = OrdererNameTB.Text;
            string ModelType = ModelTypeTB.Text;
            string ModelName = ModelNameTB.Text;
            string FaultDesc = FaultDescTB.Text;
            string SerialNumber = SerialNumberTB.Text;

            MySqlConnection conn = new MySqlConnection(Variables.ConnSett.ConnectionString);



            try
            {
                conn.Open();

                string AddModelString = $"INSERT INTO repaired_models(rm_name, rm_type) VALUES (\"{ModelName}\", \"{ModelType}\");";

                MySqlCommand command = new MySqlCommand(AddModelString, conn);
                command.ExecuteNonQuery();

                command = new MySqlCommand($"SELECT rm_id FROM repaired_models WHERE rm_name LIKE \"{ModelName}\" AND rm_type LIKE \"{ModelType}\";",conn);

                var reader = command.ExecuteReader();

                reader.Read();
                string FaultModelId = reader.GetValue(0).ToString();

                conn.Close();
                conn.Open();

                string AddFaultTypeString = $"INSERT INTO fault_types(ft_description, ft_model_id) VALUES (\"{FaultDesc}\", \"{FaultModelId}\");";

                command = new MySqlCommand(AddFaultTypeString, conn);
                command.ExecuteNonQuery();


                conn.Close();
                conn.Open();


                command = new MySqlCommand($"SELECT ft_id FROM fault_types WHERE ft_description LIKE \"{FaultDesc}\" AND ft_model_id LIKE \"{FaultModelId}\";",conn);

                reader = command.ExecuteReader();

                reader.Read();
                string FaultTypeId = reader.GetValue(0).ToString();

                conn.Close();
                conn.Open();

                DateTime CurrentDateTime = DateTime.Now;
                string date = Variables.CheckForDate(CurrentDateTime.ToString());
                string AddOrderString = $"INSERT INTO orders(o_name, o_serial_number, o_order_date, o_ft_id) VALUES(\"{OrdererName}\", \"{SerialNumber}\", \"{date}\", \"{FaultTypeId}\");";
              

                command = new MySqlCommand(AddOrderString, conn);
                command.ExecuteNonQuery();
                conn.Close();
                Notification.ShowNotice("Заказ оформлен");
                Variables.ReturnToMenuFrom(this);
            }
            catch(Exception err)
            {
                Console.WriteLine(err.Message);
                Notification.ShowError(err.Message);
            }



        }

        private void CancelOrderButton_Click(object sender, RoutedEventArgs e)
        {
            Variables.MenuWindow_Window.Show();
            Hide();
        }
    }
}
