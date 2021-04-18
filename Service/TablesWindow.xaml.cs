using MySql.Data.MySqlClient;
using Service.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;
using static Service.Variables;

namespace Service
{
	/// <summary>
	/// Логика взаимодействия для Window1.xaml
	/// </summary>
	public partial class TablesWindow : Window
	{
		public static int SelectedTabIndex;
		public static DataGrid SelectedDataGrid;


		public TablesWindow()
		{
			InitializeComponent();

		}

		private void Window_Closed(object sender, EventArgs e)
		{
			ApplicationStop();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			string[] TabHeaders = new string[] { };
			switch (ProfileId)
			{
				// ServiceManager - Менеджер
				case 1:
					TabHeaders = new string[] { "Заказы", "Сотрудники" };
					break;
				// ServiceRepairer - Ремонтник
				case 2:
					TabHeaders = new string[] { "Заказы", "Запчасти", "Ремонтируемые модели" };
					break;
				// ServiceAccountant - Бухгалтер
				case 3:
					break;
				// ServicePersDepart - Отдел кадров
				case 4:
					TabHeaders = new string[] { "Сотрудники" };
					break;
				// ServiceDBAdmin - Администратор БД
				case 5:
					TabHeaders = new string[] { "*" };
					break;
				// ServiceOrderer - Заказчик
				case 6:
					TabHeaders = new string[] { "Заказы" };
					break;
			}
			Name = "TablesWindow";
			TabItems.Clear(this);
			List<TabItem> CreatedTabItemsList = TabItems.Create(this, TabHeaders);
			foreach(var i in CreatedTabItemsList)
            {
				MainTabs.Items.Add(i);
            }

			ReloadTables();
		}

		// Заполнение таблиц
		public void ReloadTables()
		{
			foreach (TabItem CurrentTabItem in MainTabs.Items)
			{
				
				DataGrid CurrentDataGrid = FindMyDGByName(CurrentTabItem.Name).DG;
				if (CurrentDataGrid.Name.ToString() != null)
                {
					FillTable.ByDG(CurrentDataGrid);
                }
                else
                {
					Console.WriteLine("Неизвестная таблица");
                }
			}
		}

		// Кнопка возврата в меню
		private void ReturnButton_Click(object sender, RoutedEventArgs e)
		{
			MyDGs.Clear();
			ReturnToMenuFrom(this);
		}

		// Кнопка обновления таблиц
		private void ReloadButton_Click(object sender, RoutedEventArgs e)
		{
			ReloadTables();
		}

		// Кнопка фильтрации строк
		private void FilterButton_Click(object sender, RoutedEventArgs e)
		{
			new FilterWindow(DataGrids.ElementAt(SelectedTabIndex), this).ShowDialog();
		}

		// Кнопка поиска по таблице
		private void SearchButton_Click(object sender, RoutedEventArgs e)
		{
			new SearchWindow(this).ShowDialog();
			
		}

		// Изменение выбранной вкладки
		private void TabControlChangedSelection(object sender, SelectionChangedEventArgs e)
		{
			SelectedTabIndex = MainTabs.SelectedIndex;
			CurrentDataGridName = DataGrids[SelectedTabIndex].Name;
		}

		// Кнопка удаления строки
		private void DeleteButton_Click(object sender, RoutedEventArgs e)
		{
			string res = "NO";
			int ItemsCount = DataGrids.ElementAt(SelectedTabIndex).SelectedItems.Count;

			if (ItemsCount == 1)
            {
				res = Notification.ShowAsk("Вы уверены что хотите удалить эту запись?").ToString();
            }
			else if(ItemsCount > 1)
			{
				res = Notification.ShowAsk("Вы уверены что хотите удалить эти записи?").ToString();
			}
            else
            {
				Notification.ShowNotice("Выберите удаляемые строки");
			}
			if (res.ToLower() == "yes")
			{
				MyDataGrid thisDG = FindMyDGByName(DataGrids[SelectedTabIndex].Name);

				DataGrid SelectedDataGrid = DataGrids.ElementAt(SelectedTabIndex);

				string sql = "DELETE FROM " + SelectedDataGrid.Name + " WHERE ";
				
				
				for(int i = 0; i < ItemsCount; i++)
                {
					sql += MyDGs[SelectedTabIndex].PK + " = \"" + thisDG.DV.Table.Rows[(SelectedDataGrid.SelectedIndex) + i][0]+ "\"";
                    sql += i == ItemsCount - 1? ";":" or ";
				}

				ExecuteSqlQueryNoResults(sql);
			}
		}

		// Кнопка изменения строки
		public void ChangeRowButton_Click(object sender, RoutedEventArgs e)
		{
			SelectedDataGrid = DataGrids.ElementAt(SelectedTabIndex);

			IList<DataGridCellInfo> SelectedCells = SelectedDataGrid.SelectedCells;

			if (SelectedCells.Count == SelectedDataGrid.Columns.Count)
			{
				new DataManipulationsWindow(SelectedDataGrid, true).Show();
			}
			else
			{
				Notification.ShowNotice("Выберите одну строку");
			}

			ReloadTables();
		}

		// Кнопка добавления строки
		private void AddRowButton_Click(object sender, RoutedEventArgs e)
		{
			new DataManipulationsWindow(DataGrids.ElementAt(SelectedTabIndex), false).ShowDialog();
			ReloadTables();
		}

		// Выполнение запроса без возврата результата
		public void ExecuteSqlQueryNoResults(string sql)
        {

			conn = DBUtils.GetDBConnection(DBlogin, DBpassword);
			MySqlCommand command = new MySqlCommand(sql, conn);
			try
			{
				Console.WriteLine("Openning connection...");
				conn.Open();
				Console.WriteLine("Connection successful!");


				Console.WriteLine("Trying to execute sql query...");
				command.ExecuteNonQuery();
                Console.WriteLine("Successfully!");

				ReloadTables();
			}
			catch (Exception err)
			{
				Console.WriteLine(err.Message);
				if (err.Message.Split(' ')[1]=="delete" && err.Message.Split(' ')[5] == "parent")
                {
					Regex regex = new Regex(@"\(`\w*?`\.`\w*?`,");
					var asdffa = regex.Match(err.Message);

					string ConstraintInTable = asdffa.Value.Trim(new char[] { '(', ',' });
					Notification.ShowError($"Эта строка является родительской для другой. Сначала удалите дочернюю строку.\nДочерняя строка в таблице  {ConstraintInTable}.");
				}
                else
                {
					string NotifMsg = "Ошибка при выполнении команды в базе данных.";
					if(new Regex("command denied").IsMatch(err.Message))
                    {
						NotifMsg = "Нет прав на удаление.";
                    }

					Notification.ShowError(NotifMsg);
                }
			}
			finally
			{
				conn.Close();
			}
		}

		// Обновление на F5
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
			switch (e.Key) {
				case Key.F5:
					ReloadTables();
					break;
				case Key.F1:
					ShowHelp();
					break;
			}
		}
    }
}
