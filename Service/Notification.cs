using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Service
{
    class Notification
    {
        public static void ShowError(string msg, string err)
        {
            MessageBox.Show(msg, err, MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public static void ShowError(string msg)
        {
            MessageBox.Show(msg, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public static void ShowNotice(string msg)
        {
            MessageBox.Show(msg, "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public static MessageBoxResult ShowAsk(string msg)
        {
            return MessageBox.Show(msg, "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Information);
        }
    }
}
