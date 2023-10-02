using GonharovCafeKK.AppFolder.MenuList;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GonharovCafeKK.AppFolder.StaffFolder.MenuList
{
    /// <summary>
    /// Логика взаимодействия для DishInfoPage.xaml
    /// </summary>
    public partial class DishInfoPage : Page
    {
        Frame AEFrame;
        MenuListPage menuListPage;

        TimeSpan blockExit;

        public DishInfoPage(Frame AEFrame, MenuListPage menuListPage, EntityFolder.Menu selectedDish)
        {

            this.AEFrame = AEFrame;
            this.menuListPage = menuListPage;

            menuListPage.IsEnabled = false;

            DataContext = selectedDish;
            blockExit = DateTime.Now.AddSeconds(0.5).TimeOfDay;

            InitializeComponent();

            if ((bool)selectedDish.isNew)
            {
                NewB.Visibility = Visibility.Visible;
            }

            if ((bool)selectedDish.isHit)
            {
                HitB.Visibility = Visibility.Visible;
            }

            if (selectedDish.DishCategory.NameCategory.ToUpper().Contains("НАПИТКИ"))
            {
                EnergySP.Visibility = Visibility.Collapsed;

                WeightDishTB.Text = selectedDish.WeightDishGR + " мл.";
            }
            else
            {
                WeightDishTB.Text = selectedDish.WeightDishGR + " г.";
            }

            if (selectedDish.Calories == null && selectedDish.Protein == null ||
                selectedDish.Fat == null && selectedDish.Carb == null)
            {
                EnergySP.Visibility = Visibility.Collapsed;
            }


        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DateTime.Now.TimeOfDay > blockExit)
            {
                menuListPage.IsEnabled = true;
                AEFrame.Navigate(null);
            }
        }
    }
}
