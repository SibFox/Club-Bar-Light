using BarLight.Common;
using BarLight.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BarLight.Pages
{
    public partial class P_Profile : Page
    {
        Client user = null;
        public P_Profile()
        {
            InitializeComponent();
            try
            {
                user = Manager.Context.Clients.Where(c => c.ID == Manager.ClientID).FirstOrDefault();
                if (user == null)
                {
                    MessageBox.Show("Ошибка в получении данных профиля", "Ошибка");
                    Manager.ClientID = 0;
                    Manager.FrameContainer_First.Navigate(new P_Authorization());
                    return;
                }
                double summOfOrders = 0;
                foreach (Order item in user.Orders)
                    summOfOrders += Convert.ToDouble(item.Price);
                int discount = (int)(summOfOrders / 5000) + 1;
                if (discount > 30) discount = 30;
                if (discount < 1) discount = 1;
                user.Discount = discount;
                Manager.Context.SaveChanges();
                txtBlockFIO.DataContext = user;
                txtBlockPhoneNum.DataContext = user;
                if (user.City != 0 && user.Adress != null)
                    txtBlockAdress.Text = $"{user.Cities.Name}, {user.Adress}";
                txtBlockEMail.DataContext = user;
                txtBlockDiscount.DataContext = user;
                txtBlockSumm.Text = $"В общей сумме вы сделали заказы на: {summOfOrders}руб.";
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке профиля\n~~~~~\n" + ex.ToString(), "Ошибка");
                Manager.ClientID = 0;
                Manager.FrameContainer_First.Navigate(new P_Authorization());
            }
            
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            Manager.ClientID = 0;
            Manager.FrameContainer_First.Navigate(new P_Authorization());
            Manager.ChangePFButton("Войти в профиль");
        }

        private void btnRedAcc_Click(object sender, RoutedEventArgs e)
        {
            Manager.FrameContainer_First.Navigate(new P_ProfileRedacting());
        }

        private void btnShowOrders_Click(object sender, RoutedEventArgs e)
        {
            Manager.FrameContainer_First.Navigate(new P_MadeOrders());
        }
    }
}
