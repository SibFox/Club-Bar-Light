using BarLight.Common;
using System.Windows;
using System.Windows.Controls;

namespace BarLight.Pages
{
    public partial class P_Start : Page
    {
        public P_Start()
        {
            InitializeComponent();
            Manager.FrameContainer_First = StartPageFrame;
            Manager.FrameContainer_First.Navigate(new P_FoodMenu());
        }

        private void btnShowMenu_Click(object sender, RoutedEventArgs e)
        {
            Manager.FrameContainer_First.Navigate(new P_FoodMenu());
        }

        private void btnShowReservation_Click(object sender, RoutedEventArgs e)
        {
            Manager.FrameContainer_First.Navigate(new P_TableReservation());
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (Manager.ClientID < 1)
                Manager.FrameContainer_First.Navigate(new P_Authorization());
            else
                Manager.FrameContainer_First.Navigate(new P_Profile());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Manager.ProfileButton = btnLogin;
            if (Manager.ClientID > 0)
                Manager.ChangePFButton("Профиль");
        }
    }
}
