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
    /// Логика взаимодействия для SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {

        DataTable SelectedDataGrid;
        DataTable arr;
        MyDataGrid thisDG;


        public SearchWindow(Window thisWindow)
        {
            InitializeComponent();
            arr = new DataTable();
        }


        private void SearchWindow_Loaded(object sender, RoutedEventArgs e)
        {
            thisDG = Variables.FindMyDGByName(Variables.CurrentDataGridName);

            SearchTextBox.Focus();
            SelectedDataGrid = thisDG.DV.Table;

            for (int i = 0; i < SelectedDataGrid.Columns.Count; i++) arr.Columns.Add(new DataColumn() { ColumnName = SelectedDataGrid.Columns[i].ColumnName });

            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            arr.Rows.Clear();
            string str = SearchTextBox.Text;
            Regex regex = new Regex(@"" + str);




            for (int i = 0; i < SelectedDataGrid.Rows.Count; i++)
            {
                for(int j = 0; j < SelectedDataGrid.Columns.Count; j++)
                {
                    string CurCell = SelectedDataGrid.Rows[i][j].ToString();
                    if (regex.IsMatch(CurCell))
                    {
                        DataRow thisRow = arr.NewRow();
                        thisRow.ItemArray = SelectedDataGrid.Rows[i].ItemArray;


                        arr.Rows.Add(thisRow);

                        break;
                    }
                }
            }

            thisDG.DG.ItemsSource = arr.DefaultView;
            thisDG.DV = arr.DefaultView;

            SearchTextBox.Clear();
            Hide();
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Button_Click(new object(), new RoutedEventArgs());
        }

        private void SearchWindow1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Escape) Hide();
        }
    }
}
