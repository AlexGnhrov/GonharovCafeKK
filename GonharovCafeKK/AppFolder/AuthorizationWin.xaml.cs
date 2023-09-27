using GonharovCafeKK.AppFolder.ResourceFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GonharovCafeKK.AppFolder
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWin.xaml
    /// </summary>
    public partial class AuthorizationWin : Window
    {
        public AuthorizationWin()
        {
            InitializeComponent();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }


        private void CloseLB_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void EnterBT_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }

        private void LoginTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableButton();
        }

        private void PasswordPB_PasswordChanged(object sender, RoutedEventArgs e)
        {
            EnableButton();
        }


        private void EnableButton()
        {
            EnterBT.IsEnabled = !(string.IsNullOrWhiteSpace(LoginTB.Text) || string.IsNullOrWhiteSpace(PasswordPB.Password));
        }


        private void Text_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (EnterBT.IsEnabled)
                {
                    EnterBT_Click(sender, e);
                    return;
                }


                if (LoginTB.IsFocused)
                {
                    PasswordPB.Focus();
                }
                else
                {
                    LoginTB.Focus();
                }
            }
        }
    }
}
