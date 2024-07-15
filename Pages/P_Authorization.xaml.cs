using BarLight.Common;
using BarLight.Model;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BarLight.Pages
{
    public partial class P_Authorization : Page
    {
        public P_Authorization()
        {
            InitializeComponent();
        }

        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            Client user = Manager.Context.Clients.Where(c => c.Login == txtBoxLogin.Text).Where(c => c.Password == txtBoxPassword.Text).ToList().FirstOrDefault();
            if (user == null)
            {
                txtBlockAccNotFound.Visibility = Visibility.Visible;
                return;
            }

            Manager.ClientID = user.ID;

            List<ClientCart> clientCart = Manager.Context.ClientCarts.Where(cc => cc.ClientID == Manager.ClientID).ToList();
            Manager.Cart.Clear();
            foreach (ClientCart c in clientCart)
            {
                Manager.Cart.Add(c);
            }

            int? cartAmmText = 0;
            foreach (ClientCart item in Manager.Cart)
            {
                cartAmmText += item.Quanity == null ? 0 : item.Quanity;
            }
            Manager.CartAmmount.Text = cartAmmText.ToString();
            
            Manager.ChangePFButton("Профиль");
            Manager.FrameContainer_First.Navigate(new P_Profile());
        }
    }
}
