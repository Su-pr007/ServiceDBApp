using GemBox.Spreadsheet;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
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
    /// Логика взаимодействия для ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        public static int SelectedTabIndex;
        public static bool isLoaded = false;

        public ReportWindow()
        {
            SelectedTabIndex = 0;
            InitializeComponent();
        }

        private void ReportWindow_Loaded(object sender, RoutedEventArgs e)
        {
            isLoaded = false;
            string[] TabHeaders = new string[] { };
            switch (Variables.ProfileId)
            {
                // ServiceManager - Менеджер
                case 1:
                    TabHeaders = new string[] { "Список заказов" };
                    break;
                // ServiceRepairer - Ремонтник
                case 2:
                    TabHeaders = new string[] { "Список неисправностей", "Список заказов" };
                    break;
                // ServiceAccountant - Бухгалтер
                case 3:
                    TabHeaders = new string[] { "Список заказов" };
                    break;
                // ServicePersDepart - Отдел кадров
                case 4:
                    TabHeaders = new string[] { "Отдел кадров" };
                    break;
                // ServiceDBAdmin - Администратор БД
                case 5:
                    TabHeaders = new string[] { "*" };
                    break;
                // ServiceOrderer - Заказчик
                case 6:
                    TabHeaders = new string[] { };
                    break;
            }

            Name = "ReportWindow";
            ReportTabs.Items.Clear();

            List<TabItem> CreatedTabItems = TabItems.Create(this, TabHeaders);
            foreach (var i in CreatedTabItems)
            {
                ReportTabs.Items.Add(i);
            }

            ReloadTables();
            isLoaded = true;

            ReportTabs.SelectedIndex = 0;
        }

        private void ReportWindow_Closed(object sender, EventArgs e)
        {
            Variables.ApplicationStop();
        }

        private void ReloadTables()
        {
            Variables.ReportTables.Clear();

            for (int i = 0; i < ReportTabs.Items.Count; i++)
            {
                string sql;
                switch (Variables.ReportGrids[i].Name)
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
                    default:
                        sql = "SHOW TABLES;";
                        break;
                }
                FillTable.ByDG(sql, Variables.ReportGrids[i]);
            }
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            isLoaded = false;
            Variables.ReturnToMenuFrom(this);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Variables.CurrentDataGridName = Variables.ReportTables.ElementAt(ReportTabs.SelectedIndex).DG.Name;
            var ToExcel = Variables.FindMyDGByName(Variables.CurrentDataGridName).DV.Table;
            ToExcel.TableName = Variables.CurrentDataGridName;
            Excel(ToExcel);
            
        }


        public static void Excel(DataTable Table)
        {
            Table = Variables.TranslateColumns(Table);

            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

            var workbook = new ExcelFile();
            var sheet = workbook.Worksheets.Add("Sheet 1");




            // Заполнение строк
            for (int i = 0; i < Table.Columns.Count; i++)
            {
                for(int j = 0;j< Table.Rows.Count; j++)
                {
                    sheet.Rows[j + 1].Cells[i].Style.Borders.SetBorders(MultipleBorders.All, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
                    sheet.Rows[j+1].Cells[i].Value = Table.Rows[j][i];
                }
            }

            // Создание колонок
            for (int i = 0; i < Table.Columns.Count; i++)
            {
                sheet.Rows[0].Cells[i].Style.Borders.SetBorders(MultipleBorders.All, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
                sheet.Rows[0].Cells[i].Style.Borders.SetBorders(MultipleBorders.Bottom, SpreadsheetColor.FromArgb(0,0,0), LineStyle.Medium);
                sheet.Rows[0].Cells[i].Style.FillPattern.SetSolid(SpreadsheetColor.FromArgb(230,230,230));

                sheet.Rows[0].Cells[i].Value = Table.Columns[i].ColumnName;
                sheet.Columns[i].AutoFit();
            }

            try
            {
                string ExcelFileName = "TestExcel.xlsx";

                ExcelFileName = Table.TableName+".xlsx";
                string ExcelFilePath = "saves\\";
                if (!Directory.Exists(ExcelFilePath))
                {
                    Directory.CreateDirectory(ExcelFilePath);
                }
                workbook.Save(ExcelFilePath+"\\"+ExcelFileName);
                if (File.Exists(ExcelFilePath + "\\"+ExcelFileName))
                {
                    if(Notification.ShowAsk("Сохранение завершено. Открыть файл?").ToString().ToLower() == "yes")
                    {
                        Process.Start(ExcelFilePath + "\\"+ExcelFileName);
                    }
                }
            }
            catch(Exception err)
            {
                Console.WriteLine(err.Message);
                Notification.ShowError(err.Message);
            }
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            InitTabName();
            new FilterWindow(Variables.ReportGrids.ElementAt(SelectedTabIndex), this).ShowDialog();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            InitTabName();
            new SearchWindow(this).ShowDialog();
        }

        private void TabControlSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InitTabName();
        }

        // Индекс выбранной вкладки
        public void InitTabName()
        {
            SelectedTabIndex = ReportTabs.SelectedIndex;
            string TableName = "";
            var dadad = ReportTabs.Items[0].ToString().Split(':');
            switch (dadad[dadad.Count()-2])
            {
                case "Отдел кадров Content":
                    TableName = "PersonnelDepartment";
                    break;
                case "Список неисправностей Content":
                    TableName = "FaultsList";
                    break;
                case "Список заказов Content":
                    TableName = "OrdersList";
                    break;
            }
            if (isLoaded)
            {   
                Variables.CurrentDataGridName = Variables.FindMyDGByName(TableName).Name;
            }
        }

        private void ReportWindow1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F1:
                    Variables.ShowHelp();
                    break;
                case Key.F5:
                    ReloadTables();
                    break;
            }
        }

        private void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            ReloadTables();
        }
    }
}
