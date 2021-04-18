using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

namespace Service
{
	/// <summary>
	/// Логика взаимодействия для FilterWindow.xaml
	/// </summary>
	/// 
	public partial class FilterWindow : Window
	{
		string ThisDGName;
		public DataTable FilterDataTable;
		string ThisWindowName;

		public FilterWindow(DataGrid ThisDG, Window window)
		{
			InitializeComponent();

			ThisWindowName = window.Name;
			ThisDGName = ThisDG.Name;
		}
		private void FilterWindow1_Loaded(object sender, RoutedEventArgs e)
		{
			FilterDataTable = null;
			AndCheckBox.IsChecked = Variables.AndFilterChecked;
			DataTable thisDV;
			if (ThisWindowName == "TablesWindow") thisDV = Variables.FindMyDGByName(ThisDGName).DV.Table;
			else thisDV = Variables.FindMyDGByName(ThisDGName).DV.Table;


			DataTable NewTable = new DataTable();
			for (int i = 0; i < 2; i++)
			{
				DataColumn NewColumn = new DataColumn()
				{
					ColumnName = i == 0 ? "Поле" : "Значение",
					ReadOnly = i == 0,
				};
				NewTable.Columns.Add(NewColumn);
			}
			for(int i = 0; i < thisDV.Columns.Count; i++)
			{
				DataRow NewRow = NewTable.NewRow();
				NewRow[0] = thisDV.Columns[i].ColumnName;
				NewRow[1] = "";
				NewTable.Rows.Add(NewRow);
			}

			FilterDataTable = NewTable;
			FilterDataGrid.ItemsSource = NewTable.DefaultView;
		}


		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			string msg = "Поиск завершён";
			Variables.AndFilterChecked = AndCheckBox.IsChecked.Value;

			DataTable DGTable = Variables.FindMyDGByName(Variables.CurrentDataGridName).DV.Table;

			DataTable FilteredDT = new DataTable();
			List<string> FilterDGData = new List<string>();


			for (int i = 0; i < FilterDataTable.Rows.Count; i++)
			{
				FilteredDT.Columns.Add(DGTable.Columns[i].ColumnName);
				FilterDGData.Add(FilterDataTable.Rows[i][1].ToString());
			}
			bool FoundRow = false;


			for (int i = 0; i < FilterDGData.Count; i++)
			{
				if (FilterDGData[i] != "")
				{
					FoundRow = true;
					break;
				}
			}
			if (FoundRow)
			{
				string sql = "";
				MyDataGrid thisDG = Variables.FindMyDGByName(Variables.CurrentDataGridName);


				switch (thisDG.Name)
				{
					case "PersonnelDepartment":
						sql = Variables.PersonnelDepartment;
						break;
					case "FaultsList":
						sql = Variables.FaultsList;
						break;
					case "OrdersList":
						sql = Variables.OrdersList;
						break;
					case "employees":
						sql = "SELECT * FROM employees;";
						break;
					case "fault_types":
						sql = "SELECT * FROM fault_types;";
						break;
					case "orders":
						sql = "SELECT * FROM orders;";
						break;
					case "parts":
						sql = "SELECT * FROM parts;";
						break;
					case "parts_faults":
						sql = "SELECT * FROM parts_faults;";
						break;
					case "positions":
						sql = "SELECT * FROM positions;";
						break;
					case "repaired_models":
						sql = "SELECT * FROM repaired_models;";
						break;
					case "served_shops":
						sql = "SELECT * FROM served_shops;";
						break;
				}

				sql = sql.Split(';')[0] + " WHERE ";
				for(int i = 0;i< FilterDGData.Count; i++)
                {
					if (FilterDGData[i] == "") continue;
                    
					sql += "`"+ Variables.DictionarySearchKey(Variables.ColumnsDictionary, thisDG.DV.Table.Columns[i].ColumnName)+"` LIKE \""+FilterDGData[i]+"\"";
                    
					if (AndCheckBox.IsChecked.Value)
					{
						sql += " AND ";
                    }
                    else
                    {
						sql += " OR ";
                    }
				}
				sql = new Regex(@"AND $").Replace(sql, ";");
				sql = new Regex(@"OR $").Replace(sql, ";");


				MySqlConnection conn = DBUtils.GetDBConnection(Variables.DBlogin, Variables.DBpassword);
				MySqlCommand cmnd = new MySqlCommand(sql, conn);


				conn.Open();
				var reader = cmnd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
						List<string> ValuesStrings = new List<string>();
						for (int i = 0; i < reader.FieldCount; i++)
						{
							ValuesStrings.Add(reader.GetValue(i).ToString());
						}
						DataRow NewRow = FilteredDT.NewRow();
						NewRow.ItemArray = ValuesStrings.ToArray();
						FilteredDT.Rows.Add(NewRow);
					}
                }
				conn.Close();




				Variables.FindMyDGByName(Variables.CurrentDataGridName).DV = FilteredDT.DefaultView;
				Variables.FindMyDGByName(Variables.CurrentDataGridName).DG.ItemsSource = FilteredDT.DefaultView;
			}
			else
			{
				Variables.TablesWindow_Window.ReloadTables();
			}
			Notification.ShowNotice(msg);


			Hide();
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			Hide();
		}

		private void FilterWindow1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape) Hide();
		}
	}
}
