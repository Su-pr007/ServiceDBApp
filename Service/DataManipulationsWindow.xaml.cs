using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Service.Properties
{
	/// <summary>
	/// Логика взаимодействия для DataManipulationsWindow.xaml
	/// </summary>
	public partial class DataManipulationsWindow : Window
	{

		public DataGrid SelectedDataGrid = null;
		List<string> Columns = new List<string>();
		DataTable DMDataSource = null;
		object EntityId = null;

		bool IsChange;

		// Добавление / изменение данных
		public DataManipulationsWindow(DataGrid SelectedDataGrid, bool IsChange)
		{
			Owner = Variables.TablesWindow_Window;
			InitializeComponent();

            if (IsChange)
            {
				this.IsChange = IsChange;
				Title = "Изменение строки";
            }
            else
            {
				this.IsChange = false;
				Title = "Добавление строки";
			}

			this.SelectedDataGrid = SelectedDataGrid;
		}

		private void DataManipulationsWindow1_Loaded(object sender, RoutedEventArgs e)
		{
            for (int i = 0; i < SelectedDataGrid.Columns.Count; i++)
            {
				Columns.Add(SelectedDataGrid.Columns.ElementAt(i).Header.ToString());
            }

			

			List<object> xsw = new List<object>();
			for (int i = 0; i < Columns.Count; i++)
			{
				if (IsChange)
                {
                    try
                    {
						var DV1 = Variables.FindMyDGByName(SelectedDataGrid.Name).DV;
						var DV2 = DV1.Table.Rows[SelectedDataGrid.SelectedIndex][i];
						xsw.Add(DV2);
						/*xsw.Add(Variables.FindMyDGByName(SelectedDataGrid.Name).DV.Table.Rows[SelectedDataGrid.SelectedIndex][i]);*/

					}
                    catch
                    {
						Notification.ShowError("Нет прав");
						Hide();
                    }
                }
                else xsw.Add("");
			}

			DMDataSource = CreateDMDataTable(xsw);
			DMDataGrid.ItemsSource = DMDataSource.DefaultView;
			DMDataGrid.CanUserAddRows = false;
			DMDataGrid.CanUserDeleteRows = false;
		}
		// Вставить только значения. Названия полей метод берёт из переменной Columns
		public DataTable CreateDMDataTable(List<object> Values)
        {

			DataTable NewTable = new DataTable();

			// Создание структуры таблицы
			for(int i = 0; i < 2; i++)
            {

				DataColumn NewColumn = new DataColumn()
				{
					ColumnName = i == 0 ? "Поле" : "Значение",
					ReadOnly = i == 0,
				};
				NewTable.Columns.Add(NewColumn);
            }

			// Заполнение таблицы
			for (int i = 0; i < Columns.Count; i++)
			{
				DataRow DataTableRow = NewTable.NewRow();
                if (i == 0)
                {
					EntityId = Values[i];
					continue;
                }
				DataTableRow.ItemArray = new object[] { Columns[i], Values[i] };

				NewTable.Rows.Add(DataTableRow);
			}

			return NewTable;

		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			List<string> Values = new List<string>();
			bool IsSaved = false;

			Values.Add((string)EntityId);

			for (int i = 0; i < DMDataGrid.Items.Count; i++)
			{
				Values.Add(DMDataSource.Rows[i].ItemArray[1].ToString());
			}

			MySqlConnection conn = DBUtils.GetDBConnection(Variables.DBlogin, Variables.DBpassword);
			try
			{
				Console.WriteLine("Openning of connection...");
				conn.Open();
				Console.WriteLine("Connection successfull!");
				string sql;

				if (IsChange)
                {
					sql = "UPDATE " + SelectedDataGrid.Name + " SET ";
                }
                else
                {
					sql = "INSERT INTO " + SelectedDataGrid.Name + "(";
				}

				for (int i = 0; i < Columns.Count; i++)
				{
					if (!IsChange && i == 0) continue;
					sql += "`" + Variables.FindMyDGByName(SelectedDataGrid.Name).ColumnsEng[i] + "`";
					if (IsChange)
					{
						if (Values[i] == "") sql += " = null";
						else sql += " = '" + Variables.CheckForDate(Values[i]) + "'";
                    }
					sql += i == Values.Count - 1 ? " " : ", ";
				}
				if (IsChange)
				{
					sql += "WHERE `" + Variables.FindMyDGByName(SelectedDataGrid.Name).PK + "` LIKE \"" + Values[0] + "\";";
				}
                else {
					sql += ") VALUES (";
					for(int i = 1; i < Values.Count; i++)
                    {
						if(Values[i]=="") sql+="null";
						else sql += "'" + Variables.CheckForDate(Values[i]) + "'";
						sql += i == Values.Count - 1 ? " " : ", ";
					}
					sql += ");";
                }

				MySqlCommand command = new MySqlCommand(sql, conn);
				command.ExecuteScalar();
				IsSaved = true;
			}
			catch(Exception err)
			{
                Console.WriteLine("Connection error!");
                Console.WriteLine(err.Message);
				string NotificationMessage = "Не получен ответ от базы данных";
				if (new Regex("command denied").IsMatch(err.Message))
                {
					NotificationMessage = "Нет прав на ";
					if (new Regex("UPDATE command").IsMatch(err.Message)) NotificationMessage += "изменение.";
					else NotificationMessage += "добавление.";
                }
				Notification.ShowError(NotificationMessage);
			}
			finally
			{
				conn.Close();
                Console.WriteLine("Connection closed");
                if (IsSaved)
                {
					Close();
                }
			}

		}
		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

        private void DataManipulationsWindow1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
			Variables.TablesWindow_Window.ReloadTables();
		}
	}
}
