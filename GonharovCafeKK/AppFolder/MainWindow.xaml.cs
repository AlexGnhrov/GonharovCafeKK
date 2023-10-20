using GonharovCafeKK.AdminFolder;
using GonharovCafeKK.AppFolder.EntityFolder;
using GonharovCafeKK.AppFolder.MenuList;
using GonharovCafeKK.GlobalClassFolder;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TC_Application.AppFolder.GlobalClassFolder;
using Vimpel_Accounting.AppFolder.ClassFolder;

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

            AddEditFrame.Visibility = Visibility.Visible;

            InfoMainFrameB.Visibility = Visibility.Hidden;

            try
            {
                UpdateUserData();

                if (GlobalVariablesClass.currentRoleID == 2)
                {
                    UserListBtn.Visibility = Visibility.Collapsed;
                    MainFrame.Navigate(new MenuListPage(AddEditFrame));

                    usingBtn = MenuListBtn;
                    usingBtn.Tag = "Using";
                }

                InfoMainFrameB.Visibility = Visibility.Visible;

            }
            catch (Exception ex)
            {

                MBClass.Error(ex, "Ошибка выгрузки");

                new AuthorizationWin().Show();
                Close();
            }


        }

        public void UpdateUserData()
        {
            try
            {
                var currentUser = DBEntities.GetContext().User.FirstOrDefault(u => u.UserID == GlobalVariablesClass.currentUserID);

                DataContext = currentUser;

                PhotoUserIB.ImageSource = LoadReadImageClass.GetImageFromBytes(currentUser.Photo);

                SNPworkerTBl.Text = $"{currentUser.Surname} {currentUser.Name.Remove(1)}.";

                if (currentUser.Patronymic != null)
                    SNPworkerTBl.Text += $"{currentUser.Patronymic.Remove(1)}.";
            }
            catch
            {

            };

        }

        private void ExpandShrinkeColumnLB_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            if (ShrinkeColumnLB.Visibility == Visibility.Hidden)
            {
                LeftColumnCD.Width = new GridLength(225);
                UserDataRD.Height = new GridLength(195);

                ExpandColumnLB.Visibility = Visibility.Hidden;
                ShrinkeColumnLB.Visibility = Visibility.Visible;


                AppLogoE.Margin = new Thickness(35, 0, 35, 35);

                UserTB.Visibility =
                MenuTB.Visibility =
                OrderTB.Visibility =
                ExitTB.Visibility =
                    Visibility.Visible;

            }
            else
            {
                LeftColumnCD.Width = new GridLength(65);
                UserDataRD.Height = new GridLength(100);

                ExpandColumnLB.Visibility = Visibility.Visible;
                ShrinkeColumnLB.Visibility = Visibility.Hidden;

                AppLogoE.Margin = new Thickness(5, 10, 5, 35);

                UserTB.Visibility =
                    MenuTB.Visibility =
                    OrderTB.Visibility =
                    ExitTB.Visibility =
                    Visibility.Collapsed;
            }
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MBClass.Question("Вы действительно хотите сменить пользователя"))
            {

                GlobalVariablesClass.isOrdering = false;
                new AuthorizationWin().Show();
                Close();
            }
        }


        Button usingBtn = null;

        private void MenuButtons_Click(object sender, RoutedEventArgs e)
        {


            Button selectedBtn = sender as Button;


            if (usingBtn != null && (selectedBtn.Content == usingBtn.Content)) return;

            if (GlobalVariablesClass.isOrdering && !MBClass.Question("Сменив список заказ будет утерян.\n" +
                                                         "Вы действительно хотите перейти?"))
            {
                return;
            }
            else
            {
                GlobalVariablesClass.isOrdering = false;
            }


            switch (selectedBtn.Name)
            {
                case "UserListBtn":

                    MainFrame.Navigate(new UserListPage(this,AddEditFrame));

                    break;
                case "MenuListBtn":

                    MainFrame.Navigate(new MenuListPage(AddEditFrame));

                    break;
                case "OrderListBtn":

                    MainFrame.Navigate(new OrderListPage(AddEditFrame));

                    break;

            }

            if (usingBtn != null)
            {
                usingBtn.Tag = "NoUse";
            }

            usingBtn = selectedBtn;
            usingBtn.Tag = "Using";

            UpdateUserData();
        }


        private void SomeElement_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.XButton1 || e.ChangedButton == MouseButton.XButton2)
            {
                e.Handled = true;
            }

        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void RoofBtn_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as Button).Name)
            {
                case "HideBtn":
                    WindowState = WindowState.Minimized;
                    break;
                case "ResizeBtn":
                    WindowState = WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
                    break;
                case "CloseBtn":
                    if (MBClass.Question("Вы действительно хотите закрыть приложение?"))
                    {
                        App.Current.Shutdown();
                    }
                    break;
            }
        }
    }
}
