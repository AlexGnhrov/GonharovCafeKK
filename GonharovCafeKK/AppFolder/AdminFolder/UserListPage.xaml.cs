using GonharovCafeKK.AppFolder.AdminFolder;
using GonharovCafeKK.AppFolder.EntityFolder;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
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


        private void UpdateList()
        {
            UserListDG.ItemsSource = DBEntities.GetContext().User.Where(u => u.Surname.Contains(SearchTB.Text)
                                                                           || u.Name.Contains(SearchTB.Text)
                                                                           || u.Patronymic.Contains(SearchTB.Text)
                                                                           || u.Login.Contains(SearchTB.Text)
                                                                           || u.Role.NameRole.Contains(SearchTB.Text)
                                                                           || u.StatusUser.NameStatus.Contains(SearchTB.Text)).ToList();
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateList();
        }

        private void NewWorkerBTN_Click(object sender, RoutedEventArgs e)
        {
            AddEditFrame.Navigate(new AEUserPage(AddEditFrame, null,UserListDG, SearchTB));
        }

        private void EditMI_Click(object sender, RoutedEventArgs e)
        {
            Edit();
        }

        private void RemoveMI_Click(object sender, RoutedEventArgs e)
        {
            if (MBClass.Question("Вы действительно хотите удалить этого сотрудника?"))
            {
                DBEntities.GetContext().User.Remove(UserListDG.SelectedItem as User);

                DBEntities.GetContext().SaveChanges();

                UpdateList();
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
                AddEditFrame.Navigate(new AEUserPage(AddEditFrame, UserListDG.SelectedItem as User, UserListDG, SearchTB));
            }
        }
    }
}