using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TC_Application.AppFolder.GlobalClassFolder;

namespace TC_Application.DataFolder
{
    public static class InsertDataClass
    {


        public static void PasteOnlyNums(this ExecutedRoutedEventArgs e)
        {
            try
            {
                if (e.Command == ApplicationCommands.Paste)
                {
                    string text = Clipboard.GetText();

                    if (Convert.ToInt32(text) == 0 || Convert.ToInt32(text) < 0)
                    {
                        e.Handled = true;
                    }
                }
            }
            catch
            {
                e.Handled = true;
            }
        }

        public static void PasteOnlyNumsFloat(this ExecutedRoutedEventArgs e)
        {
            try
            {
                if (e.Command == ApplicationCommands.Paste)
                {
                    string text = Clipboard.GetText();

                    if (Convert.ToDouble(text) == 0 || Convert.ToDouble(text) < 0)
                    {
                        e.Handled = true;
                    }
                }
            }
            catch
            {
                e.Handled = true;
            }
        }

    }
}

