using GonharovCafeKK.AppFolder.AdminFolder;
using GonharovCafeKK.AppFolder.EntityFolder;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TC_Application.AppFolder.GlobalClassFolder;

namespace GonharovCafeKK.AdminFolder
{
    /// <summary>
    /// Логика взаимодействия для UserListPage.xaml
    /// </summary>
    public partial class UserListPage : Page
    {
        Frame AddEditFrame;



        public UserListPage(Frame AddEditFrame)
        {
            InitializeComponent();

            this.AddEditFrame = AddEditFrame;

            UserListDG.ItemsSource = DBEntities.GetContext().User.ToList();
        }


        public void UpdateList()
        {
            UserListDG.ItemsSource = DBEntities.GetContext().User.Where(u => u.Surname.Contains(SearchTB.Text)
                                                                           || u.Name.Contains(SearchTB.Text)
                                                                           || u.Patronymic.Contains(SearchTB.Text)
                                                                           || u.Login.Contains(SearchTB.Text)
                                                                           || u.PhoneNum.Contains(SearchTB.Text)
                                                                           || u.Role.NameRole.Contains(SearchTB.Text)
                                                                           || u.StatusUser.NameStatus.Contains(SearchTB.Text)).ToList();
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(SearchTB.Text))
            {
                RemoveSearchTextLB.Visibility = Visibility.Hidden;
            }
            else
            {
                RemoveSearchTextLB.Visibility = Visibility.Visible;
            }
        }

        private void NewWorkerBTN_Click(object sender, RoutedEventArgs e)
        {
            AddEditFrame.Navigate(new AEUserPage(AddEditFrame,this ,null));
        }

        private void EditMI_Click(object sender, RoutedEventArgs e)
        {
            Edit();
        }

        private void RemoveMI_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedUser = UserListDG.SelectedItem as User;




                if (MBClass.Question("Вы действительно хотите удалить этого сотрудника?\n"))
                {
                    var order = DBEntities.GetContext().Order.Where(u => u.UserID == selectedUser.UserID);

                    if (order != null)
                    {
                        foreach (var item in order)
                        {
                            item.UserID = null;
                        }
                    }

                    DBEntities.GetContext().User.Remove(selectedUser);

                    DBEntities.GetContext().SaveChanges();

                    UpdateList();
                }
            }
            catch (Exception ex)
            {

                MBClass.Error(ex, "Ошибка при удалении");
            }

        }

        private void UserListDG_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Edit();
        }

        private void Edit()
        {
            if (UserListDG.SelectedItem != null)
            {
                AddEditFrame.Navigate(new AEUserPage(AddEditFrame, this, UserListDG.SelectedItem as User));
            }
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            EditMI.IsEnabled = false;
            StatusMI.IsEnabled = false;
            RemoveMI.IsEnabled = false;

            StatusActionMI.IsEnabled = false;
            StatusBlockedMI.IsEnabled = false;

            if (UserListDG.SelectedItem != null)
            {
                EditMI.IsEnabled = true;
                if((UserListDG.SelectedItem as User).UserID == 1)
                {
                    StatusMI.IsEnabled = false;
                    RemoveMI.IsEnabled = false;
                }
                else
                {
                    StatusMI.IsEnabled = true;
                    RemoveMI.IsEnabled = true;
                }

                if((UserListDG.SelectedItem as User).StatusID == 1)
                {
                    StatusBlockedMI.IsEnabled = true;
                }
                else
                {
                    StatusActionMI.IsEnabled = true;
                }


            }
        }

        private void StatusActionMI_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                (UserListDG.SelectedItem as User).StatusID = 1;


                DBEntities.GetContext().SaveChanges();

                UpdateList();

            }
            catch (Exception ex)
            {

                MBClass.Error(ex, "Ошибка смены статуса");
            }
        }

        private void StatusBlockedMI_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                (UserListDG.SelectedItem as User).StatusID = 2;


                DBEntities.GetContext().SaveChanges();

                UpdateList();

            }
            catch (Exception ex)
            {

                MBClass.Error(ex, "Ошибка смены статуса");
            }
        }

        private void SearchBT_Click(object sender, RoutedEventArgs e)
        {
            UpdateList();
        }

        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SearchTB.Text = "";

            UpdateList();
        }

        private void SearchTB_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                SearchBT_Click(sender, e);
            }
        }
    }
}