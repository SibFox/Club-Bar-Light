using BarLight.Common;
using System.Windows;
using BarLight.Pages;
using BarLight.Windows;

namespace BarLight
{
    public partial class W_Main : Window
    {
        public W_Main()
        {
            InitializeComponent();
            Manager.MainFrame = MainFrame;
            Manager.MainFrame.Navigate(new P_Start());
            Manager.CartAmmount = txtBlockCartAmmount;
        }

        private void btnCart_Click(object sender, RoutedEventArgs e)
        {
            Manager.FrameContainer_First.Navigate(new P_Cart());
        }

        private void btnToConverter_Click(object sender, RoutedEventArgs e)
        {
            new W_ImageConvert().Show();
        }
    }
}
