using BarLight.Common;
using System;
using System.Windows;

namespace BarLight.Windows
{
    public partial class W_CardInfo : Window
    {
        public W_CardInfo()
        {
            InitializeComponent();
        }

        private void btnAcc_Click(object sender, RoutedEventArgs e)
        {
            if (txtBoxCardNum.Text.Trim().Replace(" ", "").Replace("_", "").Length < 16 ||
                txtBoxCVV.Text.Trim().Replace(" ", "").Replace("_", "").Length < 3 ||
                txtBoxDate.Text.Trim().Replace(" ", "").Replace("_", "").Replace("/", "").Length < 4)
            {
                MessageBox.Show("Не все поля заполнены");
                return;
            }
            int month = Convert.ToInt32(txtBoxDate.Text.Replace("_", "").Split('/')[0]);
            int year = Convert.ToInt32(txtBoxDate.Text.Replace("_", "").Split('/')[1]) + 2000;

            if (month < 1 || month > 12)
            {
                MessageBox.Show("Параметр месяца должен находится в диапозоне от 1 до 12", "Неверный ввод данных");
                return;
            }
            if (year < 0)
            {
                MessageBox.Show("Параметр года не может быть отрицательным", "Неверный ввод данных");
                return;
            }

            if (DateTime.Now > new DateTime(year, month, DateTime.Now.Day))
            {
                MessageBox.Show("Срок карты истёк");
                return;
            }

            Manager.CardNum = txtBoxCardNum.Text.Trim().Replace(" ", "");

            DialogResult = true;
            Close();

        }
    }
}
