using System;
using System.Windows;

namespace TC_Application.AppFolder.GlobalClassFolder
{
    class MBClass
    {
        public static void Error(string text)
        {
            MessageBox.Show(text, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void Error(Exception ex, string Caption)
        {
            MessageBox.Show(ex.Message, Caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void Info(string text)
        {
            MessageBox.Show(text, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static bool Question(string text)
        {
            return MessageBoxResult.Yes ==
                  MessageBox.Show(text, "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question);
        }


        public static bool Risk(string text)
        {
            return MessageBoxResult.Yes ==
                MessageBox.Show(text, "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
        }

    }
}
