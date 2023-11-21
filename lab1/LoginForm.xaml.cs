using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using lab1.Logic;
using lab1.Model;

namespace lab1
{
    /// <summary>
    /// Логика взаимодействия для LoginForm.xaml
    /// </summary>
    public partial class LoginForm : Window
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            AuthLogic auth = new AuthLogic();
            Account account = new Account();

            if (regLoginBox.Text.Trim() != "" &&
                regNameBox.Text.Trim() != "" &&
                regPasswordBox1.Text.Trim() != "" &&
                regEmailBox.Text.Trim() != "")
                if (regPasswordBox1.Text.Trim() == regPasswordBox2.Text.Trim())
                {
                    account.Name = regNameBox.Text.Trim();
                    account.Email = regEmailBox.Text.Trim();
                    account.Login = regLoginBox.Text.Trim();
                    account.Password = BCrypt.Net.BCrypt.HashPassword(regPasswordBox1.Text.Trim());

                    auth.Register(account);
                    
                    MessageBox.Show("Пользователь успешно зарегестрирован!");
                }
                else MessageBox.Show("Пароли не совпадают!");
            else MessageBox.Show("Данные не заполнены!");
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            AuthLogic auth = new AuthLogic();
            int id = auth.Login(logLoginBox.Text.Trim(), logPasswordBox.Text.Trim());
            if (id != -1)
            {
                MessageBox.Show("Авторизован пользователь: " + id.ToString());
                MainWindow mw = new MainWindow(id);
                this.Hide();
                mw.ShowDialog();
                this.Show();
            }
            else MessageBox.Show("Неверный логин или пароль!");
        }
    }
}
