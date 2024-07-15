using BarLight.Common;
using BarLight.Model;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BarLight.Pages
{
    public partial class P_MadeOrders : Page
    {
        public P_MadeOrders()
        {
            InitializeComponent();
            List<Order> orders = new List<Order>(Manager.Context.Orders.Where(o => o.ClientID == Manager.ClientID).ToList());
            lstViewMadeOrders.ItemsSource = orders;
            Manager.FrameContainer_First.Refresh();
        }

        private void btnGoBack_Click(object sender, RoutedEventArgs e)
        {
            Manager.FrameContainer_First.Navigate(new P_Profile());
        }

        private void btnSeeOrder_Click(object sender, RoutedEventArgs e)
        {
            Order choosenOrder = (sender as Button).DataContext as Order;
            Manager.FrameContainer_First.Navigate(new P_SeeOrder(choosenOrder));
        }

        private void lstViewMadeOrders_Loaded(object sender, RoutedEventArgs e)
        {
            lstViewMadeOrders.Items.Refresh();
        }
    }
}
