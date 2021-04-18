using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;
using static Service.Variables;

namespace Service
{
    class FillTable
    {

        // sql - Текст MySQL запроса
        // DataGridToFill - Имя заполняемой таблицы
        public static void ByDG(string sql, DataGrid DataGridToFill)
        {

            DataGridToFill.ItemsSource = null;

            DataTable NewDataTable = new DataTable();

            conn = new MySqlConnection(ConnSett.ConnectionString);
            conn.Open();

            MySqlCommand command = new MySqlCommand(sql, conn);
            var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                var ColumnsEng = new List<string>() { };

                var SchemaTable = reader.GetSchemaTable();

                for(int i = 0; i < reader.FieldCount; i++)
                {
                    ColumnsEng.Add(SchemaTable.Rows[i].ItemArray[0].ToString());
                    NewDataTable.Columns.Add(DictionarySearch(ColumnsDictionary, ColumnsEng.Last()));

                }
                while (reader.Read())
                {
                    DataRow NewRow = NewDataTable.NewRow();
                    var Row = new List<string>() { };
                    for (int i=0;i< reader.FieldCount;i++)
                    {
                        try
                        {
                            if (reader.IsDBNullAsync(i).Result)
                            {
                                Row.Add("");
                            }
                            else
                            {
                                Row.Add(reader.GetString(ColumnsEng[i]));

                            }
                        }
                        catch
                        {
                            Row.Add("");
                        }
                        

                    }
                    NewRow.ItemArray = Row.ToArray();
                    NewDataTable.Rows.Add(NewRow);
                }

            }

            conn.Close();

                MyDataGrid thisTable = new MyDataGrid()
                {
                    DV = NewDataTable.DefaultView,
                    DG = DataGridToFill,
                    Name = DataGridToFill.Name,
                };
                thisTable.DV.Table.TableName = DataGridToFill.Name;
                ReportTables.Add(thisTable);

            DataGridToFill.ItemsSource = NewDataTable.DefaultView;
        }
        public static void ByDG(DataGrid DataGridToFill)
        {

            DataGridToFill.ItemsSource = null;



            DataTableCollection Tables = ServiceDB.Tables;

            if (Tables.Contains(DataGridToFill.Name))
            {
                DataView TableView;


                TableView = GetTableDataByTableName(DataGridToFill.Name);
                var MyDGName = FindMyDGByName(DataGridToFill.Name);
                MyDGName.Name = DataGridToFill.Name;

                if (TableView != null)
                {
                    TableView.Table.TableName = DataGridToFill.Name;
                    MyDGName.DV = TableView;
                    DataGridToFill.ItemsSource = TableView;
                } 

            }
            else
            {
                Console.WriteLine("Неопознанная таблица");
            }



        }



    }
}
