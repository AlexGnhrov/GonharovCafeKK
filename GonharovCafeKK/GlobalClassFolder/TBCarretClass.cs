using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TC_Application.AppFolder.GlobalClassFolder
{
    public static class TBCarretClass
    {
        public static void SetCarretsBack(this DependencyObject parent)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                if (child is TextBox)
                {
                    TextBox tb = (TextBox)child;
                    tb.CaretIndex = tb.Text.Length;
                }
                else
                {
                    SetCarretsBack(child);
                }
            }
        }

    }
}
