using GonharovCafeKK.AppFolder.EntityFolder;
using GonharovCafeKK.AppFolder.MenuList;
using GonharovCafeKK.AppFolder.StaffFolder.MenuList.ClassFolder;
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

namespace GonharovCafeKK.AppFolder.StaffFolder.MenuList
{
    /// <summary>
    /// Логика взаимодействия для PreviewOrderPage.xaml
    /// </summary>
    public partial class PreviewOrderPage : Page
    {
        Frame AEFrame;
        MenuListPage menuListPage;
        EntityFolder.Menu selectedDish;
        DataGrid PlacinOrderDG;

        TimeSpan blockExit;

        List<FillerItemsOrderClass> fillerItemsOrders;

        int DishPriceOne;
        int DishPriceOneCopy;

        bool isLoad = false;
        bool isMash = false;

        public PreviewOrderPage(Frame AEFrame, MenuListPage menuListPage,
                                EntityFolder.Menu selectedDish, DataGrid PlacinOrderDG)
        {
            this.AEFrame = AEFrame;
            this.menuListPage = menuListPage;
            this.PlacinOrderDG = PlacinOrderDG;
            this.selectedDish = selectedDish;

            DataContext = selectedDish;

            menuListPage.IsEnabled = false;
            blockExit = DateTime.Now.AddSeconds(0.5).TimeOfDay;

            InitializeComponent();

            AddiPanelSP.Visibility = Visibility.Collapsed;

            try
            {


                if (selectedDish.NameDish.ToUpper().Contains("КРОШКА КАРТОШКА"))
                {
                    AddiPanelSP.Visibility = Visibility.Visible;
                    SecondFillerCB.Visibility = Visibility.Collapsed;

                    LoadData(true, false);
                }
                else if (selectedDish.NameDish.ToUpper().Contains("ПЕЧЁНЫЙ МЭШ"))
                {
                    isMash = true;
                    AddToOrderBtn.IsEnabled = false;

                    AddiPanelSP.Visibility = Visibility.Visible;

                    AddiIngredientsSP.Visibility = Visibility.Collapsed;

                    if (selectedDish.NameDish.ToUpper().Contains("СТАНДАРТ"))
                    {
                        SecondFillerCB.Visibility = Visibility.Collapsed;
                        LoadData(false, false);
                    }
                    else
                    {
                        LoadData(false, true);
                    }

                }


                PriceLB.Content = DishPriceOne = DishPriceOneCopy = selectedDish.Price;

                AmountTB.Text = "1";

                if ((bool)selectedDish.isNew)
                    NewB.Visibility = Visibility.Visible;

                if ((bool)selectedDish.isHit)
                    HitB.Visibility = Visibility.Visible;


                WeightDishLB.Content = selectedDish.WeightDishGR;
                WeightDishLB.Content += selectedDish.DishCategory.NameCategory == "Напитки" ? " мл." : " г.";


            }
            catch (Exception ex)
            {

                MBClass.Error(ex, "Ошибка загрузки данных");

                menuListPage.IsEnabled = true;
                AEFrame.Navigate(null);
            }


        }


        private void LoadData(bool isKrosha, bool isBig)
        {
            var Fillers = DBEntities.GetContext().Menu.Where(u => u.DishCategory.NameCategory.ToUpper().Contains("НАПОЛНИТЕЛ")).ToArray();

            fillerItemsOrders = new List<FillerItemsOrderClass>();

            FillerItemsOrderClass Fitem = new FillerItemsOrderClass()
            {
                Photo = null,
                FillerName = "Отсутствует",
                Price = 0
            };

            fillerItemsOrders.Add(Fitem);


            if (Fillers != null)
            {
                foreach (var item in Fillers)
                {

                    Fitem = new FillerItemsOrderClass();

                    Fitem.Photo = item.Photo;
                    Fitem.FillerName = item.NameDish;

                    if (!isKrosha)
                    {
                        if (item.Price <= 109)
                        {
                            Fitem.Price = 0;
                        }
                        else
                        {
                            Fitem.Price = item.Price / 2;
                        }
                    }
                    else
                    {
                        Fitem.Price = item.Price;
                    }

                    fillerItemsOrders.Add(Fitem);

                }

                FirstFillerCB.ItemsSource = fillerItemsOrders.ToList().OrderBy(u => u.Price);

                if (isBig)
                {
                    SecondFillerCB.ItemsSource = fillerItemsOrders.ToList().OrderBy(u => u.Price);
                    SecondFillerCB.SelectedIndex = 0;
                }

                FirstFillerCB.SelectedIndex = 0;
            }

            isLoad = true;

        }


        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DateTime.Now.TimeOfDay > blockExit)
            {
                menuListPage.IsEnabled = true;
                AEFrame.Navigate(null);

            }
        }



        private void IncrementAmountLB_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int amount = Convert.ToInt32(AmountTB.Text);
            AmountTB.Text = (++amount).ToString();

            AmountTB.CaretIndex = AmountTB.Text.Length;

        }

        private void AmountTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.OnlyNumsTB();
        }

        private void AmountTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(AmountTB.Text) >= 999)
                {
                    AmountTB.Text = "999";
                }

                if (Convert.ToInt32(AmountTB.Text) < 1)
                {
                    AmountTB.Text = "1";
                }
            }
            catch
            {
                AmountTB.Text = "1";
            }

            CountPriceOverAll();
        }


        private void EnableButton()
        {
            if (isMash)
            {

                if (FirstFillerCB.Visibility == Visibility.Visible && SecondFillerCB.Visibility == Visibility.Visible)
                {

                    AddToOrderBtn.IsEnabled = SecondFillerCB.SelectedIndex > 0 && FirstFillerCB.SelectedIndex > 0;

                }
                else if (FirstFillerCB.Visibility == Visibility.Visible)
                {
                    AddToOrderBtn.IsEnabled = (FirstFillerCB.SelectedIndex > 0);
                }
            }
        }



        private void DecrementAmountLB_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int amount = Convert.ToInt32(AmountTB.Text);
            AmountTB.Text = (--amount).ToString();

            AmountTB.CaretIndex = AmountTB.Text.Length;
        }

        private void AddToOrderBtn_Click(object sender, RoutedEventArgs e)
        {

            PlacingOrderClass Item = new PlacingOrderClass();

            Item.DishOrder = selectedDish.NameDish;



            if (AddiPanelSP.Visibility == Visibility.Visible)
            {
                if (PetrIngrChB.IsChecked == true ||
                    ButrIngrChB.IsChecked == true ||
                    UkropIngrChB.IsChecked == true ||
                    ChesseIngrChB.IsChecked == true ||
                    OnionAdditIngrChB.IsChecked == true)
                {
                    Item.DishOrder += "\n  Доп. ингр.:";
                }

                if (PetrIngrChB.IsChecked == true)
                    Item.DishOrder += "\n - Петрушка;";
                if (ButrIngrChB.IsChecked == true)
                    Item.DishOrder += "\n - Сливочное масло;";
                if (UkropIngrChB.IsChecked == true)
                    Item.DishOrder += "\n - Укроп с раст.маслом;";
                if (ChesseIngrChB.IsChecked == true)
                    Item.DishOrder += "\n - Тертый сыр";
                if (OnionAdditIngrChB.IsChecked == true)
                    Item.DishOrder += "\n - Лук фри";
            }

            if ((FirstFillerCB.Visibility == Visibility.Visible && FirstFillerCB.SelectedIndex > 0) ||
               (SecondFillerCB.Visibility == Visibility.Visible && SecondFillerCB.SelectedIndex > 0))
            {
                Item.DishOrder += "\nНаполнители:";

                if (FirstFillerCB.SelectedIndex > 0)
                {
                    Item.DishOrder += "\n - " + (FirstFillerCB.SelectedItem as FillerItemsOrderClass).FillerName;
                }
                if (SecondFillerCB.SelectedIndex > 0)
                {
                    Item.DishOrder += "\n - " + (SecondFillerCB.SelectedItem as FillerItemsOrderClass).FillerName;
                }
            }





            Item.AmountOrder = Convert.ToInt32(AmountTB.Text);
            Item.PriceOrderOne = selectedDish.Price * Item.AmountOrder;

            if (menuListPage.PODGcheckBeforeAdd(Item))
            {
                PlacinOrderDG.Items.Add(Item);
            }



            menuListPage.PODGrefreshData();

            menuListPage.IsEnabled = true;
            AEFrame.Navigate(null);

        }

        private void PetrIngrChB_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void AddIngridChB_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;

            if (checkBox.IsChecked == true)
            {
                DishPriceOne += 45;
            }
            else
            {
                DishPriceOne -= 45;
            }

            CountPriceOverAll();
        }


        private void CountPriceOverAll()
        {
            DishPriceOne = DishPriceOneCopy;


            if (PetrIngrChB.IsChecked == true)
                DishPriceOne += 45;
            if (ButrIngrChB.IsChecked == true)
                DishPriceOne += 45;
            if (UkropIngrChB.IsChecked == true)
                DishPriceOne += 45;
            if (ChesseIngrChB.IsChecked == true)
                DishPriceOne += 45;
            if (OnionAdditIngrChB.IsChecked == true)
                DishPriceOne += 45;

            if (FirstFillerCB.SelectedItem != null)
                DishPriceOne += (FirstFillerCB.SelectedItem as FillerItemsOrderClass).Price;

            if (SecondFillerCB.SelectedItem != null)
                DishPriceOne += (SecondFillerCB.SelectedItem as FillerItemsOrderClass).Price;

            PriceLB.Content = (DishPriceOne * Convert.ToInt32(AmountTB.Text)).ToString();
        }


        private void FillerCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isLoad)
            {
                EnableButton();
                CountPriceOverAll();
            }

        }
    }
}
