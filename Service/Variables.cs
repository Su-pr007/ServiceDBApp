using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Service
{

    class MyDataGrid
    {

        public string Name;

        public DataView DV;

        public DataGrid DG;

        // Список названий колонок на английском
        public List<string> ColumnsEng;

        // Primary Key
        public string PK;

    }

    class Variables
    {
        public static DBConnectionSettings DBConnSettings = new DBConnectionSettings();
        public static ConnectionStringSettings ConnSett;


        public static string DBlogin;
        public static string DBpassword;

        public static string DBConnIP = DBConnSettings.DefaultIP;
        public static string DBConnName = DBConnSettings.DefaultName;
        public static string DBConnPort = DBConnSettings.DefaultPort;

        public static serviceDataSet ServiceDB = new serviceDataSet();

        public static int ProfileId;

        public static MySqlConnection conn;

        public static TablesWindow TablesWindow_Window;
        public static ReportWindow ReportWindow_Window;
        public static LoginWindow LoginWindow_Window;
        public static MenuWindow MenuWindow_Window;

        public static bool AndFilterChecked;
        public static bool IsAddedToReportTables;

        public static List<DataGrid> DataGrids;
        public static List<DataGrid> ReportGrids;
        public static List<MyDataGrid> MyDGs;
        public static List<MyDataGrid> ReportTables;
        public static string CurrentDataGridName;
        public static DateTime DateTime1 = new DateTime();

        public static bool IsFirst = true;


        public static Dictionary<string, string> ColumnsDictionary = new Dictionary<string, string>()
        { };


        // Запросы
        public static string PersonnelDepartment = "SELECT e_id, e_surname, e_name_patronymic, e_age, e_sex, e_passport_series_and_number, p_name FROM employees INNER JOIN positions ON employees.e_p_id = positions.p_id;";

        public static string FaultsList = "SELECT fault_types.ft_description, fault_types.ft_symptomes, fault_types.ft_repair_methods, fault_types.ft_price, parts.part_name, repaired_models.rm_name, repaired_models.rm_type FROM fault_types INNER JOIN repaired_models ON fault_types.ft_model_id = repaired_models.rm_id INNER JOIN parts_faults ON fault_types.ft_id = parts_faults.pf_fault_id INNER JOIN parts ON parts_faults.pf_part_id = parts.part_id;";

        public static string OrdersList = "SELECT orders.o_name, orders.o_order_date, orders.o_pick_date, orders.o_serial_number, orders.o_guarantee, orders.o_guarantee_period, orders.o_price, fault_types.ft_description, served_shops.ss_name FROM orders INNER JOIN served_shops ON served_shops.ss_id = orders.o_ss_id INNER JOIN fault_types ON fault_types.ft_id = orders.o_ft_id;";








        public static void InitConnSett()
        {
            ConnSett = new ConnectionStringSettings();

            DBConnIP = DBConnSettings.IP;
            DBConnPort = DBConnSettings.Port;
            DBConnName = DBConnSettings.Name;


            ConnSett.ConnectionString = $"server={DBConnIP};user id=root;;persistsecurityinfo=True;port={DBConnPort};database={DBConnName};allowuservariables=True";

        }



        public static void InitVariables()
        {
            conn = DBUtils.GetDBConnection(DBlogin, DBpassword);

            TablesWindow_Window = new TablesWindow();
            ReportWindow_Window = new ReportWindow();
            LoginWindow_Window = new LoginWindow();
            MenuWindow_Window = new MenuWindow();

            DataGrids = new List<DataGrid>();
            ReportGrids = new List<DataGrid>();
            MyDGs = new List<MyDataGrid>();
            ReportTables = new List<MyDataGrid>();
            CurrentDataGridName = "";
            CurrentDataGridName = "";

            AndFilterChecked = false;
            IsAddedToReportTables = false;

            ColumnsDictionary = new Dictionary<string, string> { };

            // employees
            ColumnsDictionary.Add("e_id", "Код сотрудника");
            ColumnsDictionary.Add("e_surname", "Фамилия");
            ColumnsDictionary.Add("e_name_patronymic", "Имя и Отчество");
            ColumnsDictionary.Add("e_age", "Возраст");
            ColumnsDictionary.Add("e_sex", "Пол");
            ColumnsDictionary.Add("e_address", "Адрес");
            ColumnsDictionary.Add("e_phone", "Номер телефона");
            ColumnsDictionary.Add("e_passport_series_and_number", "Серия и номер паспорта");
            ColumnsDictionary.Add("e_passport_issued_by", "Кем выдан паспорт");
            ColumnsDictionary.Add("e_passport_date_of_issue", "Дата выдачи паспорта");
            ColumnsDictionary.Add("e_p_id", "Код должности");

            // fault_types
            ColumnsDictionary.Add("ft_id", "ID");
            ColumnsDictionary.Add("ft_model_id", "Id модели");
            ColumnsDictionary.Add("ft_description", "Описание");
            ColumnsDictionary.Add("ft_symptomes", "Симптомы");
            ColumnsDictionary.Add("ft_repair_methods", "Методы ремонта");
            ColumnsDictionary.Add("ft_price", "Стоимость работы");

            // orders
            ColumnsDictionary.Add("o_id", "ID заказа");
            ColumnsDictionary.Add("o_order_date", "Дата заказа");
            ColumnsDictionary.Add("o_pick_date", "Дата возврата");
            ColumnsDictionary.Add("o_name", "ФИО заказчика");
            ColumnsDictionary.Add("o_serial_number", "Серийный номер");
            ColumnsDictionary.Add("o_ft_id", "Код вида неисправности");
            ColumnsDictionary.Add("o_ss_id", "Код магазина");
            ColumnsDictionary.Add("o_guarantee", "Отметка о гарантии");
            ColumnsDictionary.Add("o_guarantee_period", "Срок гарантии (месяцев)");
            ColumnsDictionary.Add("o_price", "Итоговая цена заказа");
            ColumnsDictionary.Add("o_e_id", "Код сотрудника");

            // parts
            ColumnsDictionary.Add("part_id", "Код запчасти");
            ColumnsDictionary.Add("part_name", "Наименование запчасти");
            ColumnsDictionary.Add("part_functions", "Функции");
            ColumnsDictionary.Add("part_price", "Цена запчасти");

            // parts_faults
            ColumnsDictionary.Add("pf_id", "ID");
            ColumnsDictionary.Add("pf_fault_id", "Код вида неисправности");
            ColumnsDictionary.Add("pf_part_id", "Код запчасти");

            // positions
            ColumnsDictionary.Add("p_id", "Код должности");
            ColumnsDictionary.Add("p_name", "Наименование должности");
            ColumnsDictionary.Add("p_salary", "Оклад");
            ColumnsDictionary.Add("p_duties", "Обязанности");
            ColumnsDictionary.Add("p_requirements", "Требования");

            // repaired_models
            ColumnsDictionary.Add("rm_id", "Код модели");
            ColumnsDictionary.Add("rm_name", "Наименование");
            ColumnsDictionary.Add("rm_type", "Тип");
            ColumnsDictionary.Add("rm_performance", "Производительность");
            ColumnsDictionary.Add("rm_tech_param", "Технические характеристики");
            ColumnsDictionary.Add("rm_specials", "Особенности");

            // served_shops
            ColumnsDictionary.Add("ss_id", "Код магазина");
            ColumnsDictionary.Add("ss_name", "Наименование магазина");
            ColumnsDictionary.Add("ss_address", "Адрес магазина");
            ColumnsDictionary.Add("ss_phone_number", "Номер телефона магазина");
        }



        public static void ClearVariables()
        {

            DBConnIP = DBConnSettings.DefaultIP;
            DBConnName = DBConnSettings.DefaultName;
            DBConnPort = DBConnSettings.DefaultPort;


            DBlogin = "";
            DBpassword = "";
            ProfileId = 0;
            CurrentDataGridName = "";
            CurrentDataGridName = "";

            TablesWindow_Window = null;
            ReportWindow_Window = null;
            LoginWindow_Window = null;
            MenuWindow_Window = null;


            DataGrids = null;
            ReportGrids = null;
            MyDGs = null;
            ReportTables = null;

            ColumnsDictionary = null;
        }


        public static MyDataGrid FindMyDGByName(string TableName)
        {
            var AllGrids = MyDGs.Concat(ReportTables);
            foreach (MyDataGrid curTable in AllGrids)
            {
                if (curTable.DG != null)
                {
                    if (curTable.DG.Name == TableName)
                    {
                        return curTable;
                    }
                }
            }
            return null;
        }



        public static DataView GetTableDataByTableName(string TableName)
        {
            DataView TableData;
            try
            {
                TableData = CreateDataView(TableName);

                DataTable RusColumnsTable = new DataTable();
                RusColumnsTable = TranslateColumns(TableData.Table);
                RusColumnsTable.TableName = TableName;

                return RusColumnsTable.DefaultView;
            }
            catch (Exception err)
            {
                Console.WriteLine("Error: " + err.Message);
                Notification.ShowError("Не удалось получить данные из базы");
            }
            return null;

        }


        public static DataView CreateDataView(string DataGridName)
        {
            DataTable NewTable = new DataTable();
            string sql = "SHOW TABLES;";
            switch (DataGridName)
            {
                case "PersonnelDepartment":
                    sql = PersonnelDepartment;
                    break;
                case "FaultsList":
                    sql = FaultsList;
                    break;
                case "OrdersList":
                    sql = OrdersList;
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


            MySqlConnection conn = new MySqlConnection(ConnSett.ConnectionString);
            MySqlCommand SqlCommand = new MySqlCommand(sql, conn);

            conn.Open();
            MySqlDataReader reader = SqlCommand.ExecuteReader();

            NewTable.TableName = DataGridName;
            if (reader.HasRows)
            {
                reader.Read();

                DataTable Schema = reader.GetSchemaTable();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    NewTable.Columns.Add(Schema.Rows[i].ItemArray[0].ToString());
                }
                List<string> NewString = new List<string>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var ThisCell = reader.GetValue(i);
                    NewString.Add(ThisCell.ToString());
                }
                NewTable.Rows.Add(NewString.ToArray());

                while (reader.Read())
                {
                    NewString = new List<string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        NewString.Add(reader.GetValue(i).ToString());
                    }
                    NewTable.Rows.Add(NewString.ToArray());
                }
            }
            conn.Close();


            return NewTable.DefaultView;

        }

        public static DataTable TranslateColumns(DataTable TableFrom)
        {
            DataTable TableTo = new DataTable();
            MyDataGrid thisDG = FindMyDGByName(TableFrom.TableName);
            if (thisDG == null)
            {
                Notification.ShowError("");
                return null;
            }
            thisDG.ColumnsEng = new List<string>();

            foreach (DataColumn i in TableFrom.Columns)
            {
                TableTo.Columns.Add(DictionarySearch(ColumnsDictionary, i.ColumnName));
                thisDG.ColumnsEng.Add(i.ColumnName);
            }
            foreach (DataRow i in TableFrom.Rows)
            {
                TableTo.Rows.Add(i.ItemArray);
            }
            TableTo.TableName = TableFrom.TableName;
            return TableTo;
        }
        public static string DictionarySearch(Dictionary<string, string> DictionaryFrom, string key)
        {
            string toReturn = "error";
            for (int i = 0; i < DictionaryFrom.Keys.Count; i++)
            {
                if (DictionaryFrom.Keys.ElementAt(i) == key || DictionaryFrom.Values.ElementAt(i) == key)
                {
                    toReturn = DictionaryFrom.Values.ElementAt(i);
                    break;
                }
            }
            return toReturn;
        }
        public static string DictionarySearchKey(Dictionary<string, string> DictionaryFrom, string key)
        {
            string toReturn = "error";
            for (int i = 0; i < DictionaryFrom.Keys.Count; i++)
            {
                if (DictionaryFrom.Keys.ElementAt(i) == key || DictionaryFrom.Values.ElementAt(i) == key)
                {
                    toReturn = DictionaryFrom.Keys.ElementAt(i);
                    break;
                }
            }
            return toReturn;
        }

        public static void ReturnToMenuFrom(Window WindowFrom)
        {
            MyDGs.Clear();
            MenuWindow_Window.Show();
            WindowFrom.Hide();
        }


        public static string CheckForDate(string StringFrom)
        {
            string StringTo = StringFrom;
            if (new Regex(@"^\d\d\.\d\d\.\d\d\d?\d? \d?\d:\d\d:\d\d$").IsMatch(StringFrom))
            {
                string[] DTimeStrings = StringFrom.Split(' ');
                string[] Date = DTimeStrings[0].Split('.');
                StringTo = Date[2] + "-" + Date[1] + "-" + Date[0] + " " + DTimeStrings[1];
            }
            else if (new Regex(@"^\d\d\.\d\d\.\d\d\d?\d?$").IsMatch(StringFrom))
            {
                string[] DTimeStrings = StringFrom.Split(' ');
                string[] Date = DTimeStrings[0].Split('.');
                StringTo = Date[2] + "-" + Date[1] + "-" + Date[0] + " 0:00:00";
            }
            return StringTo;
        }


        public static void ShowHelp()
        {
            if (IsFirst)
            {
                Process.Start(@"help.chm");
                IsFirst = false;
            }
            else
            {
                IsFirst = true;
            }
        }
        public static void ApplicationStop()
        {
            Process.GetCurrentProcess().Kill();
        }
    }
}
