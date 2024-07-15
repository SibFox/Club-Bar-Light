using BarLight.Common;
using BarLight.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BarLight.Pages
{
    public partial class P_Registration : Page
    {
        public P_Registration()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (txtBoxName.Text.Trim().Replace(" ", "").Length < 1 ||
                txtBoxSurname.Text.Trim().Replace(" ", "").Length < 1 ||
                txtBoxLogin.Text.Trim().Replace(" ", "").Length < 1 ||
                txtBoxPassword.Text.Trim().Replace(" ", "").Length < 1 ||
                txtBoxPasswordCheck.Text.Trim().Replace(" ", "").Length < 1)
            {
                MessageBox.Show("Обязательные поля должны быть заполнены.");
                return;
            }

            if (txtBoxPNum.Text.Length != 10 && txtBoxPNum.Text.Length > 0)
            {
                MessageBox.Show("Номер телефона введён не верно");
                return;
            }

            if (txtBoxPassword.Text != txtBoxPasswordCheck.Text)
            {
                MessageBox.Show("Пароли не идентичны");
                return;
            }


            List<Client> clients = Manager.Context.Clients.ToList();
            if (clients.Contains(clients.Find(c => c.Login == txtBoxLogin.Text)))
            {
                MessageBox.Show("Профиль с таким логином уже зарегестрированн");
                return;
            }
            if (clients.Contains(clients.Find(c => c.PhoneNum == txtBoxPNum.Text)))
            {
                MessageBox.Show("Профиль с таким телефоном уже зарегестрированн");
                return;
            }

            int lastID = clients.LastOrDefault().ID;


            Client newClient = new Client()
            {
                ID = lastID + 1,
                Name = txtBoxName.Text,
                Surname = txtBoxSurname.Text,
                Patronymic = txtBoxPatronymic.Text,
                PhoneNum = txtBoxPNum.Text,
                City = cmbBoxCity.SelectedIndex + 1,
                Adress = txtBoxAdress.Text,
                Login = txtBoxLogin.Text,
                Password = txtBoxPassword.Text,
                EMail = txtBoxEMail.Text,
                Discount = 1,
            };

            try
            {
                Manager.Context.Clients.Add(newClient);
                Manager.Context.SaveChanges();
                MessageBox.Show("Профиль создан!");
                Manager.FrameContainer_First.Navigate(new P_Authorization());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении.\n~~~~~~\n" + ex.ToString(), "Ошибка");
            }
        }
    }
}
