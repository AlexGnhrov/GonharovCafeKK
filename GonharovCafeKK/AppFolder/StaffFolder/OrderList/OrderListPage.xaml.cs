using GonharovCafeKK.AppFolder.AdminFolder;
using GonharovCafeKK.AppFolder.EntityFolder;
using GonharovCafeKK.GlobalClassFolder;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TC_Application.AppFolder.GlobalClassFolder;

namespace GonharovCafeKK.AppFolder
{
    /// <summary>
    /// Логика взаимодействия для OrderListPage.xaml
    /// </summary>
    public partial class OrderListPage : Page
    {
        public Frame AEFrame;

        private bool loaded = false;

        public OrderListPage(Frame AEFrame)
        {
            InitializeComponent();

            this.AEFrame = AEFrame;

            SortCB.Text = "Сегодня";

            if (GlobalVariablesClass.currentRoleID == 2)
            {
                SeparatorMI.Visibility = Visibility.Collapsed;
                RemoveMI.Visibility = Visibility.Collapsed;
            }

            UpdateList();
            loaded = true;
        }


        private void UpdateList()
        {


            IQueryable<Order> sortList = DBEntities.GetContext().Order;

            SortselectedCombobox(ref sortList);


            sortList = sortList.Where(u => u.User.Surname.Contains(SearchTB.Text)
                              || u.User.Name.Contains(SearchTB.Text)
                              || u.User.Patronymic.Contains(SearchTB.Text)
                              || u.StatusOrder.NameStatus.Contains(SearchTB.Text));

            sortList = sortList.OrderByDescending(u => u.OrderID);

            OrderListDG.ItemsSource = sortList.ToList();



        }


        private void SortselectedCombobox(ref IQueryable<Order> sortList)
        {
            DateTime selectedDate = DateTime.Now.Date;
            DateTime selectedDate2 = selectedDate.AddDays(1).Date;



            switch (SortCB.Text.Trim())
            {
                case "Сегодня":
                    sortList = sortList.Where(u => u.DateOfReceiving >= selectedDate && u.DateOfReceiving <= selectedDate2);
                    break;
                case "За неделю":
                    selectedDate = selectedDate.AddDays(-7).Date;

                    sortList = sortList.Where(u => u.DateOfReceiving >= selectedDate && u.DateOfReceiving <= selectedDate2);
                    break;
                case "За месяц":
                    selectedDate = selectedDate.AddMonths(-1).Date;

                    sortList = sortList.Where(u => u.DateOfReceiving >= selectedDate && u.DateOfReceiving <= selectedDate2);
                    break;
                case "За прошлый месяц":
                    selectedDate = selectedDate.AddMonths(-2).Date;
                    selectedDate2 = selectedDate2.AddMonths(-1).Date;

                    sortList = sortList.Where(u => u.DateOfReceiving >= selectedDate && u.DateOfReceiving <= selectedDate2);
                    break;
            }
        }


        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTB.Text))
            {
                RemoveSearchTextLB.Visibility = Visibility.Hidden;
            }
            else
            {
                RemoveSearchTextLB.Visibility = Visibility.Visible;
            }
        }

        private async void SortCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (loaded)
            {
                await Task.Delay(1);

                UpdateList();
            }
        }




        private void RemoveMI_Click(object sender, RoutedEventArgs e)
        {
            if (MBClass.Question("Вы действительно хотите удалить этот заказ?"))
            {
                DBEntities.GetContext().Order.Remove(OrderListDG.SelectedItem as Order);

                DBEntities.GetContext().SaveChanges();

                UpdateList();
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
            if (e.Key == Key.Enter)
            {
                SearchBT_Click(sender, e);
            }
        }

        private void StatusIssuieMI_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MBClass.Question("Сменить статус далее будет невозможно." +
                                    "Продолжить?"))
                {

                    Order order = OrderListDG.SelectedItem as Order;

                    order.DateOfIssue = DateTime.Now;
                    order.StatusID = 1;

                    DBEntities.GetContext().SaveChanges();

                    UpdateList();
                }
            }
            catch (Exception ex)
            {

                MBClass.Error(ex, "Ошибка смены статуса");
            }
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {


            StatusMI.IsEnabled = false;
            RemoveMI.IsEnabled = false;




            if (OrderListDG.SelectedItem != null)
            {

                StatusMI.IsEnabled = OrderListDG.SelectedItem != null;
                RemoveMI.IsEnabled = OrderListDG.SelectedItem != null;



                if ((OrderListDG.SelectedItem as Order).StatusID == 1 || (OrderListDG.SelectedItem as Order).StatusID == 3)
                {
                    StatusMI.IsEnabled = false;
                }


            }
        }


        private void StatusCanceledMI_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (MBClass.Question("Вы действительно хотите отменить заказ?\n" +
                 "Сменить статус далее будет невозможно."))
                {
                    (OrderListDG.SelectedItem as Order).StatusID = 3;

                    DBEntities.GetContext().SaveChanges();

                    UpdateList();
                }
            }
            catch (Exception ex)
            {

                MBClass.Error(ex, "Ошибка смены статуса");
            }
        }
    }
}