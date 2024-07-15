using BarLight.Common;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BarLight.Model;
using System.Data.Entity;
using System.Diagnostics;

namespace BarLight.Pages
{
    /// <summary>
    /// Логика взаимодействия для P_Cart.xaml
    /// </summary>
    public partial class P_Cart : Page
    {
        public P_Cart()
        {
            InitializeComponent();
            lstViewCart.ItemsSource = Manager.Cart;
            txtBlockPrice.Text = "На сумму: " + GetOverallPrice().ToString() + "руб.";
        }

        private void btnIncreaseFromCart_Click(object sender, RoutedEventArgs e)
        {
            ClientCart foodItem = (sender as Button).DataContext as ClientCart; // Сам нажатый предмет
            ClientCart cartItem = Manager.Cart.Where(c => c == foodItem).FirstOrDefault(); // Ссылка на него в корзине

            
            if (Manager.ClientID > 0)
            {
                DbSet<ClientCart> cart = Manager.Context.ClientCarts;
                cart.Where(cc => cc.ClientID == Manager.ClientID).Where(f => f.FoodID == foodItem.FoodID).FirstOrDefault().Quanity++;
                Manager.Context.SaveChanges();
            }
            else
            {
                cartItem.Quanity++;
            }

            UpdateGraph(foodItem);
        }

        private void btnDecreaseFromCart_Click(object sender, RoutedEventArgs e)
        {
            ClientCart foodItem = (sender as Button).DataContext as ClientCart; // Сам нажатый предмет
            ClientCart cartItem = Manager.Cart.Where(c => c == foodItem).FirstOrDefault(); // Ссылка на него в корзине

            if (foodItem.Quanity > 1)
            {
                if (Manager.ClientID > 0)
                {
                    DbSet<ClientCart> cart = Manager.Context.ClientCarts;
                    cart.Where(cc => cc.ClientID == Manager.ClientID).Where(f => f.FoodID == foodItem.FoodID).FirstOrDefault().Quanity--;
                    Manager.Context.SaveChanges();
                }
                else
                {
                    cartItem.Quanity--;
                }
            }
            else if (foodItem.Quanity <= 1)
            {
                Manager.Cart.Remove(foodItem);
                if (Manager.ClientID > 0)
                {
                    DbSet<ClientCart> cart = Manager.Context.ClientCarts;
                    cart.Remove(cart.Where(cc => cc.ClientID == Manager.ClientID).Where(f => f.FoodID == foodItem.FoodID).FirstOrDefault());
                    Manager.Context.SaveChanges();
                }
            }

            UpdateGraph(foodItem);
        }

        void UpdateGraph(ClientCart foodItem)
        {
            int? cartAmmText = 0;
            foreach (ClientCart item in Manager.Cart)
            {
                cartAmmText += item.Quanity == null ? 0: item.Quanity;
            }
            Manager.CartAmmount.Text = cartAmmText.ToString();

            txtBlockPrice.Text = "На сумму: " + GetOverallPrice().ToString() + "руб.";
            lstViewCart.Items.Refresh();
        }


        double GetOverallPrice()
        {
            double price = 0;
            foreach (ClientCart item in Manager.Cart)
            {
                price += Convert.ToDouble(Manager.Context.FoodMenu.Where(f => f.ID == item.FoodID).FirstOrDefault().Price * item.Quanity);
            }
            //foreach(FoodMenu item in Manager.Cart)
            //    price += Convert.ToDouble(item.Price);
            Manager.CartPrice = Math.Round(price, 2);
            return Math.Round(price, 2);
        }

        private void btnMakeOrder_Click(object sender, RoutedEventArgs e)
        {
            if (Manager.Cart.Count < 1)
            {
                MessageBox.Show("Ваша корзина пуста");
                return;
            }
            Manager.FrameContainer_First.Navigate(new P_Order());
        }

        
    }
}
