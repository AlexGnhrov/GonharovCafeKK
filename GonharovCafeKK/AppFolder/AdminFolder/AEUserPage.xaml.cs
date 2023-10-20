using GonharovCafeKK.AdminFolder;
using GonharovCafeKK.AppFolder.EntityFolder;
using GonharovCafeKK.AppFolder.ResourceFolder;
using Microsoft.Win32;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        MainWindow mainWindow;
        UserListPage userListPage;


        private string selectedPhoto = "";

        public AEUserPage(MainWindow mainWindow, Frame AEFrame ,UserListPage userListPage,User user)
        {
            this.AEFrame = AEFrame;
            this.mainWindow = mainWindow;
            this.userListPage = userListPage;

            userListPage.IsEnabled = false;

            InitializeComponent();

            try
            {
                RoleCB.ItemsSource = DBEntities.GetContext().Role.ToList();
                StatusUserCB.ItemsSource = DBEntities.GetContext().StatusUser.ToList();


                PhoneNumTB.Text += "+7 ";
                PhoneNumTB.CaretIndex = PhoneNumTB.Text.Length;

                StatusUserCB.SelectedValue = 1;

                if (user != null)
                {
                    this.user = user;

                    if(user.UserID == 1)
                    {
                        RoleCB.IsEnabled = false;
                        StatusUserCB.IsEnabled = false;
                    }

                    WinLB.Content = Title = "Редактирование сотрудника";
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

                    AddEditBTN.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {

                MBClass.Error(ex, "Ошибка выгрузки данных");
                userListPage.IsEnabled = true;
            }

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
                var checkUser = DBEntities.GetContext().User.FirstOrDefault(u => u.Login == LoginTB.Text.Trim());

                if (checkUser != null)
                {
                    MBClass.Error("Данный логин уже существует");
                    return;
                }

                User user = new User();

                user.Photo = LoadReadImageClass.SetImageToBytes(selectedPhoto);

                user.Login = LoginTB.Text.Trim();
                user.Password = PasswordTB.Text.Trim();

                string[] splitSNP = SNPworkerTB.Text.Split(' ');

                user.Surname = splitSNP[0].Trim();
                user.Name = splitSNP[1].Trim();

                user.Patronymic = splitSNP.Length > 2 ? splitSNP[2].Trim() : null;

                user.PhoneNum = PhoneNumTB.Text.Trim();
                user.RoleID = (int)RoleCB.SelectedValue;
                user.StatusID = (int)StatusUserCB.SelectedValue;

                DBEntities.GetContext().User.Add(user);

                DBEntities.GetContext().SaveChanges();

                MBClass.Info("Рабочий успешно добавлен!");

                userListPage.UpdateList();
                userListPage.IsEnabled = true;
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
                    user.Login = LoginTB.Text.Trim();
                }

                user.Password = PasswordTB.Text.Trim();


                if (selectedPhoto != "Картинка кароче есть")
                    user.Photo = LoadReadImageClass.SetImageToBytes(selectedPhoto);

                string[] splitSNP = SNPworkerTB.Text.Split(' ');

                user.Surname = splitSNP[0].Trim();
                user.Name = splitSNP[1].Trim();

                user.Patronymic = splitSNP.Length > 2 ? splitSNP[2].Trim() : null;

                user.PhoneNum = PhoneNumTB.Text.Trim();
                user.RoleID = (int)RoleCB.SelectedValue;
                user.StatusID = (int)StatusUserCB.SelectedValue;


                DBEntities.GetContext().SaveChanges();

                MBClass.Info("Рабочий успешно отредактирован!");

                userListPage.UpdateList();
                userListPage.IsEnabled = true;

                AEFrame.Navigate(null);

                mainWindow.UpdateUserData();

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
            else if((sender as TextBox).Name == "SNPworkerTB")
            {
                string[] SplitSNP = SNPworkerTB.Text.Split(' ');

                int strlen = SNPworkerTB.Text.Length;

                if (SplitSNP.Length > 3)
                {
                    SNPworkerTB.Text = SNPworkerTB.Text.Remove(strlen - 1);
                    SNPworkerTB.CaretIndex = strlen;
                }

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
                string.IsNullOrWhiteSpace(PasswordTB.Text) ||
                string.IsNullOrWhiteSpace(SNPworkerTB.Text) ||
                string.IsNullOrWhiteSpace(PhoneNumTB.Text) ||
                RoleCB.SelectedValue == null ||
                StatusUserCB.SelectedValue == null || 
                selectedPhoto == "" || PhoneNumTB.Text.Length < 16 ||
                                 (splitSNP.Length == 1 ||
                 splitSNP.Length == 2 && splitSNP[1] == "" ||
                 splitSNP.Length == 3 && splitSNP[2] == ""))
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
            userListPage.IsEnabled = true;
            AEFrame.Navigate(null);
        }

        private void StatusUserCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButton();
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if(AddEditBTN.IsEnabled)
                {
                    AddEditBTN_Click(sender, e);
                }
            }
        }
    }
}
