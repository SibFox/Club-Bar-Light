using BarLight.Common;
using BarLight.Model;
using BarLight.Windows;
using System;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BarLight.Pages
{
    public partial class P_Order : Page
    {
        private int paymentMethod = 0;
        public int discount = 0;
        double priceEnd = Manager.CartPrice;
        public P_Order()
        {
            InitializeComponent();
            txtBlockPrice.Text = $"Заказ на сумму: {Manager.CartPrice:N2}руб.";
            rBtnOnline.IsChecked = true;

            if (Manager.ClientID > 0)
            {
                Client user = Manager.Context.Clients.Where(c => c.ID == Manager.ClientID).FirstOrDefault();
                double summOfOrders = 0.0;
                foreach (Order item in user.Orders)
                    summOfOrders += Convert.ToDouble(item.Price);
                int discount = (int)(summOfOrders / 5000) + 1;
                if (discount > 30) discount = 30;
                if (discount < 1) discount = 1;
                user.Discount = discount;
                Manager.Context.SaveChanges();

                txtBoxName.Text = user.Name;
                txtBoxPhoneNum.Text = user.PhoneNum;
                txtBoxAdress.Text = user.Adress;
                cmbBoxCity.SelectedIndex = (int)(user.City != null ? user.City - 1 : -1);
                txtBlockDiscount.Text = $"Ваша скидка: {user.Discount}%";
                priceEnd *= 1 - user.Discount * 0.01;
            }
            txtBlockOverallPrice.Text = $"Итоговая сумма: {priceEnd:N2}руб.";
        }

        private void btnCancelOrder_Click(object sender, RoutedEventArgs e)
        {
            Manager.FrameContainer_First.Navigate(new P_Cart());
        }

        private void btnToPayment_Click(object sender, RoutedEventArgs e)
        {
            if (txtBoxName.Text.Trim().Replace(" ", "").Length < 1 ||
                txtBoxPhoneNum.Text.Trim().Replace(" ", "").Length < 1 ||
                txtBoxAdress.Text.Trim().Replace(" ", "").Length < 1 ||
                cmbBoxCity.SelectedIndex < 0)
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            if (txtBoxPhoneNum.Text.Length != 10 && txtBoxPhoneNum.Text.Length > 0)
            {
                MessageBox.Show("Номер телефона введён не верно");
                return;
            }

            bool online = (bool)rBtnOnline.IsChecked;
            bool cardOH = (bool)rBtnCardOnHands.IsChecked;
            bool cashOH = (bool)rBtnCashOnHands.IsChecked;

            if (online || cardOH || cashOH)
            {
                if (online)
                {
                    if ((bool)new W_CardInfo().ShowDialog())
                        PerfromPayment();
                }
                else
                    PerfromPayment();
            }
                
            else
            {
                MessageBox.Show("Выберите способ оплаты");
                return;
            }
        }

        void PerfromPayment()
        {
            Order order = new Order()
            {
                Name = txtBoxName.Text,
                PhoneNum = txtBoxPhoneNum.Text,
                City = cmbBoxCity.SelectedIndex + 1,
                Adress = txtBoxAdress.Text,
                PaymentMethod = paymentMethod,
                Price = Convert.ToDecimal(priceEnd),
                OrderDate = DateTime.Now,
                Status = 1,
                PaymentMethods = Manager.Context.PaymentMethods.Where(pm => pm.MethodID == paymentMethod).First(),
                OrderStatus = Manager.Context.OrderStatuses.Where(o => o.StatusID == 1).First(),
            };

            if (Manager.ClientID > 0)
            {
                order.Discount = Manager.Context.Clients.Where(c => c.ID == Manager.ClientID).FirstOrDefault().Discount;
                order.ClientID = Manager.ClientID;
            }

            if (paymentMethod == 1) 
            {
                order.CardNum = Manager.CardNum;
            }
            if (paymentMethod == 3)
            {
                if (txtBoxRemainder.Text.Length > 0)
                {
                    if (int.Parse(txtBoxRemainder.Text) < priceEnd)
                    {
                        MessageBox.Show("\"Остаток с\" не может быть меньше цены.", "Не верно указаны данные");
                        return;
                    }
                    decimal remainder = Convert.ToDecimal(txtBoxRemainder.Text);
                    order.RemainderFrom = remainder;
                }
            }

            try
            {
                int entryID = 0;
                Manager.Context.Orders.Add(order);
                Manager.Context.SaveChanges();
                if (Manager.Context.Orders.ToList().Count > 0)
                    entryID = Manager.Context.Orders.ToList().LastOrDefault().EntryID;
                foreach (ClientCart slot in Manager.Cart)
                {
                    Manager.Context.OrderCarts.Add(new OrderCart()
                    {
                        OrderID = entryID,
                        FoodID = slot.FoodID,
                        Quanity = slot.Quanity,
                    });
                }
                Manager.Context.SaveChanges();
                MessageBox.Show($"Заказ успешно оформлен!\nНомер вашего заказа {entryID}\nЖдите доставки.");
                Manager.FrameContainer_First.Navigate(new P_FoodMenu());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении данных\n~~~~~~\n" + ex.Message + "\n~~~~~~\n" + ex.InnerException, "Ошибка");
                return;
            }

            try
            {
                Manager.Cart.Clear();
                Manager.CartAmmount.Text = "0";
                if (Manager.ClientID > 0)
                {
                    DbSet<ClientCart> cart = Manager.Context.ClientCarts;
                    cart.RemoveRange(cart.Where(c => c.ClientID == Manager.ClientID));

                    Client user = Manager.Context.Clients.Where(c => c.ID == Manager.ClientID).FirstOrDefault();
                    double summOfOrders = 0.0;
                    foreach (Order item in user.Orders)
                        summOfOrders += Convert.ToDouble(item.Price);
                    int discount = (int)(summOfOrders / 5000) + 1;
                    if (discount > 30) discount = 30;
                    if (discount < 1) discount = 1;
                    user.Discount = discount;
                    Manager.Context.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.ToString(), "Ошибка");
                return;
            }
            
        }

        private void rBtnOnline_Checked(object sender, RoutedEventArgs e)
        {
            paymentMethod = 1;
            txtBoxRemainder.IsEnabled = false;
        }

        private void rBtnCardOnHands_Checked(object sender, RoutedEventArgs e)
        {
            paymentMethod = 2;
            txtBoxRemainder.IsEnabled = false;
        }

        private void rBtnCashOnHands_Checked(object sender, RoutedEventArgs e)
        {
            paymentMethod = 3;
            txtBoxRemainder.IsEnabled = true;
        }

        private void txtBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
