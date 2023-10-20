using GonharovCafeKK.AppFolder.EntityFolder;
using GonharovCafeKK.AppFolder.StaffFolder.MenuList;
using GonharovCafeKK.GlobalClassFolder;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TC_Application.AppFolder.GlobalClassFolder;

namespace GonharovCafeKK.AppFolder.MenuList
{
    /// <summary>
    /// Логика взаимодействия для MenuListPage.xaml
    /// </summary>
    public partial class MenuListPage : Page
    {
        ListView[] listViews = new ListView[10];

        MenuItem[] infoMI = new MenuItem[10];
        MenuItem[] editMI = new MenuItem[10];
        MenuItem[] removeMI = new MenuItem[10];
        Separator[] separatorsMI = new Separator[10];

        StackPanel[] stackPanelsMenus = new StackPanel[10];

        Frame AEFrame;
        //PlacingOrderClass placingOrderClass;

        public MenuListPage(Frame AEFrame)
        {
            this.AEFrame = AEFrame;

            InitializeComponent();

            PlacinOrderB.Visibility = Visibility.Collapsed;

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

            infoMI[0] = GrillChessInfoMI;
            infoMI[1] = SeasonnInfoMI;
            infoMI[2] = KroshkaKartoskaInfoMI;
            infoMI[3] = FillerInfoMI;
            infoMI[4] = SaladInfoMI;
            infoMI[5] = SoupAndHotInfoMI;
            infoMI[6] = SnackInfoMI;
            infoMI[7] = SandvichesAndRollsInfoMI;
            infoMI[8] = DesertesInfoMI;
            infoMI[9] = DrinksInfoMI;

            editMI[0] = GrillChessEditMI;
            editMI[1] = SeasonEditMI;
            editMI[2] = KroshkaKartoskaEditMI;
            editMI[3] = FillerEditMI;
            editMI[4] = SaladEditMI;
            editMI[5] = SoupAndHotEditMI;
            editMI[6] = SnacksEditMI;
            editMI[7] = SandvichesAndRollsEditMI;
            editMI[8] = DesertesEditMI;
            editMI[9] = DrinksEditMI;

            separatorsMI[0] = SeparatorGrillChess;
            separatorsMI[1] = SeparatorSeason;
            separatorsMI[2] = SeparatorKroshkaKartoska;
            separatorsMI[3] = SeparatorFiller;
            separatorsMI[4] = SeparatorSalad;
            separatorsMI[5] = SeparatorSoupAndHot;
            separatorsMI[6] = SeparatorSnacks;
            separatorsMI[7] = SeparatorSandvichesAndRolls;
            separatorsMI[8] = SeparatorDesertes;
            separatorsMI[9] = SeparatorDrinks;

            removeMI[0] = GrillChessRemoveMI;
            removeMI[1] = SeasonRemoveMI;
            removeMI[2] = KroshkaKartoskaRemoveMI;
            removeMI[3] = FillerRemoveMI;
            removeMI[4] = SaladRemoveMI;
            removeMI[5] = SoupAndHotRemoveMI;
            removeMI[6] = SnacksRemoveMI;
            removeMI[7] = SandvichesAndRollsRemoveMI;
            removeMI[8] = DesertesRemoveMI;
            removeMI[9] = DrinksRemoveMI;

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

            if (GlobalVariablesClass.currentRoleID == 2)
            {
                AddDishBtn.Visibility = Visibility.Collapsed;

                for (int i = 0; i < removeMI.Length; i++)
                {
                    editMI[i].Visibility = Visibility.Collapsed;
                    removeMI[i].Visibility = Visibility.Collapsed;
                    separatorsMI[i].Visibility = Visibility.Collapsed;
                }
            }

            UpdateList();


        }



        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                ScrollSV.LineUp();
                ScrollSV.LineUp();
                ScrollSV.LineUp();
                ScrollSV.LineUp();
            }

            else if (e.Delta < 0)
            {
                ScrollSV.LineDown();
                ScrollSV.LineDown();
                ScrollSV.LineDown();
                ScrollSV.LineDown();
            }

            e.Handled = true;
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




                GrillChesseLV.ItemsSource = data.ToList().Where(u => u.DishCategoryID == 9);
                SeasonLV.ItemsSource = data.ToList().Where(u => u.isSeason == true);
                KroshkaKartoskaLV.ItemsSource = data.ToList().Where(u => u.DishCategoryID == 1);
                FillerLV.ItemsSource = data.ToList().Where(u => u.DishCategoryID == 2);
                SaladLV.ItemsSource = data.ToList().Where(u => u.DishCategoryID == 3);
                SoupAndHotLV.ItemsSource = data.ToList().Where(u => u.DishCategoryID == 4);
                SnacksLV.ItemsSource = data.ToList().Where(u => u.DishCategoryID == 5);
                SandvichesAndRollsLV.ItemsSource = data.ToList().Where(u => u.DishCategoryID == 6);
                DesertesLV.ItemsSource = data.ToList().Where(u => u.DishCategoryID == 7);
                DrinksLV.ItemsSource = data.ToList().Where(u => u.DishCategoryID == 8);

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


        private void SearchTB_KeyUp(object sender, KeyEventArgs e)
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

            AEFrame.Navigate(new PreviewOrderPage(AEFrame, this, selectedDish, PlacinOrderDG));
        }

        private void AddDishBtn_Click(object sender, RoutedEventArgs e)
        {
            AEFrame.Navigate(new AEDishPage(AEFrame, this, null));
        }

        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SearchTB.Text = "";

            UpdateList();
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(SearchTB.Text))
            {
                RemoveSearchTextLB.Visibility = Visibility.Visible;
            }
            else
            {
                RemoveSearchTextLB.Visibility = Visibility.Collapsed;
            }
        }

        private void InfoMenu_Click(object sender, RoutedEventArgs e)
        {
            EntityFolder.Menu selectedDish = selectedItem as EntityFolder.Menu;

            AEFrame.Navigate(new DishInfoPage(AEFrame, this, selectedDish));
        }


        private void DishInfoLV_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                object selectedItem;

                CheckItemForNull(out selectedItem);

                if (selectedItem == null) return;


                EntityFolder.Menu selectedDish = selectedItem as EntityFolder.Menu;

                AEFrame.Navigate(new DishInfoPage(AEFrame, this, selectedDish));

            }
        }






        object selectedItem;

        private void ListView_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            selectedItem = null;

            for (int i = 0; i < removeMI.Length; i++)
            {
                editMI[i].IsEnabled = false;
                removeMI[i].IsEnabled = false;
                infoMI[i].IsEnabled = false;
            }

            CheckItemForNull(out selectedItem);

            if (selectedItem == null) return;

            for (int i = 0; i < removeMI.Length; i++)
            {
                editMI[i].IsEnabled = true;
                removeMI[i].IsEnabled = true;
                infoMI[i].IsEnabled = true;
            }

        }

        private void EditMenu_Click(object sender, RoutedEventArgs e)
        {

            EntityFolder.Menu item = selectedItem as EntityFolder.Menu;

            AEFrame.Navigate(new AEDishPage(AEFrame, this, item));

        }

        private void RemoveMenu_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (MBClass.Question("Вы действительно хотите удалить это блюдо?"))
                {

                    EntityFolder.Menu selectedDish = selectedItem as EntityFolder.Menu;

                    DBEntities.GetContext().Menu.Remove(selectedDish);

                    DBEntities.GetContext().SaveChanges();


                    UpdateList();
                }
            }
            catch (Exception ex)
            {
                MBClass.Error(ex, "Ошибка при удалени.");
            }
        }


        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {


        }



        //-------------Методы для списка заказов-------------


        int Price = 0;
        public void PODGrefreshData()
        {
            if (PlacinOrderDG.Items.Count <= 0)
            {
                PlacinOrderB.Visibility = Visibility.Collapsed;
                GlobalVariablesClass.isOrdering = false;
            }
            else
            {
                PlacinOrderB.Visibility = Visibility.Visible;
                GlobalVariablesClass.isOrdering = true;

            }

            Price = PlacinOrderDG.Items.OfType<PlacingOrderClass>().Sum(item => item.PriceOrder);
            PriceOverAllLB.Content = $"{Price} ₽";

        }

        public bool PODGcheckBeforeAdd(PlacingOrderClass checkeditem)
        {


            foreach (var item in PlacinOrderDG.Items.OfType<PlacingOrderClass>())
            {

                if (item.DishOrder == checkeditem.DishOrder)
                {

                    item.AmountOrder += checkeditem.AmountOrder;

                    if (item.AmountOrder > 999)
                    {
                        item.AmountOrder = 999;
                    }


                    PlacinOrderDG.Items.Refresh();
                    return false;
                }
            }

            return true;

        }

        private void ClearDishFromOrder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PlacinOrderDG.Items.Remove(PlacinOrderDG.SelectedItem);
            PODGrefreshData();
        }

        private void CancelPlacinOrder_Click(object sender, RoutedEventArgs e)
        {
            PlacinOrderDG.Items.Clear();
            PODGrefreshData();
        }

        private void AddOrderBtn_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (MBClass.Question("Вы действительно хотите оформить заказ?"))
                {
                    Order order = new Order();

                    string Dishes = "";

                    for (int i = 0; i < PlacinOrderDG.Items.Count; i++)
                    {
                        var item = PlacinOrderDG.Items[i] as PlacingOrderClass;

                        Dishes += $"• {i + 1}: {item.DishOrder}";


                        Dishes += $"\n({item.PriceOrder}₽x{item.AmountOrder} = {item.PriceOrder * item.AmountOrder}₽)"; ;


                        if (i < PlacinOrderDG.Items.Count - 1)
                        {
                            Dishes += "\n";
                        }

                    }

                    Random r = new Random();

                    string firstCH = Convert.ToChar(r.Next(65, 91)).ToString();
                    string SecondCH = Convert.ToChar(r.Next(65, 91)).ToString();

                    order.NumOrder = firstCH + SecondCH + "-" + r.Next(1, 100).ToString();
                    order.DishOrder = Dishes;
                    order.Price = Price;
                    order.DateOfReceiving = DateTime.Now;
                    order.UserID = GlobalVariablesClass.currentUserID;
                    order.StatusID = 2;

                    DBEntities.GetContext().Order.Add(order);

                    DBEntities.GetContext().SaveChanges();


                    MBClass.Info("Заказ успешно добавлен");


                    PlacinOrderDG.Items.Clear();
                    PODGrefreshData();
                }
            }
            catch (Exception ex)
            {

                MBClass.Error(ex, "Ошибка при добавлении заказа");
            }

        }


        private void AmoutOrder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;

            PlacingOrderClass item = PlacinOrderDG.SelectedItem as PlacingOrderClass;

            if (textBlock.Name == "IncrOrderTbl")
            {
                ++item.AmountOrder;

                if (item.AmountOrder > 999)
                {

                    item.AmountOrder = 999;
                }
            }
            else
            {
                --item.AmountOrder;

                if (item.AmountOrder < 1)
                {
                    item.AmountOrder = 1;
                }

            }



            PlacinOrderDG.Items.Refresh();
        }

    }
}
