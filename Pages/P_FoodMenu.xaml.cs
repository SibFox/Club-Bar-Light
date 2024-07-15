using BarLight.Common;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BarLight.Model;
using System.Diagnostics;

namespace BarLight.Pages
{
    public partial class P_FoodMenu : Page
    {
        private int filterBy = 0;
        private static List<FoodMenu> _foodMenu = null;
        public P_FoodMenu()
        {
            InitializeComponent();
            if (_foodMenu == null)
            {
                Debug.WriteLine("Loaded Food Menu Info");
                _foodMenu = Manager.Context.FoodMenu.ToList();
            }
            FilterFood();
        }

        private void btnAddToCart_Click(object sender, RoutedEventArgs e)
        {
            FoodMenu food = (sender as Button).DataContext as FoodMenu;

            ClientCart foodItem = Manager.Cart.Where(cc => cc.FoodID == food.ID).FirstOrDefault();
            ClientCart clientFoodItem = null;
            if (Manager.ClientID > 0)
            {
                clientFoodItem = Manager.Context.ClientCarts.Where(c => c.ClientID == 
                Manager.ClientID).Where(cc => cc.FoodID == food.ID).FirstOrDefault();
            }
            if (foodItem == null)
            {
                foodItem = new ClientCart()
                {
                    ClientID = Manager.ClientID,
                    FoodID = food.ID,
                    Quanity = 1,
                    FoodMenu = Manager.Context.FoodMenu.Where(fm => fm.ID == food.ID).FirstOrDefault()
                };
                Manager.Cart.Add(foodItem);
                
                if (Manager.ClientID > 0)
                {
                    Manager.Context.ClientCarts.Add(foodItem);
                }
            }
            else
            {
                if (clientFoodItem != null) clientFoodItem.Quanity++;
                else foodItem.Quanity++;
            }

            int? cartAmmText = 0;
            foreach (ClientCart item in Manager.Cart)
            {
                cartAmmText += item.Quanity == null ? 0 : item.Quanity;
            }
            Manager.CartAmmount.Text = cartAmmText.ToString();

            Manager.Context.SaveChanges();
            Debug.WriteLine($"{foodItem.ClientID} {foodItem.FoodID} {foodItem.Quanity}");
        }

        private void FilterByAll(object sender, RoutedEventArgs e)
        {
            filterBy = 0;
            FilterFood();
        }

        private void FilterBySnaks(object sender, RoutedEventArgs e)
        {
            filterBy = 1;
            FilterFood();
        }

        private void FilterBySalads(object sender, RoutedEventArgs e)
        {
            filterBy = 2;
            FilterFood();
        }

        private void FilterBySoups(object sender, RoutedEventArgs e)
        {
            filterBy = 3;
            FilterFood();
        }

        private void FilterByHot(object sender, RoutedEventArgs e)
        {
            filterBy = 4;
            FilterFood();
        }

        private void FilterByDrinks(object sender, RoutedEventArgs e)
        {
            filterBy = 5;
            FilterFood();
        }

        void FilterFood()
        {
            List<FoodMenu> menu = new List<FoodMenu>();
            List<FoodMenu> filtered = new List<FoodMenu>();
            if (filterBy > 0)
                filtered = _foodMenu.Where(f => f.Category == filterBy).ToList();
            else
                filtered = _foodMenu;
            lstViewMenu.ItemsSource = filtered.OrderBy(p => p.Category);
            lstViewMenu.Items.Refresh();
        }
    }
}
