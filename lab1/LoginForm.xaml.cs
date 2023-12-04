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
        Account account;
        LoginData rmm = new LoginData();
        public static bool canRegister = true;
        public LoginForm()
        {
            InitializeComponent();
            account = new Account();
            this.DataContext = account;
            //LoginData lg = new LoginData();
            var loginfo = RememberMeManager.ReadLoginData();
            rememberMe.IsChecked = loginfo.AutoLogin;
            logLoginBox.Text = loginfo.Login;
            logPasswordBox.Password = loginfo.Password;
            if(loginfo.AutoLogin==true)
            {
                doLogin(loginfo.Login,loginfo.Password);
            }
        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            AuthLogic auth = new AuthLogic();
            Account account = new Account();

            if(canRegister)
            if (regLoginBox.Text.Trim() != "" &&
                regNameBox.Text.Trim() != "" &&
                regPasswordBox1.Text.Trim() != "" &&
                regEmailBox.Text.Trim() != "")
                if (regPasswordBox1.Text.Trim() == regPasswordBox2.Text.Trim())
                {
                    account.Name = regNameBox.Text.Trim();
                    account.Email = regEmailBox.Text.Trim();
                    account.Login = regLoginBox.Text.Trim();
                    account.Type = 2;
                    account.Phone = String.Empty;
                    account.Password = regPasswordBox1.Text.Trim();

                    auth.Register(account);
                    
                    MessageBox.Show("Пользователь успешно зарегестрирован!");
                }
                else MessageBox.Show("Пароли не совпадают!");
            else MessageBox.Show("Данные не заполнены!");
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            doLogin(logLoginBox.Text.Trim(), logPasswordBox.Password.Trim());
        }
        public void doLogin(string login, string pass)
        {
            AuthLogic auth = new AuthLogic();
            int id = auth.Login(login, pass);
            if (id != -1)
            {
                rmm.Login = login; 
                rmm.Password = "";
                rmm.LastLogin = DateTime.Now;
                rmm.AutoLogin = rememberMe.IsChecked;
                if (rememberMe.IsChecked == true)
                    rmm.Password = pass;
                RememberMeManager.SaveLoginData(rmm);
                MainWindow mw = new MainWindow(id);
                this.Hide();
                mw.ShowDialog();
                account = new Account();
                this.Show();
            }
            else MessageBox.Show("Неверный логин или пароль!");
        }

        private void showPassword_Checked(object sender, RoutedEventArgs e)
        {
          
        }

        private void logPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
        }

        private void logShowPasswordBox_Copy_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            rmm.AutoLogin = rememberMe.IsChecked;
            RememberMeManager.SaveLoginData(rmm);
        }
    }
}
