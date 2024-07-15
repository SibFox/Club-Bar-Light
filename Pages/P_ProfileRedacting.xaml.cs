using BarLight.Common;
using BarLight.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BarLight.Pages
{
    public partial class P_ProfileRedacting : Page
    {
        Client user = new Client();
        public P_ProfileRedacting()
        {
            InitializeComponent();
            user = Manager.Context.Clients.Where(c => c.ID == Manager.ClientID).FirstOrDefault();

            txtBoxName.Focus();
            txtBoxName.Text = user.Name;
            txtBoxSurname.Text = user.Surname;
            txtBoxPatronymic.Text = user.Patronymic;
            txtBoxPNum.Text = user.PhoneNum;
            txtBoxEMail.Text = user.EMail;
            cmbBoxCity.SelectedIndex = user.City != null ? (int)user.City - 1 : 0;
            txtBoxAdress.Text = user.Adress;
            txtBoxLogin.Text = user.Login;
        }

        private void btnRedAccept_Click(object sender, RoutedEventArgs e)
        {
            if (txtBoxName.Text.Trim().Replace(" ", "").Length < 1 ||
                txtBoxSurname.Text.Trim().Replace(" ", "").Length < 1 ||
                txtBoxLogin.Text.Trim().Replace(" ", "").Length < 1)
            {
                MessageBox.Show("Некоторые поля для редактирования пусты");
                return;
            }

            if (txtBoxPNum.Text.Length != 10 && txtBoxPNum.Text.Length > 0)
            {
                MessageBox.Show("Номер телефона введён не верно");
                return;
            }

            if (txtBoxPassword.Text.Trim().Replace(" ", "").Length < 1 ||
                txtBoxPasswordCheck.Text.Trim().Replace(" ", "").Length < 1)
            {
                MessageBox.Show("Пароль обязателен для заполнения");
                return;
            }

            if (txtBoxPassword.Text != txtBoxPasswordCheck.Text)
            {
                MessageBox.Show("Пароли не совпадают");
                return;
            }

            try
            {
                if (cmbBoxCity.SelectedIndex != -1 && txtBoxAdress.Text.Trim().Replace(" ", "").Length > 0)
                {
                    user.City = cmbBoxCity.SelectedIndex + 1;
                    user.Adress = txtBoxAdress.Text;
                }
                else
                {
                    MessageBox.Show("Город или адресс не могут быть введены раздельно.");
                    return;
                }
                user.Name = txtBoxName.Text;
                user.Surname = txtBoxSurname.Text;
                user.Patronymic = txtBoxPatronymic.Text;
                user.EMail = txtBoxEMail.Text;
                user.PhoneNum = txtBoxPNum.Text;
                user.Login = txtBoxLogin.Text;
                user.Password = txtBoxPassword.Text;

                Manager.Context.SaveChanges();
                MessageBox.Show("Данные успешно сохранены!");
                Manager.FrameContainer_First.Navigate(new P_Profile());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при редактировании данных.\n~~~~~~\n" + ex.Message, "Ошибка");
            }
            
        }

        private void btnRedDecline_Click(object sender, RoutedEventArgs e)
        {
            Manager.FrameContainer_First.Navigate(new P_Profile());
        }
    }
}
