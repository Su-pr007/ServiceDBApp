using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Service
{
    class TabItems
    {

        // Создать вкладки
        // ------------------
        // Window - Используется для передачи текущего окна. Использовать this
        // TabHeaders - Названия вкладок
        public static List<TabItem> Create(Window Window, string[] TabHeaders)
        {
            List<TabItem> Result = new List<TabItem>() { };
            string[] AllTabHeaders = new string[] { };
            string[] AllTabNames = new string[] { };
            if (Window.Name == "TablesWindow")
            {
                AllTabHeaders = new string[] { "Сотрудники", "Заказы", "Запчасти", "Должности", "Ремонтируемые модели", "Обслуживаемые магазины", "Виды неисправностей", };
                AllTabNames = new string[] { "employees", "orders", "parts", "positions", "repaired_models", "served_shops", "fault_types", };
            }
            else if (Window.Name == "ReportWindow")
            {
                AllTabHeaders = new string[] { "Отдел кадров", "Список неисправностей", "Список заказов" };
                AllTabNames = new string[] { "PersonnelDepartment", "FaultsList", "OrdersList" };
            }

            List<string> TabNames = new List<string>();


            if (TabHeaders.Length == 1 && TabHeaders.First() == "*")
            {
                TabNames = AllTabNames.ToList();
                TabHeaders = AllTabHeaders;
            }
            else
            {
                foreach (var i in TabHeaders)
                {
                    try
                    {
                        TabNames.Add(AllTabNames[AllTabHeaders.ToList().IndexOf(i)]);
                    }
                    catch
                    {
                        Notification.ShowError("Ошибка при поиске доступных таблиц. Обратитесь к администратору");
                    }
                }

            }



            int TabCount = TabHeaders.Length;


            if (TabCount > 0)
            {

                Brush ColorToBrush = new SolidColorBrush(new Color()
                {
                    R = 229,
                    G = 229,
                    B = 229,
                    A = 255,
                });

                for (int i = 0; i < TabCount; i++)
                {
                    DataGrid CurrentDataGrid = new DataGrid()
                    {
                        Name = TabNames[i],
                        Margin = new Thickness
                        {
                            Left = 0,
                            Top = 0,
                            Right = 0,
                            Bottom = 0,
                        },
                        VerticalAlignment = VerticalAlignment.Stretch,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        IsReadOnly = true,
                        CanUserReorderColumns = false,
                    };


                    Grid CurrentGrid = new Grid()
                    {
                        Background = ColorToBrush,
                    };

                    TabItem CurrentTabItem = new TabItem()
                    {
                        IsSelected = i == 0,
                        Header = TabHeaders.ElementAt(i),
                        Name = TabNames.ElementAt(i),
                    };


                    CurrentGrid.Children.Add(CurrentDataGrid);
                    CurrentTabItem.Content = CurrentGrid;



                    Result.Add(CurrentTabItem);

                    string PKey = "";
                    switch (TabNames[i])
                    {
                        case "employees":
                            PKey = "e_id";
                            break;
                        case "orders":
                            PKey = "o_id";
                            break;
                        case "parts":
                            PKey = "part_id";
                            break;
                        case "positions":
                            PKey = "p_id";
                            break;
                        case "repaired_models":
                            PKey = "rm_id";
                            break;
                        case "served_shops":
                            PKey = "ss_id";
                            break;
                        case "fault_types":
                            PKey = "ft_id";
                            break;
                        case "parts_faults":
                            PKey = "pf_id";
                            break;
                    }



                    if (Window.Name == "TablesWindow")
                    {
                        Variables.MyDGs.Add(new MyDataGrid()
                        {
                            DG = CurrentDataGrid,
                            PK = PKey,
                        });
                        CurrentDataGrid.MouseDoubleClick += CurrentDataGrid_MouseDoubleClick;
                        Variables.TablesWindow_Window.RegisterName(CurrentDataGrid.Name, CurrentDataGrid);
                        Variables.DataGrids.Add(CurrentDataGrid);
                    }
                    else
                    {
                        Variables.ReportGrids.Add(CurrentDataGrid);
                    }

                }
            }
            return Result;
        }

        private static void CurrentDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Variables.TablesWindow_Window.ChangeRowButton_Click(new object(), new RoutedEventArgs());
        }

        public static void Clear(TablesWindow thisWindow)
        {
            thisWindow.MainTabs.Items.Clear();
        }

    }
}
