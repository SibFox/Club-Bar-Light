using BarLight.Common;
using BarLight.Model;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BarLight.Pages
{
    public partial class P_TableReservation : Page
    {
        public P_TableReservation()
        {
            InitializeComponent();

            lstViewEstablishments.ItemsSource = Manager.Context.Establishments.ToList();

            if (Manager.ChoosenEstablishment != null)
                Manager.ChoosenEstablishment = null;
            if (Manager.IsELButtonNotNull())
            {
                Manager.ChangeELButton("Забронировать столик");
                Manager.EstablishmentButton = null;
            }

            if (Manager.ClientID > 0)
            {
                Client client = Manager.Context.Clients.Where(c => c.ID == Manager.ClientID).FirstOrDefault();
                txtBoxName.Text = client.Name;
                txtBoxPNum.Text = client.PhoneNum;
            }

            cmbBoxTime.Items.Add("09:00");
            cmbBoxTime.SelectedIndex = cmbBoxTime.Items.Count - 1;
            int hour = 9;
            int minute = 0;
            while (cmbBoxTime.SelectedItem.ToString() != "23:45")
            {
                minute += 15;
                if (minute >= 60)
                {
                    minute = 0;
                    hour++;
                }
                cmbBoxTime.Items.Add($"{hour:00}:{minute:00}");
                cmbBoxTime.SelectedIndex = cmbBoxTime.Items.Count - 1;

                if (hour > 25)
                    break;
            }

            cmbBoxTime.SelectedIndex = 0;
        }

        private void txtBoxNumeric_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void txtBoxText_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^а-яА-Я]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btnSelectRes_Click(object sender, RoutedEventArgs e)
        {
            Manager.ChoosenEstablishment = (sender as Button).DataContext as Establishment;
            if (Manager.IsELButtonNotNull())
                Manager.ChangeELButton("Забронировать столик");
            Manager.EstablishmentButton = sender as Button;
            Manager.ChangeELButton("Выбрано");
        }

        private void btnAccRes_Click(object sender, RoutedEventArgs e)
        {
            if (Manager.ChoosenEstablishment == null)
            {
                MessageBox.Show("Выберите заведение, в котором хотите сделать бронь");
                return;
            }

            StringBuilder stringBuilder = new StringBuilder();
            if (txtBoxName.Text.Trim().Replace(" ", "").Length < 1)
                stringBuilder.AppendLine("Введите ваше имя.");
            if (txtBoxPNum.Text.Trim().Replace(" ", "").Length < 10)
                stringBuilder.AppendLine("Введите ваш телефон.");
            if (!dtpResDate.SelectedDate.HasValue)
                stringBuilder.AppendLine("Выберите дату бронирования.");
            if (cmbBoxTime.SelectedIndex < 0)
                stringBuilder.AppendLine("Выберите время бронирования.");

            if (stringBuilder.Length > 0)
            {
                MessageBox.Show(stringBuilder.ToString());
                return;
            }
            if (dtpResDate.SelectedDate <= DateTime.Now)
            {
                MessageBox.Show("Дата бронирования должна быть позже сегодняшнего дня");
                return;
            }

            DateTime resTime = dtpResDate.SelectedDate.Value;
            int hour = Convert.ToInt32(cmbBoxTime.Text.Split(':')[0]);
            int minute = Convert.ToInt32(cmbBoxTime.Text.Split(':')[1]);
            resTime = resTime.AddMinutes(minute);
            resTime = resTime.AddHours(hour);
            try
            {
                ReservedTable reservedTable = new ReservedTable()
                {
                    EstablishmentID = Manager.ChoosenEstablishment.ID,
                    ClientID = (Nullable<int>)Manager.ClientID > 0 ? (Nullable<int>)Manager.ClientID : null,
                    ReserveDate = resTime,
                    Name = txtBoxName.Text,
                    PhoneNum = txtBoxPNum.Text,
                };
                Manager.Context.ReservedTables.Add(reservedTable);
                Manager.Context.SaveChanges();

                ReservedTable table = Manager.Context.ReservedTables.ToList().LastOrDefault();
                MessageBox.Show($"Столик успешно зарезервирован на имя {table.Name}\nПо адресу {table.Establishment.Adress}\nНа время: {table.ReserveDate:dd MMM, HH:mm}", "Успешно зарезервираванно");

                Manager.FrameContainer_First.Navigate(new P_TableReservation());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при записи данных\n~~~~~~\n" + ex.Message + "\n~~~~~~\n" + ex.InnerException, "Ошибка");
                return;
            }
        }
    }
}
