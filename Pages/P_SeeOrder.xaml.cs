using BarLight.Common;
using BarLight.Model;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BarLight.Pages
{
    public partial class P_SeeOrder : Page
    {
        private Order _choosenOrder;

        public P_SeeOrder(Order choosenOrder)
        {
            InitializeComponent();

            _choosenOrder = choosenOrder;

            if (choosenOrder.Status == 2 || choosenOrder.Status == 3)
            {
                gridButoons.Visibility = Visibility.Hidden;
            }

            txtBlockOrderNum.Text = $"Заказ №: {choosenOrder.EntryID:0000000}";
            txtBlockOrderPrice.Text = $"Общая цена: {choosenOrder.Price:N2}руб.";
            lstViewOrder.ItemsSource = Manager.Context.OrderCarts.Where(oc => oc.OrderID == choosenOrder.EntryID).ToList();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Manager.FrameContainer_First.Navigate(new P_MadeOrders());
        }

        private void btnConfirmOrder_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Подтвердить получение заказа?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _choosenOrder.Status = 2;
                Manager.Context.SaveChanges();
                _choosenOrder.OrderStatus = Manager.Context.OrderStatuses.Where(o => o.StatusID == 2).First();
                Manager.Context.SaveChanges();
                gridButoons.Visibility = Visibility.Hidden;
            }
        }

        private void btnCancelOrder_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Подтвердить отмену заказа?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _choosenOrder.Status = 3;
                Manager.Context.SaveChanges();
                _choosenOrder.OrderStatus = Manager.Context.OrderStatuses.Where(o => o.StatusID == 3).First();
                Manager.Context.SaveChanges();
                gridButoons.Visibility = Visibility.Hidden;
            }
        }
    }
}
