using GonharovCafeKK.AppFolder.EntityFolder;
using GonharovCafeKK.AppFolder.MenuList;
using Microsoft.Win32;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TC_Application.AppFolder.GlobalClassFolder;
using Vimpel_Accounting.AppFolder.ClassFolder;

namespace GonharovCafeKK.AppFolder.StaffFolder.MenuList
{
    /// <summary>
    /// Логика взаимодействия для AEDishPage.xaml
    /// </summary>
    public partial class AEDishPage : Page
    {
        Frame AEFrame;
        MenuListPage menuListPage;
        EntityFolder.Menu selectedItem;

        string selectedPhoto = "";
        string saveNameDish = "";

        TimeSpan blockphoto;

        public AEDishPage(Frame AEFrame, MenuListPage menuListPage, EntityFolder.Menu selectedItem)
        {
            this.AEFrame = AEFrame;
            this.menuListPage = menuListPage;
            this.selectedItem = selectedItem;

            InitializeComponent();

            menuListPage.IsEnabled = false;

            try
            {
                CategoryCB.ItemsSource = DBEntities.GetContext().DishCategory.OrderBy(u => u.CategoryID).ToList();

                blockphoto = DateTime.Now.TimeOfDay;



                if (selectedItem != null)
                {
                    WinLB.Content = Title = "Редактирование блюда";

                    AddEditBtn.Content = "Редактировать";

                    NameDishTB.Text = saveNameDish = selectedItem.NameDish;

                    PhotoIB.ImageSource = LoadReadImageClass.GetImageFromBytes(selectedItem.Photo);
                    selectedPhoto = "Чотко";
                    PriceTB.Text = selectedItem.Price.ToString();
                    WeightTB.Text = selectedItem.WeightDishGR.ToString();
                    CategoryCB.SelectedValue = selectedItem.DishCategoryID;

                    KcalTB.Text = selectedItem.Calories.ToString();
                    FatTB.Text = selectedItem.Fat.ToString();
                    ProteinTB.Text = selectedItem.Protein.ToString();
                    СarbTB.Text = selectedItem.Carb.ToString();

                    CompositonTB.Text = selectedItem.Composition;

                    NewChB.IsChecked = (bool)selectedItem.isNew;
                    HitChB.IsChecked = (bool)selectedItem.isHit;
                    SeasonDishChB.IsChecked = (bool)selectedItem.isSeason;
                }
            }
            catch (Exception ex)
            {

                MBClass.Error(ex, "Ошибка выгрузки данных");

                menuListPage.IsEnabled = true;
                AEFrame.Navigate(null);
            }

        }

        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            menuListPage.IsEnabled = true;
            AEFrame.Navigate(null);
        }


        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DateTime.Now.TimeOfDay > blockphoto)
                AddPhoto();
        }


        private void AddPhoto()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png *.jpeg *.jpg)|*.png;*.jpeg;*.jpg";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);


            if (openFileDialog.ShowDialog() == true)
            {
                selectedPhoto = openFileDialog.FileName;
                PhotoIB.ImageSource = LoadReadImageClass.GetImageFromBytes(LoadReadImageClass.SetImageToBytes(selectedPhoto));
                EnableButton();

                blockphoto = DateTime.Now.AddSeconds(0.1).TimeOfDay;
            }
        }

        private void EnableButton()
        {
            AddEditBtn.IsEnabled = !(string.IsNullOrWhiteSpace(NameDishTB.Text) ||
                                    string.IsNullOrWhiteSpace(PriceTB.Text) ||
                                    string.IsNullOrWhiteSpace(WeightTB.Text) ||
                                    selectedPhoto == "" ||
                                    CategoryCB.SelectedValue == null);
        }


        private void TextBox_Changed(object sender, TextChangedEventArgs e)
        {
            TextBox selectedTextBox = sender as TextBox;

            if (selectedTextBox.Name == "KcalTB" ||
                selectedTextBox.Name == "FatTB" ||
                selectedTextBox.Name == "ProteinTB" ||
                selectedTextBox.Name == "СarbTB")
            {
                selectedTextBox.FloatNums();
            }

            EnableButton();
        }

        private void CategoryCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButton();
        }

        private void OnlyIntNums_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.OnlyNumsTB();
        }

        private void AddEditBtn_Click(object sender, RoutedEventArgs e)
        {
            bool isEdit = true;

            try
            {
                if (selectedItem == null)
                {
                    isEdit = false;

                    selectedItem = new EntityFolder.Menu();
                }

                var checkNameDish = DBEntities.GetContext().Menu.FirstOrDefault(u => u.NameDish == NameDishTB.Text.Trim());

                if (checkNameDish != null && !isEdit)
                {
                    MBClass.Error("Данное блюдо уже существует");
                    NameDishTB.Focus();
                    return;
                }

                selectedItem.NameDish = NameDishTB.Text.Trim(); ;

                if (selectedPhoto != "Чотко" && isEdit)
                    selectedItem.Photo = LoadReadImageClass.SetImageToBytes(selectedPhoto);

                if (selectedPhoto != "" && !isEdit)
                    selectedItem.Photo = LoadReadImageClass.SetImageToBytes(selectedPhoto);



                selectedItem.Price = Convert.ToInt32(PriceTB.Text);
                selectedItem.WeightDishGR = Convert.ToInt32(WeightTB.Text);
                selectedItem.DishCategoryID = (int)CategoryCB.SelectedValue;

                if (!string.IsNullOrWhiteSpace(KcalTB.Text))
                    selectedItem.Calories = Convert.ToDouble(KcalTB.Text);

                if (!string.IsNullOrWhiteSpace(FatTB.Text))
                    selectedItem.Fat = Convert.ToDouble(FatTB.Text);

                if (!string.IsNullOrWhiteSpace(ProteinTB.Text))
                    selectedItem.Protein = Convert.ToDouble(ProteinTB.Text);

                if (!string.IsNullOrWhiteSpace(СarbTB.Text))
                    selectedItem.Carb = Convert.ToDouble(СarbTB.Text);

                selectedItem.Composition = CompositonTB.Text.Trim();

                selectedItem.isNew = (bool)NewChB.IsChecked;
                selectedItem.isHit = (bool)HitChB.IsChecked;
                selectedItem.isSeason = (bool)SeasonDishChB.IsChecked;

                if (!isEdit)
                    DBEntities.GetContext().Menu.Add(selectedItem);

                DBEntities.GetContext().SaveChanges();


                if (isEdit)
                    MBClass.Info("Блюдо успешно отредактировано!");
                else
                    MBClass.Info("Блюдо успешно добавлено!");

                menuListPage.UpdateList();

                menuListPage.IsEnabled = true;
                AEFrame.Navigate(null);

            }
            catch (Exception ex)
            {

                MBClass.Error(ex.Message);

                if(!isEdit)
                {
                    selectedItem = null;
                }

            }


        }


        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (AddEditBtn.IsEnabled)
                    AddEditBtn_Click(sender, e);
            }
        }

    }

}

