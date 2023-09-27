using GonharovCafeKK.AppFolder.EntityFolder;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using Vimpel_Accounting.AppFolder.ClassFolder;

namespace GonharovCafeKK.AppFolder.AdminFolder
{
    /// <summary>
    /// Логика взаимодействия для AEUserPage.xaml
    /// </summary>
    public partial class AEUserPage : Page
    {
        User user;

        Frame AEFrame;
        DataGrid UserListDG;
        TextBox SearchTB;

        private string selectedPhoto = "";

        public AEUserPage(Frame AEFrame, User user, DataGrid UserListDG, TextBox SearchTB)
        {
            this.AEFrame = AEFrame;
            this.UserListDG = UserListDG;
            this.SearchTB = SearchTB;


            InitializeComponent();

            RoleCB.ItemsSource = DBEntities.GetContext().Role.ToList();
            StatusUserCB.ItemsSource = DBEntities.GetContext().StatusUser.ToList();

            PhoneNumTB.Text += "+7 ";
            PhoneNumTB.CaretIndex = PhoneNumTB.Text.Length;

            if (user != null)
            {
                this.user = user;

                WinLB.Content = Title = "Редактирование рабочего";
                AddEditBTN.Content = "Редактировать";

                PhotoIB.ImageSource = LoadReadImageClass.GetImageFromBytes(user.Photo);
                selectedPhoto = "Картинка кароче есть";

                LoginTB.Text = user.Login;
                PasswordTB.Text = user.Password;

                SNPworkerTB.Text = $"{user.Surname} {user.Name}";

                if (user.Patronymic != null)
                {
                    SNPworkerTB.Text += $" {user.Patronymic}";
                }

                PhoneNumTB.Text = user.PhoneNum;

                RoleCB.SelectedValue = user.RoleID;
                StatusUserCB.SelectedValue = user.StatusID;
            }

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


        private void AddEditBTN_Click(object sender, RoutedEventArgs e)
        {

            if (user == null)
            {
                Add();
            }
            else
            {
                Edit();
            }
        }

        private void Add()
        {
            try
            {
                var checkUser = DBEntities.GetContext().User.FirstOrDefault(u => u.Login == LoginTB.Text);

                if (checkUser != null)
                {
                    MBClass.Error("Данный логин уже существует");
                    return;
                }

                User user = new User();

                user.Photo = LoadReadImageClass.SetImageToBytes(selectedPhoto);

                user.Login = LoginTB.Text;
                user.Password = PasswordTB.Text;

                string[] splitSNP = SNPworkerTB.Text.Split(' ');

                user.Surname = splitSNP[0].Trim();
                user.Name = splitSNP[1].Trim();
                if (splitSNP.Length > 2)
                {
                    user.Patronymic = splitSNP[2].Trim();
                }

                user.PhoneNum = PhoneNumTB.Text;
                user.RoleID = (int)RoleCB.SelectedValue;
                user.StatusID = (int)StatusUserCB.SelectedValue;

                DBEntities.GetContext().User.Add(user);

                DBEntities.GetContext().SaveChanges();

                MBClass.Info("Рабочий успешно добавлен!");

                UpdateList();
                AEFrame.Navigate(null);


            }
            catch (Exception ex)
            {
                MBClass.Error(ex,"Ошибка при добавлении");
            }
        }

        private void Edit()
        {
            try
            {
                if (user.Login != LoginTB.Text)
                {
                    var checkUser = DBEntities.GetContext().User.FirstOrDefault(u => u.Login == LoginTB.Text);

                    if (checkUser != null)
                    {
                        MBClass.Error("Данный логин уже существует");
                        return;
                    }
                    user.Login = LoginTB.Text;
                }

                user.Password = PasswordTB.Text;


                if (selectedPhoto != "Картинка кароче есть")
                    user.Photo = LoadReadImageClass.SetImageToBytes(selectedPhoto);


                string[] splitSNP = SNPworkerTB.Text.Split(' ');

                user.Surname = splitSNP[0].Trim();
                user.Name = splitSNP[1].Trim();
                if (splitSNP.Length > 2)
                {
                    user.Patronymic = splitSNP[2].Trim();
                }

                user.PhoneNum = PhoneNumTB.Text;
                user.RoleID = (int)RoleCB.SelectedValue;
                user.StatusID = (int)StatusUserCB.SelectedValue;


                DBEntities.GetContext().SaveChanges();

                MBClass.Info("Рабочий успешно отредактирован!");

                UpdateList();
                AEFrame.Navigate(null);

            }
            catch (Exception ex)
            {
                MBClass.Error(ex,"Ошибка при добавлении");
            }
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            AddPhoto();
        }


        private void AddPhoto()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png *.jpeg)|*.png;*.jpeg";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);


            if (openFileDialog.ShowDialog() == true)
            {
                selectedPhoto = openFileDialog.FileName;
                PhotoIB.ImageSource = LoadReadImageClass.GetImageFromBytes(LoadReadImageClass.SetImageToBytes(selectedPhoto));
                EnableButton();
            }
        }

        private void TextBox_Changed(object sender, TextChangedEventArgs e)
        {


            if ((sender as TextBox).Name == "PhoneNumTB")
            {
                Phone();
            }

            EnableButton();
        }


        private void Phone()
        {
            TextBox PhoneNumText = PhoneNumTB;

            try
            {
                if (!Keyboard.IsKeyDown(Key.Back))
                {
                    switch (PhoneNumText.Text.Length)
                    {
                        case 7:
                            PhoneNumText.Text = PhoneNumText.Text.Insert(6, "-");
                            PhoneNumText.CaretIndex = 8;
                            break;
                        case 11:
                            PhoneNumText.Text = PhoneNumText.Text.Insert(10, "-");
                            PhoneNumText.CaretIndex = 12;
                            break;
                        case 14:
                            PhoneNumText.Text = PhoneNumText.Text.Insert(13, "-");
                            PhoneNumText.CaretIndex = 15;
                            break;
                    }
                }

                else
                {
                    if (PhoneNumText.Text.Length < 4)
                    {
                        PhoneNumText.Text = "+7 ";
                        PhoneNumText.CaretIndex = 4;
                    }
                    switch (PhoneNumText.Text.Length)
                    {
                        case 7:
                            PhoneNumText.Text =
                                PhoneNumText.Text.Remove(PhoneNumText.Text.LastIndexOf("-"));
                            PhoneNumText.CaretIndex = 6;
                            break;
                        case 11:
                            PhoneNumText.Text =
                                PhoneNumText.Text.Remove(PhoneNumText.Text.LastIndexOf("-"));
                            PhoneNumText.CaretIndex = 10;
                            break;
                        case 14:
                            PhoneNumText.Text =
                                PhoneNumText.Text.Remove(PhoneNumText.Text.LastIndexOf("-"));
                            PhoneNumText.CaretIndex = 13;
                            break;
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }


        private void RoleCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButton();
        }

        private void EnableButton()
        {
            string[] splitSNP = SNPworkerTB.Text.Split(' ');

            if (string.IsNullOrWhiteSpace(LoginTB.Text) ||
                string.IsNullOrWhiteSpace(SNPworkerTB.Text) ||
                string.IsNullOrWhiteSpace(PhoneNumTB.Text) ||
                RoleCB.SelectedValue == null ||
                StatusUserCB.SelectedValue == null ||
                selectedPhoto == "" || splitSNP.Length < 2 ||
                PhoneNumTB.Text.Length < 16)
            {
                AddEditBTN.IsEnabled = false;
            }
            else
            {
                AddEditBTN.IsEnabled = true;
            }
        }

        private void PassportNumTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            AEFrame.Navigate(null);


        }

        private void StatusUserCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButton();
        }
    }
}
