using GonharovCafeKK.AdminFolder;
using GonharovCafeKK.AppFolder.MenuList;
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
using TC_Application.AppFolder.GlobalClassFolder;

namespace GonharovCafeKK.AppFolder.ResourceFolder
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }



        private void ExpandShrinkeColumnLB_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            if (ShrinkeColumnLB.Visibility == Visibility.Hidden)
            {
                LeftColumnCD.Width = new GridLength(225);
                UserDataRD.Height = new GridLength(195);

                ExpandColumnLB.Visibility = Visibility.Hidden;
                ShrinkeColumnLB.Visibility = Visibility.Visible;

                RoleNameLB.Visibility =
                UserDataSP.Visibility =
                UserTB.Visibility =
                MenuTB.Visibility =
                OrderTB.Visibility =
                ExitTB.Visibility =
                    Visibility.Visible;

            }
            else
            {
                LeftColumnCD.Width = new GridLength(65);
                UserDataRD.Height = new GridLength(30);

                ExpandColumnLB.Visibility = Visibility.Visible;
                ShrinkeColumnLB.Visibility = Visibility.Hidden;

                RoleNameLB.Visibility = 
                    UserDataSP.Visibility =
                    UserTB.Visibility =
                    MenuTB.Visibility =
                    OrderTB.Visibility =
                    ExitTB.Visibility =
                    Visibility.Collapsed;
            }
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            if(MBClass.Question("Вы действительно хотите сменить пользователя"))
            {
                new AuthorizationWin().Show();
                Close();
            }    
        }


        Button usingBtn = null;
        private void MenuButtons_Click(object sender, RoutedEventArgs e)
        {
            Button selectedBtn = sender as Button;

            if (usingBtn != null)
                usingBtn.Background = null;

            usingBtn = selectedBtn;

            selectedBtn.Background = new SolidColorBrush(Color.FromRgb(35, 102, 2));

            switch (selectedBtn.Name)
            {
                case "UserListBtn":                    




                    MainFrame.Navigate(new UserListPage(AddEditFrame));


                    break;
                case "MenuListBtn":

                    MainFrame.Navigate(new MenuListPage());

                    break;
                case "OrderListBtn":

                    MainFrame.Navigate(new OrderListPage());

                    break;

            }
        }


        private void SomeElement_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Предотвратить навигацию при нажатии на элемент
            e.Handled = false;
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void RoofBtn_Click(object sender, RoutedEventArgs e)
        {
            switch((sender as Button).Name)
            {
                case "HideBtn":
                    WindowState = WindowState.Minimized;
                    break;
                case "ResizeBtn":
                    WindowState = WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
                    break;
                case "CloseBtn":
                    if(MBClass.Question("Вы действительно хотите закрыть приложение?"))
                    {
                        App.Current.Shutdown();
                    }
                    break;
            }
        }
    }
}
