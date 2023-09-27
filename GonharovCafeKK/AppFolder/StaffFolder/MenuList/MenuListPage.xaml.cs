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

namespace GonharovCafeKK.AppFolder.MenuList
{
    /// <summary>
    /// Логика взаимодействия для MenuListPage.xaml
    /// </summary>
    public partial class MenuListPage : Page
    {
        ListView[] listViews = new ListView[10];
        StackPanel[] stackPanelsMenus = new StackPanel[10];

        public MenuListPage()
        {
            InitializeComponent();

            listViews[0] = GrillChesseLV;
            listViews[1] = SeasonLV;
            listViews[2] = KroshkaKartoskaLV;
            listViews[3] = FillerLV;
            listViews[4] = SaladLV;
            listViews[5] = SoupAndHotLV;
            listViews[6] = SnacksLV;
            listViews[7] = SandvichesAndRollsLV;
            listViews[8] = DesertesLV;
            listViews[9] = DrinksLV;

            stackPanelsMenus[0] = GrillChesseSP;
            stackPanelsMenus[1] = SeasonSP;
            stackPanelsMenus[2] = KroshkaKartoskaSP;
            stackPanelsMenus[3] = FillerSP;
            stackPanelsMenus[4] = SaladSP;
            stackPanelsMenus[5] = SoupAndHotSP;
            stackPanelsMenus[6] = SnacksSP;
            stackPanelsMenus[7] = SandvichesAndRollsSP;
            stackPanelsMenus[8] = DesertesSP;
            stackPanelsMenus[9] = DrinksSP;

            UpdateList();

            
        }


        private void RemoveMenu_Click(object sender, RoutedEventArgs e)
        {
            

            object selectedItem;

            CheckItemForNull(out selectedItem);

            if (selectedItem == null)
            {
                return;
            }

            try
            {
                if (MBClass.Question("Вы действительно хотите удалить это блюдо?"))
                {

                    EntityFolder.Menu selectedDish = selectedItem as EntityFolder.Menu;

                    DBEntities.GetContext().Menu.Remove(selectedDish);

                    DBEntities.GetContext().SaveChanges();

                    MBClass.Info("Блюдо успешно удалено.");

                    UpdateList();
                }
            }
            catch(Exception ex)
            {
                MBClass.Error(ex, "Ошибка при удалени.");
            }
        }

        private void EditMenu_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = false;
        }

        private void SearchBT_Click(object sender, RoutedEventArgs e)
        {

            UpdateList();

        }


        public async void UpdateList()
        {
            SearchBT.IsEnabled = false;


            try
            {
                var data = DBEntities.GetContext().Menu.ToArray();

                var arrText = SearchTB.Text.Split(' ');

                foreach (var item in arrText)
                {
                    data = data.Where(u => u.NameDish.Contains(item)).ToArray();

                    await Task.Delay(10);
                }




                GrillChesseLV.ItemsSource = data.ToList();
                SeasonLV.ItemsSource = data.ToList();
                KroshkaKartoskaLV.ItemsSource = data.ToList();
                FillerLV.ItemsSource = data.ToList();
                SaladLV.ItemsSource = data.ToList();
                SoupAndHotLV.ItemsSource = data.ToList();
                SnacksLV.ItemsSource = data.ToList();
                SandvichesAndRollsLV.ItemsSource = data.ToList();
                DesertesLV.ItemsSource = data.ToList();
                DrinksLV.ItemsSource = data.ToList();

                for (int i = 0; i < listViews.Length; i++)
                {
                    

                    if (listViews[i].Items.Count > 0)
                    {
                        stackPanelsMenus[i].Visibility = Visibility.Visible;
                    }
                    else
                    {
                        stackPanelsMenus[i].Visibility = Visibility.Collapsed;
                    }
                }



            }
            catch (Exception ex)
            {
                MBClass.Error(ex, "Ошибка выгрузки меню");

            }
            finally
            {
                SearchBT.IsEnabled = true;
            }

        }

        private void SearchTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchBT_Click(sender, e);
            }
        }


        private void CheckItemForNull(out object outItem)
        {
            outItem = null;

            foreach (var item in listViews)
            {
                if (item.SelectedItem != null)
                {
                    outItem = item.SelectedItem;
                    break;
                }
            }

        }

        private void AddToOrder_Click(object sender, RoutedEventArgs e)
        {

            object selectedItem;

            CheckItemForNull(out selectedItem);

            if (selectedItem == null)
            {
                return;
            }

            EntityFolder.Menu selectedDish = selectedItem as EntityFolder.Menu;

            MBClass.Error(selectedDish.NameDish);
        }

        private void AddDishBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SearchTB.Text = "";

            UpdateList();
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(SearchTB.Text))
            {
                RemoveSearchTextLB.Visibility = Visibility.Visible;
            }
            else
            {
                RemoveSearchTextLB.Visibility = Visibility.Collapsed;
            }
        }
    }
}
